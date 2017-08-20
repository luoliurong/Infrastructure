using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace Infrastructure.Common
{
	public class Ensure
	{
		private static readonly ResourceManager resourceMgr;
		private static readonly CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;
		private static readonly string ARGUMENTNULLEMPTY = "ArgumentNullOrEmptyError";
		private static readonly string ARGUMENTNULL = "ArgumentNullError";
		private static readonly string INVALIDPOSITIVENUMBER = "InvalidPositiveNumber";
		private static readonly string ArgumentCannotBeGreaterThanBaseline = "ArgumentCannotBeGreaterThanBaseline";

		static Ensure()
		{
			resourceMgr = StaticResource.StaticResource.ResourceManager;
		}
		
		public static void IsNotNullOrEmpty(string text)
		{
			if(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
			{
				var exceptionMsg = GetMessageTextFromResource(ARGUMENTNULLEMPTY);
				throw new ArgumentNullException("text", exceptionMsg);
			}
		}

		public static void IsNotNull(object obj)
		{
			if(null == obj)
			{
				var exceptionMsg = GetMessageTextFromResource(ARGUMENTNULL);
				throw new ArgumentNullException("obj", exceptionMsg);
			}
		}

		public static void IsPositiveNumber(int number)
		{
			if(number < 0)
			{
				var exceptionMsg = GetMessageTextFromResource(INVALIDPOSITIVENUMBER);
				throw new ArgumentOutOfRangeException("number", exceptionMsg);
			}
		}

		public static void IsPositiveNumber(long number)
		{
			if (number < 0)
			{
				var exceptionMsg = GetMessageTextFromResource(INVALIDPOSITIVENUMBER);
				throw new ArgumentOutOfRangeException("number", exceptionMsg);
			}
		}

		public static void ArgumentNotGreaterThan(double argumentValue, double ceilingValue, string argumentName)
		{
			if (argumentValue > ceilingValue)
			{
				var message = string.Format(GetMessageTextFromResource(ArgumentCannotBeGreaterThanBaseline), argumentName, ceilingValue);
				throw new ArgumentOutOfRangeException(argumentName, argumentValue, message);
			}
		}

		private static string GetMessageTextFromResource(string resourceKey)
		{
			return resourceMgr.GetString(resourceKey, currentCulture);
		}
	}
}
