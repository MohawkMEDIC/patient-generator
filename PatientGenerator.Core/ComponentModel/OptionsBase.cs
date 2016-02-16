namespace PatientGenerator.Core.ComponentModel
{
	/// <summary>
	/// Represents a base class for options.
	/// </summary>
	public class OptionsBase
	{
		/// <summary>
		/// Initializes a new instance of the OptionsBase class.
		/// </summary>
		protected OptionsBase()
		{
		}

		/// <summary>
		/// The assigning authority to which the patient belongs.
		/// </summary>
		public string AssigningAuthority { get; set; }

		/// <summary>
		/// The receiving application.
		/// </summary>
		public string ReceivingApplication { get; set; }

		/// <summary>
		/// The receiving facility.
		/// </summary>
		public string ReceivingFacility { get; set; }

		/// <summary>
		/// The sending application.
		/// </summary>
		public string SendingApplication { get; set; }

		/// <summary>
		/// The sending facility.
		/// </summary>
		public string SendingFacility { get; set; }

		/// <summary>
		/// When true, the application will generate patients using HL7v2 messages.
		/// </summary>
		public bool UseHL7v2 { get; set; }
	}
}