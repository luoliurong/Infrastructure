using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.FaultHandling
{
	public class RetryPolicy
	{
		#region private sealed classes.
		private sealed class IgnoreErrorStrategy : IFaultDetectionStrategy
		{
			public bool IsFaultException(Exception ex)
			{
				return false;
			}
		}

		private sealed class CatchAllErrorStrategy : IFaultDetectionStrategy
		{
			public bool IsFaultException(Exception ex)
			{
				return true;
			}
		}
		#endregion

		private static RetryPolicy noRetry = new RetryPolicy(new IgnoreErrorStrategy(), RetryStrategy.NoRetry);
		private static RetryPolicy defaultFixed = new RetryPolicy(new CatchAllErrorStrategy(), RetryStrategy.DefaultFixed);
		private static RetryPolicy defaultProgressive = new RetryPolicy(new CatchAllErrorStrategy(), RetryStrategy.DefaultProgressive);
		private static RetryPolicy defaultExponential = new RetryPolicy(new CatchAllErrorStrategy(), RetryStrategy.DefaultExponential);

		public static RetryPolicy NoRetry
		{
			get { return noRetry; }
		}

		public static RetryPolicy DefaultFixed
		{
			get { return defaultFixed; }
		}

		public static RetryPolicy DefaultProgressive
		{
			get { return defaultProgressive; }
		}

		public static RetryPolicy DefaultExponential
		{
			get { return defaultExponential; }
		}

		public RetryStrategy RetryStrategy { get; private set; }
		public IFaultDetectionStrategy FaultDetectionStrategy { get; private set; }

		public event EventHandler<RetryingEventArgs> Retrying;

		#region ctor
		public RetryPolicy(IFaultDetectionStrategy faultDetectionStrategy, RetryStrategy retryStrategy)
		{
			Common.Ensure.IsNotNull(faultDetectionStrategy);
			Common.Ensure.IsNotNull(retryStrategy);

			this.FaultDetectionStrategy = faultDetectionStrategy;
			this.RetryStrategy = retryStrategy;
		}

		public RetryPolicy(IFaultDetectionStrategy faultDetectionStrategy, int retryCount)
			: this(faultDetectionStrategy, new FixedInterval(retryCount))
		{
		}

		public RetryPolicy(IFaultDetectionStrategy faultDetectionStrategy, int retryCount, TimeSpan retryInterval)
			: this(faultDetectionStrategy, new FixedInterval(retryCount, retryInterval))
		{
		}

		public RetryPolicy(IFaultDetectionStrategy faultDetectionStrategy, int retryCount, TimeSpan minBackoff, TimeSpan maxBackoff, TimeSpan deltaBackoff)
			: this(faultDetectionStrategy, new ExponentialBackOffInterval(retryCount, minBackoff, maxBackoff, deltaBackoff))
		{
		}

		public RetryPolicy(IFaultDetectionStrategy faultDetectionStrategy, int retryCount, TimeSpan initialInterval, TimeSpan increment)
			: this(faultDetectionStrategy, new IncrementalInterval(retryCount, initialInterval, increment))
		{
		}
		#endregion

		public virtual void ExecuteAction(Action action)
		{
			Common.Ensure.IsNotNull(action);

			this.ExecuteAction(() => { action(); return default(object); });
		}
		public virtual TResult ExecuteAction<TResult>(Func<TResult> func)
		{
			Common.Ensure.IsNotNull(func);

			int retryCount = 0;
			TimeSpan delay = TimeSpan.Zero;
			Exception lastError;

			var shouldRetry = this.RetryStrategy.GetShouldRetry();

			for (;;)
			{
				lastError = null;

				try
				{
					return func();
				}
#pragma warning disable 0618
				catch (RetryLimitExceededException limitExceededEx)
#pragma warning restore 0618
				{
					// The user code can throw a RetryLimitExceededException to force the exit from the retry loop.
					// The RetryLimitExceeded exception can have an inner exception attached to it. This is the exception
					// which we will have to throw up the stack so that callers can handle it.
					if (limitExceededEx.InnerException != null)
					{
						throw limitExceededEx.InnerException;
					}
					else
					{
						return default(TResult);
					}
				}
				catch (Exception ex)
				{
					lastError = ex;

					if (!(this.FaultDetectionStrategy.IsFaultException(lastError) && shouldRetry(retryCount++, lastError, out delay)))
					{
						throw;
					}
				}

				// Perform an extra check in the delay interval. Should prevent from accidentally ending up with the value of -1 that will block a thread indefinitely. 
				// In addition, any other negative numbers will cause an ArgumentOutOfRangeException fault that will be thrown by Thread.Sleep.
				if (delay.TotalMilliseconds < 0)
				{
					delay = TimeSpan.Zero;
				}

				this.OnRetrying(retryCount, lastError, delay);

				if (retryCount > 1 || !this.RetryStrategy.FastFirstRetry)
				{
					Task.Delay(delay).Wait();
				}
			}
		}

		public Task ExecuteAsync(Func<Task> taskAction)
		{
			return this.ExecuteAsync(taskAction, default(CancellationToken));
		}

		public Task ExecuteAsync(Func<Task> taskAction, CancellationToken cancellationToken)
		{
			Common.Ensure.IsNotNull(taskAction);

			return new AsyncExecution(
					taskAction,
					this.RetryStrategy.GetShouldRetry(),
					this.FaultDetectionStrategy.IsFaultException,
					this.OnRetrying,
					this.RetryStrategy.FastFirstRetry,
					cancellationToken)
				.ExecuteAsync();
		}

		public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> taskFunc)
		{
			return this.ExecuteAsync<TResult>(taskFunc, default(CancellationToken));
		}

		public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> taskFunc, CancellationToken cancellationToken)
		{
			Common.Ensure.IsNotNull(taskFunc);

			return new AsyncExecution<TResult>(
					taskFunc,
					this.RetryStrategy.GetShouldRetry(),
					this.FaultDetectionStrategy.IsFaultException,
					this.OnRetrying,
					this.RetryStrategy.FastFirstRetry,
					cancellationToken)
				.ExecuteAsync();
		}

		protected virtual void OnRetrying(int retryCount, Exception lastError, TimeSpan delay)
		{
			if (this.Retrying != null)
			{
				this.Retrying(this, new RetryingEventArgs(retryCount, delay, lastError));
			}
		}
	}

	public class RetryPolicy<T> : RetryPolicy where T : IFaultDetectionStrategy, new()
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RetryPolicy{T}"/> class with the specified number of retry attempts and parameters defining the progressive delay between retries.
		/// </summary>
		/// <param name="retryStrategy">The strategy to use for this retry policy.</param>
		public RetryPolicy(RetryStrategy retryStrategy)
			: base(new T(), retryStrategy)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RetryPolicy{T}"/> class with the specified number of retry attempts and the default fixed time interval between retries.
		/// </summary>
		/// <param name="retryCount">The number of retry attempts.</param>
		public RetryPolicy(int retryCount)
			: base(new T(), retryCount)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RetryPolicy{T}"/> class with the specified number of retry attempts and a fixed time interval between retries.
		/// </summary>
		/// <param name="retryCount">The number of retry attempts.</param>
		/// <param name="retryInterval">The interval between retries.</param>
		public RetryPolicy(int retryCount, TimeSpan retryInterval)
			: base(new T(), retryCount, retryInterval)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RetryPolicy{T}"/> class with the specified number of retry attempts and backoff parameters for calculating the exponential delay between retries.
		/// </summary>
		/// <param name="retryCount">The number of retry attempts.</param>
		/// <param name="minBackoff">The minimum backoff time.</param>
		/// <param name="maxBackoff">The maximum backoff time.</param>
		/// <param name="deltaBackoff">The time value that will be used to calculate a random delta in the exponential delay between retries.</param>
		public RetryPolicy(int retryCount, TimeSpan minBackoff, TimeSpan maxBackoff, TimeSpan deltaBackoff)
			: base(new T(), retryCount, minBackoff, maxBackoff, deltaBackoff)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RetryPolicy{T}"/> class with the specified number of retry attempts and parameters defining the progressive delay between retries.
		/// </summary>
		/// <param name="retryCount">The number of retry attempts.</param>
		/// <param name="initialInterval">The initial interval that will apply for the first retry.</param>
		/// <param name="increment">The incremental time value that will be used to calculate the progressive delay between retries.</param>
		public RetryPolicy(int retryCount, TimeSpan initialInterval, TimeSpan increment)
			: base(new T(), retryCount, initialInterval, increment)
		{
		}
	}
}
