using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Core.ComponentModel
{
	/// <summary>
	/// Represents telecom options for a patient.
	/// </summary>
	public class TelecomOptions
	{
		/// <summary>
		/// Initializes a new instance of the TelecomOptions class.
		/// </summary>
		public TelecomOptions()
		{
			EmailAddresses = new List<string>();
			PhoneNumbers = new List<string>();
		}

		/// <summary>
		/// The email addresses of the patient.
		/// </summary>
		public List<string> EmailAddresses { get; set; }

		/// <summary>
		/// The phone numbers of the patient.
		/// </summary>
		public List<string> PhoneNumbers { get; set; }
	}
}
