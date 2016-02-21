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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.FHIR
{
	public static class FhirUtil
	{
		public static void GenerateCandidateRegistry(DemographicOptions options)
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

			if (options.DateOfBirthOptions.Exact.HasValue)
			{
				patient.BirthDate = options.DateOfBirthOptions?.Exact.ToString();
			}
			else if (options.DateOfBirthOptions.Start.HasValue && options.DateOfBirthOptions.End.HasValue)
			{
				int startYear = options.DateOfBirthOptions.Start.Value.Year;
				int endYear = options.DateOfBirthOptions.End.Value.Year;

				int startMonth = options.DateOfBirthOptions.Start.Value.Month;
				int endMonth = options.DateOfBirthOptions.End.Value.Month;

				int startDay = options.DateOfBirthOptions.Start.Value.Day;
				int endDay = options.DateOfBirthOptions.End.Value.Day;

				patient.BirthDate = new DateTime(new Random().Next(startYear, endYear), new Random().Next(startMonth, endMonth), new Random().Next(startDay, endDay)).ToString();
			}
			else
			{
				patient.BirthDate = new DateTime(new Random().Next(1900, 2015), new Random().Next(1, 12), new Random().Next(1, 28)).ToString();
			}

			patient.Name = new List<HumanName>();
		}

		public static void SendFhirMessages(Patient patient)
		{
			FhirClient client = new FhirClient(new Uri(""));

			OperationOutcome outcome = null;

			client.TryValidateCreate(patient, out outcome);

			if (outcome.Success())
			{
				var result = client.Create(patient, null, true);
			}
		}
	}
}
