using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Messaging.Validation
{
	[Serializable]
	public class ResultDetail : IResultDetail
	{
		public ResultDetail(string message)
		{
			this.Message = message;
		}

		public ResultDetail(ResultDetailType type, string message) : this(type, message, null)
		{
		}

		public ResultDetail(ResultDetailType type, string message, Exception exception) : this(type, message, null, exception)
		{

		}

		public ResultDetail(ResultDetailType type, string message, string location, Exception exception)
		{
			this.Type = type;
			this.Message = message;
			this.Location = location;
			this.Exception = exception;
		}

		public Exception Exception { get; }

		public string Location { get; set; }

		public virtual string Message { get; }

		public ResultDetailType Type { get; protected set; }

		public override string ToString()
		{
			return nameof(Type) + " " + this.Message == null ? "" : this.Message + " " + Location == null ? this.Location : "";
		}
	}
}
