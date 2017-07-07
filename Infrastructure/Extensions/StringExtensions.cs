using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Extensions
{
	public static class StringExtensions
	{
		public static bool IsEmpty(this string text)
		{
			return string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
		}

		public static byte[] ToBytes(this string text, Encoding encoding)
		{
			return encoding.GetBytes(text);
		}

		public static bool IsEmail(this string text, bool isRestrictMode)
		{
			if (IsEmpty(text))
			{
				return false;
			}
			string pattern = isRestrictMode ? Constants.RestrictEmailRegExp : Constants.SimpleEmailRegExp;
			return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
		}

		public static bool IsMobilePhoneNumber(this string text)
		{
			if(IsEmpty(text))
			{
				return false;
			}

			string pattern = Constants.MobilePhoneRegExp;
			return Regex.IsMatch(text, pattern);
		}

		public static bool IsIdCardNumber(this string text)
		{
			if (IsEmpty(text))
			{
				return false;
			}
			var pattern = Constants.IDCardNo18RegExp;
			if (text.Length == 15)
			{
				pattern = Constants.IDCardNo15RegExp;
			}
			return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
		}

		public static bool IsIPAddress(this string text)
		{
			if (IsEmpty(text))
			{
				return false;
			}
			return Regex.IsMatch(text, Constants.IPAddressRegExp, RegexOptions.IgnoreCase);
		}
	}
}
