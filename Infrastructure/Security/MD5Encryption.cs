using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common;
using System.Security.Cryptography;

namespace Infrastructure.Security
{
	/// <summary>
	/// A utility class to do MD5 encryption.
	/// </summary>
	public class MD5Encryption
	{
		public static string Encrypt(string text)
		{
			return Encrypt(text, Encoding.UTF8);
		}

		public static string Encrypt(string text, Encoding encoding)
		{
			Ensure.IsNotNullOrEmpty(text);
			using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
			{
				var textbytes = encoding.GetBytes(text);
				var computedHash = md5.ComputeHash(textbytes);
				var result = BitConverter.ToString(computedHash);
				return result.Replace("-", "");
			}
		}
	}
}
