using PatientGenerator.Messaging.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Messaging.Model
{
	public class GenerationResponse
	{
		public GenerationResponse()
		{
		}

		public List<string> Messages { get; set; }

		public bool HasErrors { get; set; }
	}
}
