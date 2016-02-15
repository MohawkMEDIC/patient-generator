using System.Collections.Generic;

namespace PatientGenerator.Core
{
	public class DemographicOptions : OptionsBase
	{
		public DemographicOptions()
		{
		}

		public List<AddressOptions> Addresses { get; set; }

		public DateOfBirthOptions DateOfBirthOptions { get; set; }

		public List<NameOptions> Names { get; set; }

		public string Gender { get; set; }

		public string PersonIdentifier { get; set; }

		public List<Dictionary<string, string>> OtherIdentifiers { get; set; }

		public TelecomOptions TelecomOptions { get; set; }
	}
}