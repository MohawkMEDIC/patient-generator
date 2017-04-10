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
using PatientGenerator.Core.Model.ComponentModel;
using System;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace PatientGenerator.HL7v3
{
	/// <summary>
	/// Represents a utility for generating HL7v3 messages using the Everest framework.
	/// </summary>
	public static class EverestUtility
	{
		/// <summary>
		/// The tracer source.
		/// </summary>
		private static readonly TraceSource traceSource = new TraceSource("PatientGenerator.HL7v3");

		/// <summary>
		/// Generates a HL7v3 PRPA_IN101201CA register patient request.
		/// </summary>
		/// <param name="patient">The patient options to be used when creating the demographics for the patient.</param>
		/// <returns>Returns a PRPA_IN101201CA as an IGraphable.</returns>
		/// <exception cref="System.InvalidOperationException">Message is not valid v3, cannot send</exception>
		public static IGraphable GenerateCandidateRegistry(Demographic patient)
		{
			var registerPatientRequest = new PRPA_IN101201CA(
				Guid.NewGuid(),
				DateTime.Now,
				ResponseMode.Immediate,
				PRPA_IN101201CA.GetInteractionId(),
				PRPA_IN101201CA.GetProfileId(),
				ProcessingID.Production,
				AcknowledgementCondition.Always,
				new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Receiver(
					new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Device2(
						new II("1.3.6.1.4.1.33349.3.1.1.20.4", patient.Metadata.ReceivingFacility)
					)
				),
				new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Sender(
					new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Device1(
						new II("1.3.6.1.4.1.33349.3.1.2.99121.283", patient.Metadata.SendingFacility)
					)
				)
			);

			var dob = new TS
			{
				NullFlavor = NullFlavor.NoInformation
			};

			if (patient.DateOfBirthOptions?.Exact != null)
			{
				dob = new TS(patient.DateOfBirthOptions.Exact.Value, DatePrecision.Day);
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
				var alternateIdentifier = new II("1.3.6.1.4.1.33349.3.1.2.99121.283", Guid.NewGuid().ToString("N"));

				traceSource.TraceEvent(TraceEventType.Information, 0, $"Patient must have at least one alternate identifier, adding alternate identifier:{alternateIdentifier.Root} {alternateIdentifier.Extension}");

				registerPatientRequest.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id.Add(alternateIdentifier);
			}

			foreach (var otherIdentifier in patient.OtherIdentifiers)
			{
				registerPatientRequest.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id.Add(new II(otherIdentifier.AssigningAuthority, otherIdentifier.Value));
			}

			registerPatientRequest.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.EffectiveTime = new IVL<TS>(DateTime.Now);

			LogMessage(registerPatientRequest);

			if (!registerPatientRequest.Validate())
			{
				throw new InvalidOperationException("Message is not valid v3, cannot send");
			}

			return registerPatientRequest;
		}

		/// <summary>
		/// Send HL7v3 messages to a specified endpoint.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="endpointName">Name of the endpoint.</param>
		/// <returns><c>true</c> If the message sent successfully, <c>false</c> otherwise.</returns>
		public static bool Sendv3Messages(IGraphable message, string endpointName)
		{
			var retVal = true;

			var client = new WcfClientConnector($"endpointName={endpointName}");

			var formatter = new XmlIts1Formatter
			{
				ValidateConformance = true
			};

			client.Formatter = formatter;
			client.Formatter.GraphAides.Add(new DatatypeFormatter());

			client.Open();

			var sendResult = client.Send(message);

			traceSource.TraceEvent(TraceEventType.Verbose, 0, "Sending HL7v3 message to endpoint: " + client.ConnectionString);

			if (sendResult.Code != ResultCode.Accepted && sendResult.Code != ResultCode.AcceptedNonConformant)
			{
				traceSource.TraceEvent(TraceEventType.Error, 0, "Send result: " + Enum.GetName(typeof(ResultCode), sendResult.Code));
				retVal = false;
			}

			var receiveResult = client.Receive(sendResult);

			if (receiveResult.Code != ResultCode.Accepted && receiveResult.Code != ResultCode.AcceptedNonConformant)
			{
				traceSource.TraceEvent(TraceEventType.Error, 0, "Receive result: " + Enum.GetName(typeof(ResultCode), receiveResult.Code));
				retVal = false;
			}

			var result = receiveResult.Structure;

			if (result == null)
			{
				traceSource.TraceEvent(TraceEventType.Error, 0, "Receive result structure is null");
				retVal = false;
			}

			client.Close();

			return retVal;
		}

		/// <summary>
		/// Builds a list of addresses for a patient.
		/// </summary>
		/// <param name="patient">The patient for which to build the addresses.</param>
		/// <returns>Returns a list of addresses for a patient.</returns>
		private static LIST<AD> BuildAddresses(Demographic patient)
		{
			var addresses = new LIST<AD>();

			foreach (var item in patient.Addresses)
			{
				var city = item.City == null ? new ADXP { NullFlavor = NullFlavor.NoInformation } : new ADXP(item.City, AddressPartType.City);
				var country = item.Country == null ? new ADXP { NullFlavor = NullFlavor.NoInformation } : new ADXP(item.Country, AddressPartType.Country);
				var postal = item.ZipPostalCode == null ? new ADXP { NullFlavor = NullFlavor.NoInformation } : new ADXP(item.ZipPostalCode, AddressPartType.PostalCode);
				var state = item.StateProvince == null ? new ADXP { NullFlavor = NullFlavor.NoInformation } : new ADXP(item.StateProvince, AddressPartType.State);
				var street = item.StreetAddress == null ? new ADXP { NullFlavor = NullFlavor.NoInformation } : new ADXP(item.StreetAddress, AddressPartType.StreetAddressLine);

				addresses.Add(new AD(new[]
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
		/// <returns>Returns a list of name for a patient.</returns>
		private static LIST<PN> BuildNames(Demographic patient)
		{
			var personNames = new LIST<PN>();

			foreach (var item in patient.Names)
			{
				personNames.Add(new PN(EntityNameUse.Legal, new[]
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
		/// <returns>Returns a list of telecoms for a patient.</returns>
		private static LIST<TEL> BuildTelecoms(Demographic patient)
		{
			var telecoms = new LIST<TEL>();

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

		/// <summary>
		/// Logs the message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		private static void LogMessage(IGraphable message)
		{
			XmlWriter writer = null;

			var formatter = new XmlIts1Formatter
			{
				ValidateConformance = true,
			};

			var dtf = new DatatypeFormatter();

			formatter.GraphAides.Add(dtf);

			var sb = new StringBuilder();

			writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true });

			var stateWriter = new XmlStateWriter(writer);

			formatter.Graph(stateWriter, message);

			stateWriter.Flush();

			traceSource.TraceEvent(TraceEventType.Verbose, 0, sb.ToString());
		}
	}
}