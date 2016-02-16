using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Messaging.Validation
{
	public class ConflictingValueDetail : ResultDetail
	{
		public ConflictingValueDetail(string message) : base(message)
		{
		}

		public ConflictingValueDetail(ResultDetailType type, string message) : base(type, message)
		{

		}

		public ConflictingValueDetail(ResultDetailType type, string message, Exception exception) : base(type, message, exception)
		{
		}

		public ConflictingValueDetail(ResultDetailType type, string message, string location, Exception exception) : base(type, message, location, exception)
		{
		}
	}
}
