﻿using MARC.Everest.Connectors;
using MARC.Everest.Connectors.WCF;
using MARC.Everest.DataTypes;
using MARC.Everest.DataTypes.Interfaces;
using MARC.Everest.Formatters.XML.Datatypes.R1;
using MARC.Everest.Formatters.XML.ITS1;
using MARC.Everest.Interfaces;
using MARC.Everest.RMIM.CA.R020402.Interactions;
using MARC.Everest.RMIM.CA.R020402.Vocabulary;
using MARC.Everest.Xml;
using PatientGenerator.Core;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace PatientGenerator.HL7v3
{
	public class EverestUtil
	{
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
						new II("1.3.6.1.4.1.33349.3.1.1.20.4", "MARC-W1-1")
					)
				),
				new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Sender(
					new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Device1(
						new II("1.3.6.1.4.1.33349.3.1.2.99121.283", "SEEDER")
					)
				)
			);

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
										Util.Convert<AdministrativeGender>(patient.Gender.Substring(0)),
										new TS(patient.DateOfBirthOptions.Exact, DatePrecision.Day),
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
			registerPatientRequest.controlActEvent.Author.Time = patient.DateOfBirthOptions.Exact;
			registerPatientRequest.controlActEvent.Author.SetAuthorPerson(
				new MARC.Everest.RMIM.CA.R020402.COCT_MT090102CA.AssignedEntity(
					new SET<II>(new II("2.16.840.1.113883.3.239.18.1", Guid.NewGuid().ToString("N"))),
					new MARC.Everest.RMIM.CA.R020402.COCT_MT090108CA.Person(
						new PN(
							EntityNameUse.Legal,
							new ENXP[] {
								new ENXP("Fyfe", EntityNamePartType.Family),
								new ENXP("Justin", EntityNamePartType.Given),
								new ENXP("Dr", EntityNamePartType.Prefix)
							}
						),
						new MARC.Everest.RMIM.CA.R020402.COCT_MT090108CA.HealthCareProvider() { NullFlavor = NullFlavor.NoInformation }
					)
				)
			);

			string hcn = Guid.NewGuid().ToString("N");
			hcn = Regex.Replace(hcn, "[^.0-9]", "");
			hcn = hcn.Substring(0, 9);

			registerPatientRequest.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id = SET<II>.CreateSET(
					new II("1.3.6.1.4.1.33349.3.1.2.99121.9992", hcn),
					new II("1.2.840.114350.1.13.99998.8734", Guid.NewGuid().ToString("N")),
					new II("1.3.6.1.4.1.33349.3.1.2.99121.283", Guid.NewGuid().ToString())
			);

			registerPatientRequest.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.EffectiveTime = new IVL<TS>(DateTime.Now);

			LogGraphable(registerPatientRequest);

			return registerPatientRequest;
		}

		/// <summary>
		/// Logs an IGraphable message.
		/// </summary>
		/// <param name="graphable">The IGraphable message to log.</param>
		public static void LogGraphable(IGraphable graphable)
		{
			System.Xml.XmlWriter writer = null;

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

			Debug.WriteLine(sb.ToString());
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

			//foreach (var item in graphables)
			//{
			//	formatter.Graph(Console.OpenStandardOutput(), item);
			//}

			client.Open();

			var sendResult = client.Send(graphable);

			//var temp = sendResult.Code == ResultCode.Accepted ||
			//	sendResult.Code == ResultCode.AcceptedNonConformant &&
			//	sendResult.Details.Count(o => o.Type == ResultDetailType.Error) == 0;

			if (sendResult.Code != ResultCode.Accepted && sendResult.Code != ResultCode.AcceptedNonConformant)
			{
				retVal = false;
			}

			var recvResult = client.Receive(sendResult);

			if (recvResult.Code != ResultCode.Accepted && recvResult.Code != ResultCode.AcceptedNonConformant)
			{
				retVal = false;
			}

			var result = recvResult.Structure;

			if (result == null)
			{
				retVal = false;
			}

			client.Close();

			return retVal;
		}

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

		private static LIST<PN> BuildNames(DemographicOptions patient)
		{
			LIST<PN> personNames = new LIST<PN>();

			foreach (var item in patient.Names)
			{
				personNames.Add(new PN(EntityNameUse.Legal, new ENXP[]
				{
					//new ENXP(item.Prefix, EntityNamePartType.Prefix),
					new ENXP(item.FirstName, EntityNamePartType.Given),
					new ENXP(item.LastName, EntityNamePartType.Family)
				}));
			}

			return personNames;
		}

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