using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
	public class Ensure
	{
		private static readonly string ARGUMENTNULLEMPTY = "Argument is null or empty.";

		public static void IsNotNullOrEmpty(string text)
		{
			if(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentNullException(nameof(text), ARGUMENTNULLEMPTY);
			}
		}
	}
}
