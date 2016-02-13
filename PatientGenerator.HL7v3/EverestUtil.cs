using MARC.Everest.Connectors.WCF;
using MARC.Everest.Formatters.XML.Datatypes.R1;
using MARC.Everest.Formatters.XML.ITS1;
using MARC.Everest.Interfaces;
using MARC.Everest.Xml;
using PatientGenerator.Core;
using System;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace PatientGenerator.HL7v3
{
	public class EverestUtil
	{
		public static IGraphable GenerateCandidateRegistry(DemographicOptions patient)
		{
			//PRPA_IN101201CA registerPatientRequest = new PRPA_IN101201CA(
			//	Guid.NewGuid(),
			//	patient.DateOfBirth,
			//	ResponseMode.Immediate,
			//	PRPA_IN101201CA.GetInteractionId(),
			//	PRPA_IN101201CA.GetProfileId(),
			//	ProcessingID.Production,
			//	AcknowledgementCondition.Always,
			//	new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Receiver(
			//		new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Device2(
			//			new II("1.3.6.1.4.1.33349.3.1.1.20.4", "MARC-W1-1")
			//		)
			//	),
			//	new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Sender(
			//		new MARC.Everest.RMIM.CA.R020402.MCCI_MT002200CA.Device1(
			//			new II("1.3.6.1.4.1.33349.3.1.3.201303.0.0.0", "A4H_OSCAR_B")
			//		)
			//	)
			//);

			//registerPatientRequest.controlActEvent = PRPA_IN101201CA.CreateControlActEvent(
			//	Guid.NewGuid(),
			//	PRPA_IN101201CA.GetTriggerEvent(),
			//	new MARC.Everest.RMIM.CA.R020402.MFMI_MT700711CA.Author(),
			//	new MARC.Everest.RMIM.CA.R020402.MFMI_MT700711CA.Subject2<MARC.Everest.RMIM.CA.R020402.PRPA_MT101001CA.IdentifiedEntity>(
			//		true,
			//		new MARC.Everest.RMIM.CA.R020402.MFMI_MT700711CA.RegistrationRequest<MARC.Everest.RMIM.CA.R020402.PRPA_MT101001CA.IdentifiedEntity>(
			//			new MARC.Everest.RMIM.CA.R020402.MFMI_MT700711CA.Subject4<MARC.Everest.RMIM.CA.R020402.PRPA_MT101001CA.IdentifiedEntity>(
			//				new MARC.Everest.RMIM.CA.R020402.PRPA_MT101001CA.IdentifiedEntity(
			//						null,
			//						RoleStatus.Active,
			//						null,
			//						x_VeryBasicConfidentialityKind.Normal,
			//					new MARC.Everest.RMIM.CA.R020402.PRPA_MT101002CA.Person(
			//						new LIST<PN>(new PN[] {
			//							new PN(EntityNameUse.Legal, new ENXP[] {
			//								new ENXP(patient.FirstName, EntityNamePartType.Given),
			//								new ENXP(patient.MiddleName, EntityNamePartType.Given),
			//								new ENXP(patient.LastName, EntityNamePartType.Family)
			//							})
			//						}),
			//						LIST<TEL>.CreateList(
			//							new TEL(patient.Email, MARC.Everest.DataTypes.Interfaces.TelecommunicationAddressUse.MobileContact),
			//							new TEL(patient.PhoneNo, MARC.Everest.DataTypes.Interfaces.TelecommunicationAddressUse.WorkPlace)
			//						),
			//						MARC.Everest.Connectors.Util.Convert<AdministrativeGender>(patient.Gender),
			//						new TS(patient.DateOfBirth, DatePrecision.Day),
			//						false,
			//						null,
			//						false,
			//						null,
			//						new LIST<AD>(
			//							new AD[] {
			//								new AD(
			//									new ADXP[] {
			//										new ADXP(patient.AddressLine, AddressPartType.StreetAddressLine),
			//										new ADXP(patient.City, AddressPartType.City),
			//										new ADXP(patient.PostalCode, AddressPartType.PostalCode),
			//										new ADXP(patient.Province, AddressPartType.State),
			//										new ADXP("Canada", AddressPartType.Country)
			//									}
			//								)
			//							}
			//						),
			//						null,
			//						null,
			//						new MARC.Everest.RMIM.CA.R020402.PRPA_MT101104CA.LanguageCommunication(new CV<String>(patient.Language, "2.16.840.1.113883.6.121"), true)
			//					)
			//				)
			//			),
			//			new MARC.Everest.RMIM.CA.R020402.REPC_MT230003CA.Custodian(
			//				new MARC.Everest.RMIM.CA.R020402.COCT_MT090310CA.AssignedDevice(
			//					new II("2.16.840.1.113883.3.239.18.61", Guid.NewGuid().ToString("N")),
			//					new MARC.Everest.RMIM.CA.R020402.COCT_MT090310CA.Repository("KNG"),
			//					new MARC.Everest.RMIM.CA.R020402.COCT_MT090310CA.RepositoryJurisdiction("KGHD")
			//				)
			//			)
			//		)
			//	)

			//);

			//registerPatientRequest.controlActEvent.EffectiveTime = new IVL<TS>(patient.DateOfBirth);

			//// Author
			//registerPatientRequest.controlActEvent.Author.Time = patient.DateOfBirth;
			//registerPatientRequest.controlActEvent.Author.SetAuthorPerson(
			//	new MARC.Everest.RMIM.CA.R020402.COCT_MT090102CA.AssignedEntity(
			//		new SET<II>(new II("2.16.840.1.113883.3.239.18.1", Guid.NewGuid().ToString("N"))),
			//		new MARC.Everest.RMIM.CA.R020402.COCT_MT090108CA.Person(
			//			new PN(
			//				EntityNameUse.Legal,
			//				new ENXP[] {
			//					new ENXP("Fyfe", EntityNamePartType.Family),
			//					new ENXP("Justin", EntityNamePartType.Given),
			//					new ENXP("Dr", EntityNamePartType.Prefix)
			//				}
			//			),
			//			new MARC.Everest.RMIM.CA.R020402.COCT_MT090108CA.HealthCareProvider() { NullFlavor = NullFlavor.NoInformation }
			//		)
			//	)
			//);

			//string hcn = Guid.NewGuid().ToString("N");
			//hcn = Regex.Replace(hcn, "[^.0-9]", "");

			//registerPatientRequest.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id = SET<II>.CreateSET(
			//		new II("2.16.840.1.113883.3.239.18.1", Guid.NewGuid().ToString("N")),
			//		new II("2.16.840.1.113883.3.239.19.14", hcn),
			//		new II("2.16.840.1.113883.4.59", Guid.NewGuid().ToString())
			//);

			//return registerPatientRequest;

			return null;
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

			WcfClientConnector client = new WcfClientConnector(String.Format("endpointName={0}", endpointName));

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

			if (sendResult.Code != MARC.Everest.Connectors.ResultCode.Accepted && sendResult.Code != MARC.Everest.Connectors.ResultCode.AcceptedNonConformant)
			{
				retVal = false;
			}

			var recvResult = client.Receive(sendResult);

			if (recvResult.Code != MARC.Everest.Connectors.ResultCode.Accepted && recvResult.Code != MARC.Everest.Connectors.ResultCode.AcceptedNonConformant)
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
	}
}