using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Security;

namespace Infrastructure.Test.Security
{
	[TestClass]
	public class MD5EncryptionTests
	{
		[TestMethod]
		public void Given_DifferentStrings_Then_EncryptReturns32Characters()
		{
			var source1 = "helloword";
			var source2 = "other";

			var result1 = MD5Encryption.Encrypt(source1);
			var result2 = MD5Encryption.Encrypt(source2);
			Assert.AreNotEqual(result1, result2);
			Assert.AreEqual(32, result1.Length);
			Assert.AreEqual(32, result2.Length);
		}
	}
}
