/*
 * Copyright 2016-2017 Mohawk College of Applied Arts and Technology
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
using PatientGenerator.FHIR.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json.Linq;
using PatientGenerator.Core.Model.ComponentModel;

namespace PatientGenerator.FHIR
{
	/// <summary>
	/// Represents a utility for generating FHIR messages using the HL7 FHIR framework.
	/// </summary>
	public static class FhirUtil
	{
		/// <summary>
		/// The configuration.
		/// </summary>
		private static readonly FhirConfigurationSection configuration = ConfigurationManager.GetSection("medic.patientgen.fhir") as FhirConfigurationSection;

		/// <summary>
		/// The trace source.
		/// </summary>
		private static readonly TraceSource traceSource = new TraceSource("PatientGenerator.FHIR");

		/// <summary>
		/// Generates the candidate registry.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns>Patient.</returns>
		public static Patient GenerateCandidateRegistry(DemographicOptions options)
		{
			var patient = new Patient
			{
				Active = true,
				Address = new List<Address>()
			};

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
					PostalCode = address.ZipPostalCode
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
					var startYear = options.DateOfBirthOptions.Start.Value.Year;
					var endYear = options.DateOfBirthOptions.End.Value.Year;

					var startMonth = options.DateOfBirthOptions.Start.Value.Month;
					var endMonth = options.DateOfBirthOptions.End.Value.Month;

					var startDay = options.DateOfBirthOptions.Start.Value.Day;
					var endDay = options.DateOfBirthOptions.End.Value.Day;

					patient.BirthDate = new DateTime(new Random().Next(startYear, endYear), new Random().Next(startMonth, endMonth), new Random().Next(startDay, endDay)).ToString("yyyy-MM-dd");
				}
				else
				{
					// DateTime.Now.Year - 1 is for generating people whose birthdays are always 1 year behind the current to avoid people being created with future birthdays
					patient.BirthDate = new DateTime(new Random().Next(1900, DateTime.Now.Year - 1), new Random().Next(1, 12), new Random().Next(1, 28)).ToString("yyyy-MM-dd");
				}
			}

			switch (options.Gender)
			{
				case "F":
				case "f":
				case "female":
				case "Female":
					patient.Gender = AdministrativeGender.Female;
					break;
				case "M":
				case "m":
				case "male":
				case "Male":
					patient.Gender = AdministrativeGender.Male;
					break;
				case "O":
				case "o":
				case "other":
				case "Other":
					patient.Gender = AdministrativeGender.Other;
					break;
				default:
					patient.Gender = AdministrativeGender.Unknown;
					break;
			}

			patient.Identifier = new List<Identifier>();
			patient.Identifier.AddRange(options.OtherIdentifiers.Select(i => new Identifier(i.AssigningAuthority, i.Value)));

			patient.Name = new List<HumanName>();

			foreach (var name in options.Names)
			{
				var humanName = new HumanName
				{
					Family = name.LastName,
					Given = new List<string>
					{
						name.FirstName
					}
				};

				humanName.Given = new List<string>(name.MiddleNames)
				{
					name.FirstName
				};

				patient.Name.Add(humanName);
			}

			patient.Telecom = new List<ContactPoint>();

			patient.Telecom.AddRange(options.TelecomOptions.EmailAddresses.Select(e => new ContactPoint(ContactPoint.ContactPointSystem.Email, ContactPoint.ContactPointUse.Mobile, e)));
			patient.Telecom.AddRange(options.TelecomOptions.PhoneNumbers.Select(p => new ContactPoint(ContactPoint.ContactPointSystem.Phone, ContactPoint.ContactPointUse.Mobile, p)));

			return patient;
		}

		public static Patient GenerateCandidateRegistry(Core.Model.Common.Patient patient, Core.Model.Metadata metadata)
		{
			var fhirPatient = new Patient
			{
				Active = true,
				Address = new List<Address>
				{
					new Address
					{
						City = patient.City,
						Country = patient.Country,
						Line = new List<string>
						{
							patient.AddressLine
						},
						PostalCode = patient.PostalCode,
						State = patient.Province,
						Use = Address.AddressUse.Home
					}
				},
				BirthDate = patient.DateOfBirth.ToString("yyyy-MM-dd"),
				Communication = new List<Patient.CommunicationComponent>
				{
					new Patient.CommunicationComponent
					{
						Language = new CodeableConcept("urn:ietf:bcp:47", "en", "English"),
						Preferred = true
					},
					new Patient.CommunicationComponent
					{
						Language = new CodeableConcept("urn:ietf:bcp:47", "sw", "Swahili")
					}
				},
				Contact = new List<Patient.ContactComponent>
				{
					new Patient.ContactComponent
					{
						Name = new HumanName
						{
							Family = patient.LastName,
							Given = new List<string>
							{
								"Smith",
								"Mary"
							},
							Use = HumanName.NameUse.Official
						},
						Relationship = new List<CodeableConcept>
						{
							new CodeableConcept("", "MTH", "Mother")
						}
					}
				},
				Deceased = new FhirDateTime(DateTime.Now),
				Id = Guid.NewGuid().ToString(),
				Identifier = new List<Identifier>
				{
					new Identifier($"urn:oid:{metadata.AssigningAuthority}", patient.HealthCardNo)
				},
				MultipleBirth = new FhirBoolean(true),
				Name = new List<HumanName>
				{
					new HumanName
					{
						Family = patient.LastName,
						Given = new List<string>
						{
							patient.FirstName,
							patient.MiddleName
						},
						Use = HumanName.NameUse.Official
					}
				},
				Telecom = new List<ContactPoint>
				{
					new ContactPoint(ContactPoint.ContactPointSystem.Phone, ContactPoint.ContactPointUse.Mobile, patient.PhoneNo)
				}
			};

			switch (patient.Gender)
			{
				case "F":
				case "f":
				case "female":
				case "Female":
					fhirPatient.Gender = AdministrativeGender.Female;
					break;
				case "M":
				case "m":
				case "male":
				case "Male":
					fhirPatient.Gender = AdministrativeGender.Male;
					break;
				case "O":
				case "o":
				case "other":
				case "Other":
					fhirPatient.Gender = AdministrativeGender.Other;
					break;
				default:
					fhirPatient.Gender = AdministrativeGender.Unknown;
					break;
			}

			return fhirPatient;
		}

		/// <summary>
		/// Sends the FHIR messages.
		/// </summary>
		/// <param name="patient">The patient.</param>
		/// <returns>List&lt;System.Boolean&gt;.</returns>
		public static List<bool> SendFhirMessages(Patient patient)
		{
			var results = new List<bool>();

			foreach (var endpoint in configuration.Endpoints)
			{
				using (var client = new HttpClient())
				{
					if (endpoint.RequiresAuthorization)
					{
						client.DefaultRequestHeaders.Add("Authorization", new[] { $"Bearer {GetAuthorizationToken(endpoint)}" });
					}

					traceSource.TraceEvent(TraceEventType.Verbose, 0, "Sending FHIR message to endpoint " + endpoint);

					var content = new StringContent(FhirSerializer.SerializeResourceToXml(patient));
					content.Headers.Remove("Content-Type");
					content.Headers.Add("Content-Type", "application/xml+fhir");

					var response = client.PostAsync($"{endpoint.Address}/Patient", content).Result;

					if (response.IsSuccessStatusCode)
					{
						traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Message sent successfully, response: {response.Content.ReadAsStringAsync().Result}");
					}
					else
					{
						traceSource.TraceEvent(TraceEventType.Error, 0, $"Unable to send message, response: {response.Content.ReadAsStringAsync().Result}");
					}
				}
			}

			return results;
		}

		/// <summary>
		/// Gets the authorization token.
		/// </summary>
		/// <param name="endpoint">The endpoint.</param>
		/// <returns>Returns a string representing the authorization token.</returns>
		private static string GetAuthorizationToken(FhirEndpoint endpoint)
		{
			string accessToken;

			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Authorization", "BASIC " + Convert.ToBase64String(Encoding.UTF8.GetBytes(endpoint.AuthorizationConfiguration.ApplicationId + ":" + endpoint.AuthorizationConfiguration.ApplicationSecret)));

				var content = new StringContent($"grant_type=password&username={endpoint.AuthorizationConfiguration.Username}&password={endpoint.AuthorizationConfiguration.Password}&scope={endpoint.AuthorizationConfiguration.Scope}");

				// HACK: have to remove the headers before adding them...
				content.Headers.Remove("Content-Type");
				content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

				var result = client.PostAsync(endpoint.AuthorizationConfiguration.Endpoint, content).Result;
				var response = JObject.Parse(result.Content.ReadAsStringAsync().Result);

				accessToken = response.GetValue("access_token").ToString();
				traceSource.TraceEvent(TraceEventType.Verbose, 0, $"Access token: {accessToken}");
			}

			return accessToken;

		}
	}
}