using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FaultHandling
{
	[Serializable]
	public class RetryLimitExceededException : Exception
	{
	}
}
