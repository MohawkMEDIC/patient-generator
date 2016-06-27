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
 * Date: 2016-2-12
 */

using MARC.Everest.Connectors;
using MARC.Everest.Connectors.WCF;
using MARC.Everest.DataTypes;
using MARC.Everest.DataTypes.Interfaces;
using MARC.Everest.Formatters.XML.Datatypes.R1;
using MARC.Everest.Formatters.XML.ITS1;
using MARC.Everest.Interfaces;
using MARC.Everest.RMIM.CA.R020402.Interactions;
using MARC.Everest.RMIM.CA.R020402.Vocabulary;
using MARC.Everest.Xml;
using PatientGenerator.Core.ComponentModel;
using System;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace PatientGenerator.HL7v3
{
	public static class EverestUtil
	{
		/// <summary>
		/// Generates a HL7v3 PRPA_IN101201CA register patient request.
		/// </summary>
		/// <param name="patient">The patient options to be used when creating the demographics for the patient.</param>
		/// <returns>Returns a PRPA_IN101201CA as an IGraphable.</returns>
		public static IGraphable GenerateCandidateRegistry(DemographicOptions patient)
		{
			PRPA_IN101201CA registerPatientRequest = new PRPA_IN101201CA(
				Guid.NewGuid(),
				DateTime.Now,
				ResponseMode.Immediate,
				PRPA_IN101201CA.GetInteractionId(),
				PRPA_IN101201CA.GetProfileId(),
				ProcessingID.Production,
				AcknowledgementCondition.Always,
				new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Receiver(
					new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Device2(
						new II("1.3.6.1.4.1.33349.3.1.1.20.4", patient.ReceivingFacility)
					)
				),
				new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Sender(
					new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Device1(
						new II("1.3.6.1.4.1.33349.3.1.2.99121.283", patient.SendingFacility)
					)
				)
			);

			TS dob = new TS
			{
				NullFlavor = NullFlavor.NoInformation
			};

			if (patient.DateOfBirthOptions != null)
			{
				if (patient.DateOfBirthOptions.Exact.HasValue)
				{
					dob = new TS(patient.DateOfBirthOptions.Exact.Value, DatePrecision.Day);
				}
			}

			registerPatientRequest.controlActEvent = PRPA_IN101201CA.CreateControlActEvent(
				Guid.NewGuid(),
				PRPA_IN101201CA.GetTriggerEvent(),
				new MARC.Everest.RMIM.CA.R020402.MFMI_MT700711CA.Author(),
				new MARC.Everest.RMIM.CA.R020402.MFMI_MT700711CA.Subject2<MARC.Everest.RMIM.CA.R020402.PRPA_MT101001CA.IdentifiedEntity>(
					true,
					new MARC.Everest.RMIM.CA.R020402.MFMI_MT700711CA.RegistrationRequest<MARC.Everest.RMIM.CA.R020402.PRPA_MT101001CA.IdentifiedEntity>(
						new MARC.Everest.RMIM.CA.R020402.MFMI_MT700711CA.Subject4<MARC.Everest.RMIM.CA.R020402.PRPA_MT101001CA.IdentifiedEntity>(
							new MARC.Everest.RMIM.CA.R020402.PRPA_MT101001CA.IdentifiedEntity(
									null,
									RoleStatus.Active,
									null,
									x_VeryBasicConfidentialityKind.Normal,
									new MARC.Everest.RMIM.CA.R020402.PRPA_MT101002CA.Person(
										BuildNames(patient),
										BuildTelecoms(patient),
										Util.Convert<AdministrativeGender>(patient.Gender),
										dob,
										false,
										null,
										false,
										null,
										BuildAddresses(patient),
										null,
										null,
										new MARC.Everest.RMIM.CA.R020402.PRPA_MT101104CA.LanguageCommunication(new CV<string>("eng", "2.16.840.1.113883.6.121"), true)
									)
							)
						),
						new MARC.Everest.RMIM.CA.R020402.REPC_MT230003CA.Custodian(
							new MARC.Everest.RMIM.CA.R020402.COCT_MT090310CA.AssignedDevice(
								new II("2.16.840.1.113883.3.239.18.61", Guid.NewGuid().ToString("N")),
								new MARC.Everest.RMIM.CA.R020402.COCT_MT090310CA.Repository("KNG"),
								new MARC.Everest.RMIM.CA.R020402.COCT_MT090310CA.RepositoryJurisdiction("KGHD")
							)
						)
					)
				)
			);

			registerPatientRequest.controlActEvent.EffectiveTime = new IVL<TS>(DateTime.Now);

			// Author
			registerPatientRequest.controlActEvent.Author.Time = new TS(DateTime.Now);
			registerPatientRequest.controlActEvent.Author.SetAuthorPerson(
				new MARC.Everest.RMIM.CA.R020402.COCT_MT090102CA.AssignedEntity(
					new SET<II>(new II("2.16.840.1.113883.3.239.18.1", Guid.NewGuid().ToString("N"))),
					new MARC.Everest.RMIM.CA.R020402.COCT_MT090108CA.Person(
						new PN(
							EntityNameUse.Legal,
							new ENXP[] {
								new ENXP("Fyfe", EntityNamePartType.Family),
								new ENXP("Justin", EntityNamePartType.Given),
								new ENXP
								{
									Qualifier = new SET<CS<EntityNamePartQualifier>> { new CS<EntityNamePartQualifier>(EntityNamePartQualifier.Prefix) },
									Value = "Dr."
								}
							}
						),
						new MARC.Everest.RMIM.CA.R020402.COCT_MT090108CA.HealthCareProvider() { NullFlavor = NullFlavor.NoInformation }
					)
				)
			);

			registerPatientRequest.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id = new SET<II>();

			if (patient.OtherIdentifiers.Count == 0)
			{
				// must have one alternate identifier
				registerPatientRequest.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id.Add(new II("1.3.6.1.4.1.33349.3.1.2.99121.283", Guid.NewGuid().ToString("N")));
			}

			foreach (var otherIdentifier in patient.OtherIdentifiers)
			{
				registerPatientRequest.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id.Add(new II(otherIdentifier.Key, otherIdentifier.Value));
			}

			registerPatientRequest.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.EffectiveTime = new IVL<TS>(DateTime.Now);

#if DEBUG
			LogGraphable(registerPatientRequest);
#endif

			bool isValid = registerPatientRequest.Validate();

			return registerPatientRequest;
		}

		public static IGraphable GenerateCandidateRegistry(PatientGenerator.Core.Common.Patient patient)
		{
			return null;
		}

		/// <summary>
		/// Logs an IGraphable message.
		/// </summary>
		/// <param name="graphable">The IGraphable message to log.</param>
		internal static void LogGraphable(IGraphable graphable)
		{
			XmlWriter writer = null;

			XmlIts1Formatter formatter = new XmlIts1Formatter
			{
				ValidateConformance = false
			};

			DatatypeFormatter dtf = new DatatypeFormatter();

			formatter.GraphAides.Add(dtf);

			StringBuilder sb = new StringBuilder();

			writer = XmlWriter.Create(sb, new XmlWriterSettings() { Indent = true, OmitXmlDeclaration = true });

			XmlStateWriter stateWriter = new XmlStateWriter(writer);

			var result = formatter.Graph(stateWriter, graphable);

			stateWriter.Flush();

			Trace.TraceInformation(sb.ToString());
		}

		/// <summary>
		/// Send HL7v3 messages to a specified endpoint.
		/// </summary>
		/// <param name="epName">The endpoint name.</param>
		public static bool Sendv3Messages(IGraphable graphable, string endpointName)
		{
			bool retVal = true;

			WcfClientConnector client = new WcfClientConnector(string.Format("endpointName={0}", endpointName));

			XmlIts1Formatter formatter = new XmlIts1Formatter
			{
				ValidateConformance = true
			};

			client.Formatter = formatter;
			client.Formatter.GraphAides.Add(new DatatypeFormatter());

			client.Open();

			var sendResult = client.Send(graphable);

#if DEBUG
			Trace.TraceInformation("Sending HL7v3 message to endpoint: " + client.ConnectionString);
#endif

			if (sendResult.Code != ResultCode.Accepted && sendResult.Code != ResultCode.AcceptedNonConformant)
			{
				Trace.TraceError("Send result: " + Enum.GetName(typeof(ResultCode), sendResult.Code));
				retVal = false;
			}

			var recvResult = client.Receive(sendResult);

			if (recvResult.Code != ResultCode.Accepted && recvResult.Code != ResultCode.AcceptedNonConformant)
			{
				Trace.TraceError("Receive result: " + Enum.GetName(typeof(ResultCode), recvResult.Code));
				retVal = false;
			}

			var result = recvResult.Structure;

			if (result == null)
			{
				Trace.TraceError("Receive result structure is null");
				retVal = false;
			}

			client.Close();

			return retVal;
		}

		/// <summary>
		/// Builds a list of addresses for a patient.
		/// </summary>
		/// <param name="patient">The patient for which to build the addresses.</param>
		/// <returns>Returns a list of addresses for a patient. LIST<AD> </returns>
		private static LIST<AD> BuildAddresses(DemographicOptions patient)
		{
			LIST<AD> addresses = new LIST<AD>();

			foreach (var item in patient.Addresses)
			{
				ADXP city;
				ADXP country;
				ADXP postal;
				ADXP state;
				ADXP street;

				if (item.City == null)
				{
					city = new ADXP { NullFlavor = NullFlavor.NoInformation };
				}
				else
				{
					city = new ADXP(item.City, AddressPartType.City);
				}

				if (item.Country == null)
				{
					country = new ADXP { NullFlavor = NullFlavor.NoInformation };
				}
				else
				{
					country = new ADXP(item.Country, AddressPartType.Country);
				}

				if (item.ZipPostalCode == null)
				{
					postal = new ADXP { NullFlavor = NullFlavor.NoInformation };
				}
				else
				{
					postal = new ADXP(item.ZipPostalCode, AddressPartType.PostalCode);
				}

				if (item.StateProvince == null)
				{
					state = new ADXP { NullFlavor = NullFlavor.NoInformation };
				}
				else
				{
					state = new ADXP(item.StateProvince, AddressPartType.State);
				}

				if (item.StreetAddress == null)
				{
					street = new ADXP { NullFlavor = NullFlavor.NoInformation };
				}
				else
				{
					street = new ADXP(item.StreetAddress, AddressPartType.StreetAddressLine);
				}

				addresses.Add(new AD(new ADXP[]
				{
					city,
					country,
					postal,
					state,
					street
				}));
			}

			return addresses;
		}

		/// <summary>
		/// Builds a list of name for a patient.
		/// </summary>
		/// <param name="patient">The patient for which to build the names.</param>
		/// <returns>Returns a list of name for a patient. LIST<PN> </returns>
		private static LIST<PN> BuildNames(DemographicOptions patient)
		{
			LIST<PN> personNames = new LIST<PN>();

			foreach (var item in patient.Names)
			{
				personNames.Add(new PN(EntityNameUse.Legal, new ENXP[]
				{
					new ENXP(item.FirstName, EntityNamePartType.Given),
					new ENXP(item.LastName, EntityNamePartType.Family)
				}));
			}

			return personNames;
		}

		/// <summary>
		/// Builds a list of telecoms for a patient.
		/// </summary>
		/// <param name="patient">The patient for which to build the telecoms.</param>
		/// <returns>Returns a list of telecoms for a patient. LIST<TEL> </returns>
		private static LIST<TEL> BuildTelecoms(DemographicOptions patient)
		{
			LIST<TEL> telecoms = new LIST<TEL>();

			if (patient.TelecomOptions.EmailAddresses.Count == 0)
			{
				// TODO: supply random email address
				telecoms.Add(new TEL("admin@example.com", TelecommunicationAddressUse.Direct));
			}

			if (patient.TelecomOptions.PhoneNumbers.Count == 0)
			{
				// TODO: supply random telephone number
				telecoms.Add(new TEL("9055751212", TelecommunicationAddressUse.WorkPlace));
			}

			foreach (var email in patient.TelecomOptions.EmailAddresses)
			{
				telecoms.Add(new TEL(email, TelecommunicationAddressUse.Direct));
			}

			foreach (var phone in patient.TelecomOptions.PhoneNumbers)
			{
				telecoms.Add(new TEL(phone));
			}

			return telecoms;
		}
	}
}