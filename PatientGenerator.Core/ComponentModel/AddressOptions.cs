using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Core.ComponentModel
{
	/// <summary>
	/// Represents address options for a patient.
	/// </summary>
	public class AddressOptions
	{
		/// <summary>
		/// Initializes a new instance of the AddressOptions class.
		/// </summary>
		public AddressOptions()
		{
		}

		/// <summary>
		/// The city of the patient's address.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// The country of the patient's address.
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// The Zip/Postal Code of the patient's address.
		/// </summary>
		public string ZipPostalCode { get; set; }

		/// <summary>
		/// The State/Province of the patient's address.
		/// </summary>
		public string StateProvince { get; set; }

		/// <summary>
		/// The street address of the patient.
		/// </summary>
		public string StreetAddress { get; set; }
	}
}
