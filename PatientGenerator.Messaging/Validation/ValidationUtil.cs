using PatientGenerator.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Messaging.Validation
{
	public static class ValidationUtil
	{
		public static IEnumerable<IResultDetail> ValidateMessage(DemographicOptions options)
		{
			List<IResultDetail> details = new List<IResultDetail>();

			if (options.AssigningAuthority == null)
			{
				details.Add(new MandatoryElementMissingResultDetail(ResultDetailType.Error, nameof(options.AssigningAuthority) + " cannot be null."));
			}

			if ((options.DateOfBirthOptions?.Start != null && options.DateOfBirthOptions?.End != null) && options.DateOfBirthOptions?.Exact != null)
			{
				details.Add(new ConflictingValueResultDetail(ResultDetailType.Error, nameof(options.DateOfBirthOptions) + " cannot have all fields populated. Either Start and End OR Exact"));
			}

			if (options.UseHL7v2)
			{
				if (options.ReceivingApplication == null)
				{
					details.Add(new MandatoryElementMissingResultDetail(ResultDetailType.Error, nameof(options.ReceivingApplication) + " cannot be null."));
				}

				if (options.ReceivingFacility == null)
				{
					details.Add(new MandatoryElementMissingResultDetail(ResultDetailType.Error, nameof(options.ReceivingFacility) + " cannot be null."));
				}

				if (options.SendingApplication == null)
				{
					details.Add(new MandatoryElementMissingResultDetail(ResultDetailType.Error, nameof(options.SendingApplication) + " cannot be null."));
				}

				if (options.SendingFacility == null)
				{
					details.Add(new MandatoryElementMissingResultDetail(ResultDetailType.Error, nameof(options.SendingFacility) + " cannot be null."));
				}
			}

			return details;
		}
	}
}
