using System;

namespace PatientGenerator.Core.ComponentModel
{
	public class DateOfBirthOptions
	{
		public DateOfBirthOptions()
		{
		}

		public DateTime End { get; set; }

		public DateTime Start { get; set; }

		public DateTime Exact { get; set; }
	}
}