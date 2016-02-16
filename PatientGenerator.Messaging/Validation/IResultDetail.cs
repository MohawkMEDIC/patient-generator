using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Messaging.Validation
{
	public interface IResultDetail
	{
		Exception Exception { get; }

		string Location { get; set; }

		string Message { get; }

		ResultDetailType Type { get; }
	}
}
