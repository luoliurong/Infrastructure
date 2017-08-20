using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FaultHandling
{
	public class FixedInterval : RetryStrategy
	{
		private readonly int retryCount;
		private readonly TimeSpan retryInterval;

		public FixedInterval()
			: this(DEFAULTRETRYCOUNT)
		{ }

		public FixedInterval(int retryCount)
			: this(retryCount, DEFAULTRETRYINTERVAL)
		{ }

		public FixedInterval(int retryCount, TimeSpan retryInterval)
			: this(null, retryCount, retryInterval, DEFAULTFIRSTFASTRETRY)
		{ }

		public FixedInterval(string name, int retryCount, TimeSpan retryInterval)
			: this(name, retryCount, retryInterval, DEFAULTFIRSTFASTRETRY)
		{ }

		public FixedInterval(string name, int retryCount, TimeSpan retryInterval, bool firstFastRetry)
			:base(name, firstFastRetry)
		{
			Common.Ensure.IsPositiveNumber(retryCount);
			Common.Ensure.IsPositiveNumber(retryInterval.Ticks);

			this.retryCount = retryCount;
			this.retryInterval = retryInterval;
		}

		public override ShouldRetry GetShouldRetry()
		{
			if(this.retryCount == 0)
			{
				return delegate (int currentRetryCount, Exception lastException, out TimeSpan interval)
				{
					interval = TimeSpan.Zero;
					return false;
				};
			}

			return delegate (int currentRetryCount, Exception lastException, out TimeSpan interval)
			{
				if(currentRetryCount<this.retryCount)
				{
					interval = this.retryInterval;
					return true;
				}

				interval = TimeSpan.Zero;
				return false;
			};
		}
	}
}
