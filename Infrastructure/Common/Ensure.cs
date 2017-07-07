using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace Infrastructure.Common
{
	public class Ensure
	{
		private static readonly CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;
		private static readonly string ARGUMENTNULLEMPTY = "ArgumentNullOrEmptyError";
		private static readonly ResourceManager resourceMgr;

		static Ensure()
		{
			resourceMgr = StaticResource.StaticResource.ResourceManager;
		}
		
		public static void IsNotNullOrEmpty(string text)
		{
			if(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentNullException(nameof(text), resourceMgr.GetString(ARGUMENTNULLEMPTY, currentCulture));
			}
		}
	}
}
