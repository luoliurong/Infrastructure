using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FaultHandling
{
	public class RetryingEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RetryingEventArgs"/> class.
		/// </summary>
		/// <param name="currentRetryCount">The current retry attempt count.</param>
		/// <param name="delay">The delay that indicates how long the current thread will be suspended before the next iteration is invoked.</param>
		/// <param name="lastException">The exception that caused the retry conditions to occur.</param>
		public RetryingEventArgs(int currentRetryCount, TimeSpan delay, Exception lastException)
		{
			Common.Ensure.IsNotNull(lastException);

			this.CurrentRetryCount = currentRetryCount;
			this.Delay = delay;
			this.LastException = lastException;
		}

		/// <summary>
		/// Gets the current retry count.
		/// </summary>
		public int CurrentRetryCount { get; private set; }

		/// <summary>
		/// Gets the delay that indicates how long the current thread will be suspended before the next iteration is invoked.
		/// </summary>
		public TimeSpan Delay { get; private set; }

		/// <summary>
		/// Gets the exception that caused the retry conditions to occur.
		/// </summary>
		public Exception LastException { get; private set; }
	}
}
