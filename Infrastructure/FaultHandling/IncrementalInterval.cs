using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FaultHandling
{
	public class IncrementalInterval : RetryStrategy
	{
		private readonly int retryCount;
		private readonly TimeSpan initialInterval;
		private readonly TimeSpan increment;

		public IncrementalInterval()
			:this(DEFAULTRETRYCOUNT, DEFAULTRETRYINTERVAL, DEFAULTRETRYINCREMENT)
		{ }

		public IncrementalInterval(int retryCount, TimeSpan initialInterval, TimeSpan increment)
			: this(null, retryCount, initialInterval, increment)
		{ }

		public IncrementalInterval(string name, int retryCount, TimeSpan initialInterval, TimeSpan increment)
			: this(name, retryCount, initialInterval, increment, DEFAULTFIRSTFASTRETRY)
		{ }

		public IncrementalInterval(string name, int retryCount, TimeSpan initialInterval, TimeSpan increment, bool firstFastRetry)
			:base(name, firstFastRetry)
		{
			Common.Ensure.IsPositiveNumber(retryCount);
			Common.Ensure.IsPositiveNumber(initialInterval.Ticks);
			Common.Ensure.IsPositiveNumber(increment.Ticks);

			this.retryCount = retryCount;
			this.initialInterval = initialInterval;
			this.increment = increment;
		}

		public override ShouldRetry GetShouldRetry()
		{
			return delegate (int currentRetryCount, Exception lastException, out TimeSpan retryInterval)
			{
				if (currentRetryCount < this.retryCount)
				{
					retryInterval = TimeSpan.FromMilliseconds(this.initialInterval.TotalMilliseconds + (this.increment.TotalMilliseconds * currentRetryCount));

					return true;
				}

				retryInterval = TimeSpan.Zero;

				return false;
			};
		}
	}
}
