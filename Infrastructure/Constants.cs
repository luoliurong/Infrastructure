using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
	public class Constants
	{
		public static readonly string RestrictEmailRegExp	= @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";
		public static readonly string SimpleEmailRegExp		= @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
		public static readonly string MobilePhoneRegExp		= @"^1(3[0-9]|4[57]|5[0-35-9]|8[0-9]|70)\d{8}$";
		public static readonly string IDCardNo18RegExp		= @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$";
		public static readonly string IDCardNo15RegExp		= @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$";
		public static readonly string IPAddressRegExp		= @"^(\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d\.){3}\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d$";
	}
}
