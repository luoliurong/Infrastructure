using System;

namespace Infrastructure.Extensions
{
	public static class DateTimeExtension
	{
		public static DateTime DaysAgo(this DateTime datetime, int days)
		{
			TimeSpan timespan = new TimeSpan(days, 0, 0, 0);
			return datetime.Subtract(timespan);
		}

		public static DateTime DaysAfter(this DateTime datetime, int days)
		{
			TimeSpan timespan = new TimeSpan(days, 0, 0, 0);
			return datetime.Add(timespan);
		}
	}
}
