using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Core
{
	/// <summary>
	/// Represents name options for a patient.
	/// </summary>
	public class NameOptions
	{
		/// <summary>
		/// Initializes a new instance of the NameOptions class.
		/// </summary>
		public NameOptions()
		{

		}

		/// <summary>
		/// The prefix of the patient's name.
		/// </summary>
		public string Prefix { get; set; }

		/// <summary>
		/// The first name of the patient.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// The last name of the patient.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// The middle names of the patient.
		/// </summary>
		public List<string> MiddleNames { get; set; }

		/// <summary>
		/// The suffixes of the patient's name.
		/// </summary>
		public List<string> Suffixes { get; set; }
	}
}
