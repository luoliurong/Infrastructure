using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
	public static class StringExtensions
	{
		public static bool IsEmail(this string text)
		{
			return false;
		}

		public static bool IsMobilePhoneNumber(this string text)
		{
			return false;
		}

		public static bool IsIdCardNumber(this string text)
		{
			return false;
		}

		public static bool IsIPAddress(this string text)
		{
			return false;
		}
	}
}
