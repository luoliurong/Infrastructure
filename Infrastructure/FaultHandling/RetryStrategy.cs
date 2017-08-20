using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FaultHandling
{
	/// <summary>
	/// a callback delegate that will be invoked whenever a retry condition is encountered.
	/// </summary>
	/// <param name="retryCount">retry attemp count</param>
	/// <param name="lastException">the exception that caused the retry condition to occur</param>
	/// <param name="delay">how long the current thread will be suspended before the next interaction is invokes.</param>
	/// <returns><see langword="true" /> if a retry is allowed; otherwise, <see langword="false"/>.</returns>
	public delegate bool ShouldRetry(int retryCount, Exception lastException, out TimeSpan delay);

	/// <summary>
	/// a retry strategy that determines the number of retry attempts and the interval between retries.
	/// </summary>
	public abstract class RetryStrategy
	{
		public static readonly int DEFAULTRETRYCOUNT = 10;
		public static readonly TimeSpan DEFAULTBACKOFFTIME = TimeSpan.FromSeconds(10.0);
		public static readonly TimeSpan DEFAULTMAXBACKOFFTIME = TimeSpan.FromSeconds(30.0);
		public static readonly TimeSpan DEFAULTMINBACKOFFTIME = TimeSpan.FromSeconds(1.0);
		public static readonly TimeSpan DEFAULTRETRYINTERVAL = TimeSpan.FromSeconds(1.0);
		public static readonly TimeSpan DEFAULTRETRYINCREMENT = TimeSpan.FromSeconds(1.0);
		//indicate whether the first retry attemp will be made immediately,
		//whereas subsequent retries will remain subject to the retry interval.
		public static readonly bool DEFAULTFIRSTFASTRETRY = true;


		private static RetryStrategy noRetry = new FixedInterval(0, DEFAULTRETRYINTERVAL);
		private static RetryStrategy defaultFixed = new FixedInterval(DEFAULTRETRYCOUNT, DEFAULTRETRYINTERVAL);
		private static RetryStrategy defaultProgressive = new IncrementalInterval(DEFAULTRETRYCOUNT, DEFAULTRETRYINTERVAL, DEFAULTRETRYINCREMENT);
		private static RetryStrategy defaultExponential = new ExponentialBackOffInterval(DEFAULTRETRYCOUNT, DEFAULTMINBACKOFFTIME, DEFAULTMAXBACKOFFTIME, DEFAULTBACKOFFTIME);

		public static RetryStrategy NoRetry
		{
			get { return noRetry; }
		}

		public static RetryStrategy DefaultFixed
		{
			get { return defaultFixed; }
		}

		public static RetryStrategy DefaultProgressive
		{
			get { return defaultProgressive; }
		}

		public static RetryStrategy DefaultExponential
		{
			get { return defaultExponential; }
		}

		protected RetryStrategy(string name, bool firstFastRetry)
		{
			this.Name = name;
			this.FastFirstRetry = firstFastRetry;
		}

		public bool FastFirstRetry { get; set; }
		public string Name { get; private set; }

		public abstract ShouldRetry GetShouldRetry();
	}
}
