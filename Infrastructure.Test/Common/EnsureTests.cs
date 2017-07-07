using Infrastructure.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;

namespace Infrastructure.Test.Common
{
	[TestClass]
	public class EnsureTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ErrorMessageCanBeLoadedCorrectlyFromResourceFile()
		{
			Ensure.IsNotNullOrEmpty("");
		}
	}
}
