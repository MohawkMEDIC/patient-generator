/*
 * Copyright 2016-2016 Mohawk College of Applied Arts and Technology
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you
 * may not use this file except in compliance with the License. You may
 * obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * User: Nityan
 * Date: 2016-2-15
 */

using PatientGenerator.Core.ComponentModel;
using PatientGenerator.Core.Validation;
using System.Collections.Generic;

namespace PatientGenerator.Messaging.Validation
{
	/// <summary>
	/// Provides a utility for validating messages.
	/// </summary>
	public static class ValidationUtil
	{
		/// <summary>
		/// Validates a message.
		/// </summary>
		/// <param name="options">The message to be validated.</param>
		/// <returns>Returns an IEnumerable<IResultDetail> containing validation errors.</returns>
		public static IEnumerable<IResultDetail> ValidateMessage(DemographicOptions options)
		{
			List<IResultDetail> details = new List<IResultDetail>();

			if (string.IsNullOrEmpty(options.Metadata.AssigningAuthority) || string.IsNullOrWhiteSpace(options.Metadata.AssigningAuthority))
			{
				details.Add(new MandatoryElementMissingResultDetail(ResultDetailType.Error, nameof(options.Metadata.AssigningAuthority) + " cannot be null or empty."));
			}

			if (options.DateOfBirthOptions?.Start != null && options.DateOfBirthOptions?.Exact != null)
			{
				details.Add(new ConflictingValueResultDetail(ResultDetailType.Error, nameof(options.DateOfBirthOptions) + " DateOfBirthOptions.End must be populated if DateOfBirthOptions.Start is populated."));
			}

			if (options.DateOfBirthOptions?.End != null && options.DateOfBirthOptions?.Exact != null)
			{
				details.Add(new ConflictingValueResultDetail(ResultDetailType.Error, nameof(options.DateOfBirthOptions) + " DateOfBirthOptions.Start must be populated if DateOfBirthOptions.Start is populated."));
			}

			if ((options.DateOfBirthOptions?.Start != null && options.DateOfBirthOptions?.End != null) && options.DateOfBirthOptions?.Exact != null)
			{
				details.Add(new ConflictingValueResultDetail(ResultDetailType.Error, nameof(options.DateOfBirthOptions) + " cannot have all fields populated."));
			}

			if (options.Metadata.UseHL7v2 || options.Metadata.UseHL7v3)
			{
				if (string.IsNullOrEmpty(options.Metadata.ReceivingApplication) || string.IsNullOrWhiteSpace(options.Metadata.ReceivingApplication))
				{
					details.Add(new MandatoryElementMissingResultDetail(ResultDetailType.Error, nameof(options.Metadata.ReceivingApplication) + " cannot be null or empty."));
				}

				if (string.IsNullOrEmpty(options.Metadata.ReceivingFacility) || string.IsNullOrWhiteSpace(options.Metadata.ReceivingFacility))
				{
					details.Add(new MandatoryElementMissingResultDetail(ResultDetailType.Error, nameof(options.Metadata.ReceivingFacility) + " cannot be null or empty."));
				}

				if (string.IsNullOrEmpty(options.Metadata.SendingApplication) || string.IsNullOrWhiteSpace(options.Metadata.SendingApplication))
				{
					details.Add(new MandatoryElementMissingResultDetail(ResultDetailType.Error, nameof(options.Metadata.SendingApplication) + " cannot be null or empty."));
				}

				if (string.IsNullOrEmpty(options.Metadata.SendingFacility) || string.IsNullOrWhiteSpace(options.Metadata.SendingFacility))
				{
					details.Add(new MandatoryElementMissingResultDetail(ResultDetailType.Error, nameof(options.Metadata.SendingFacility) + " cannot be null or empty."));
				}
			}
			else
			{
				details.Add(new MandatoryElementMissingResultDetail(ResultDetailType.Error, "Must specify FHIR, HL7v2, or HL7v3"));
			}

			return details;
		}
	}
}