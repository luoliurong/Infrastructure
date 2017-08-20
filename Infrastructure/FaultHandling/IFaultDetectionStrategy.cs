using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.FaultHandling
{
	public interface IFaultDetectionStrategy
	{
		bool IsFaultException(Exception ex);
	}
}
