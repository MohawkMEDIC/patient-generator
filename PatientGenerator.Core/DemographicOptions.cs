using System.Collections.Generic;

namespace PatientGenerator.Core
{
	public class DemographicOptions
	{
		public DemographicOptions()
		{
		}

		public DateOfBirthOptions DateOfBirthOptions { get; set; }

		public List<Dictionary<string, string>> PersonIdentifiers { get; set; }
	}
}