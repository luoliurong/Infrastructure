using System;

namespace Infrastructure.Common
{
	public class StringGenerator
	{
		/// <summary>
		/// 创建流水号，序号等
		/// </summary>
		/// <param name="length">要创建的字符串长度。</param>
		/// <returns></returns>
		public static string GenerateUniqueStringOfLength(int length)
		{
			Ensure.IsPositiveNumber(length);
			var stringLength = Convert.ToInt32(length);
			var currentTimeTicks = DateTime.Now.Ticks.ToString();
			return string.Empty;
		}
	}
}
