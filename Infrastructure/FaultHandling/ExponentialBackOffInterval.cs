using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FaultHandling
{
	public class ExponentialBackOffInterval : RetryStrategy
	{
		private readonly int retryCount;
		private readonly TimeSpan minBackoff;
		private readonly TimeSpan maxBackoff;
		private readonly TimeSpan deltaBackoff;

		public ExponentialBackOffInterval()
            : this(DEFAULTRETRYCOUNT, DEFAULTMINBACKOFFTIME, DEFAULTMAXBACKOFFTIME, DEFAULTBACKOFFTIME)
        {
		}

		public ExponentialBackOffInterval(int retryCount, TimeSpan minBackoff, TimeSpan maxBackoff, TimeSpan deltaBackoff)
            : this(null, retryCount, minBackoff, maxBackoff, deltaBackoff, DEFAULTFIRSTFASTRETRY)
        {
		}

		public ExponentialBackOffInterval(string name, int retryCount, TimeSpan minBackoff, TimeSpan maxBackoff, TimeSpan deltaBackoff)
            : this(name, retryCount, minBackoff, maxBackoff, deltaBackoff, DEFAULTFIRSTFASTRETRY)
        {
		}

		public ExponentialBackOffInterval(string name, int retryCount, TimeSpan minBackoff, TimeSpan maxBackoff, TimeSpan deltaBackoff, bool firstFastRetry)
            : base(name, firstFastRetry)
        {
			Common.Ensure.IsPositiveNumber(retryCount);
			Common.Ensure.IsPositiveNumber(minBackoff.Ticks);
			Common.Ensure.IsPositiveNumber(maxBackoff.Ticks);
			Common.Ensure.IsPositiveNumber(deltaBackoff.Ticks);
			Common.Ensure.ArgumentNotGreaterThan(minBackoff.TotalMilliseconds, maxBackoff.TotalMilliseconds, "minBackoff");

			this.retryCount = retryCount;
			this.minBackoff = minBackoff;
			this.maxBackoff = maxBackoff;
			this.deltaBackoff = deltaBackoff;
		}

		public override ShouldRetry GetShouldRetry()
		{
			return delegate (int currentRetryCount, Exception lastException, out TimeSpan retryInterval)
			{
				if (currentRetryCount < this.retryCount)
				{
					var random = new Random();

					var delta = (int)((Math.Pow(2.0, currentRetryCount) - 1.0) * random.Next((int)(this.deltaBackoff.TotalMilliseconds * 0.8), (int)(this.deltaBackoff.TotalMilliseconds * 1.2)));
					var interval = (int)Math.Min(checked(this.minBackoff.TotalMilliseconds + delta), this.maxBackoff.TotalMilliseconds);

					retryInterval = TimeSpan.FromMilliseconds(interval);

					return true;
				}

				retryInterval = TimeSpan.Zero;
				return false;
			};
		}
	}
}
