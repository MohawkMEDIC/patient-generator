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
 * Date: 2016-2-21
 */

using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using PatientGenerator.Core.ComponentModel;
using PatientGenerator.FHIR.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace PatientGenerator.FHIR
{
	public static class FhirUtil
	{
		private static FhirConfigurationSection configuration = ConfigurationManager.GetSection("medic.patientgen.fhir") as FhirConfigurationSection;

		public static Patient GenerateCandidateRegistry(DemographicOptions options)
		{
			Patient patient = new Patient();

			patient.Active = true;
			patient.Address = new List<Address>();

			foreach (var address in options.Addresses)
			{
				patient.Address.Add(new Address
				{
					City = address.City,
					Country = address.Country,
					Line = new List<string>
					{
						address.StreetAddress
					},
					State = address.StateProvince,
					Zip = address.ZipPostalCode
				});
			}

			if (options.DateOfBirthOptions != null)
			{
				if (options.DateOfBirthOptions.Exact.HasValue)
				{
					patient.BirthDate = options.DateOfBirthOptions?.Exact.Value.ToString("yyyy-MM-dd");
				}
				else if (options.DateOfBirthOptions.Start.HasValue && options.DateOfBirthOptions.End.HasValue)
				{
					int startYear = options.DateOfBirthOptions.Start.Value.Year;
					int endYear = options.DateOfBirthOptions.End.Value.Year;

					int startMonth = options.DateOfBirthOptions.Start.Value.Month;
					int endMonth = options.DateOfBirthOptions.End.Value.Month;

					int startDay = options.DateOfBirthOptions.Start.Value.Day;
					int endDay = options.DateOfBirthOptions.End.Value.Day;

					patient.BirthDate = new DateTime(new Random().Next(startYear, endYear), new Random().Next(startMonth, endMonth), new Random().Next(startDay, endDay)).ToString("yyyy-MM-dd");
				}
				else
				{
					/// DateTime.Now.Year - 1 is for generating people whose birthdays are always 1 year behind the current to avoid people being created with future birthdays
					patient.BirthDate = new DateTime(new Random().Next(1900, DateTime.Now.Year - 1), new Random().Next(1, 12), new Random().Next(1, 28)).ToString("yyyy-MM-dd");
				}
			}

			patient.Gender = new CodeableConcept();
			patient.Gender.Text = options.Gender;

			patient.Identifier = new List<Identifier>();

			foreach (var identifier in options.OtherIdentifiers)
			{
				patient.Identifier.Add(new Identifier(identifier.Key, identifier.Value));
			}

			patient.Name = new List<HumanName>();

			foreach (var name in options.Names)
			{
				HumanName humanName = new HumanName();

				humanName.Family = new List<string>
				{
					name.LastName
				};

				humanName.Given = new List<string>
				{
					name.FirstName
				};

				humanName.Given.ToList().AddRange(name.MiddleNames);

				patient.Name.Add(humanName);
			}

			patient.Telecom = new List<Contact>();

			foreach (var email in options.TelecomOptions.EmailAddresses)
			{
				patient.Telecom.Add(new Contact
				{
					System = Contact.ContactSystem.Email,
					Value = email
				});
			}

			foreach (var phone in options.TelecomOptions.PhoneNumbers)
			{
				patient.Telecom.Add(new Contact
				{
					System = Contact.ContactSystem.Phone,
					Value = phone
				});
			}

			return patient;
		}

		public static List<bool> SendFhirMessages(Patient patient)
		{
			List<bool> results = new List<bool>();

			foreach (var endpoint in configuration.Endpoints)
			{
				FhirClient client = new FhirClient(new Uri(endpoint.Address));

#if DEBUG
				Trace.TraceInformation("Sending FHIR message to endpoint " + endpoint.ToString());
#endif

				OperationOutcome outcome = null;

				client.TryValidateCreate(patient, out outcome);

				if (outcome != null)
				{
					Trace.TraceWarning("Pre-validation failed for FHIR message");

					foreach (var issue in outcome.Issue)
					{
						Trace.TraceWarning(outcome.Issue.Select(x => x.Details).Aggregate((current, next) => current + ", " + next));
					}
				}

				results.Add(outcome == null);

				client.Create(patient, null, true);
			}

			return results;
		}
	}
}