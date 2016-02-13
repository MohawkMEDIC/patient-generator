namespace PatientGenerator.Core
{
	public class OptionsBase
	{
		protected OptionsBase()
		{
		}

		public string ReceivingApplication { get; set; }

		public string ReceivingFacility { get; set; }

		public string SendingApplication { get; set; }

		public string SendingFacility { get; set; }
	}
}