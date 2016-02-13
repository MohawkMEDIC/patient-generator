using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Model.V25.Message;
using NHapi.Model.V25.Segment;
using PatientGenerator.Core;
using System;

namespace PatientGenerator.HL7v2
{
	public class NHapiUtil
	{
		public NHapiUtil()
		{
		}

		public static void GenerateCandidateRegistry(DemographicOptions options)
		{
			ADT_A01 message = new ADT_A01();

			message.MSH.AcceptAcknowledgmentType.Value = "AL"; // Always send response
			message.MSH.DateTimeOfMessage.Time.Value = DateTime.Now.ToString("yyyyMMddHHmmss"); // Date/time of creation of message
			message.MSH.MessageControlID.Value = Guid.NewGuid().ToString(); // Unique id for message
			message.MSH.MessageType.MessageStructure.Value = "ADT_A01"; // Message structure type (Query By Parameter Type 21)
			message.MSH.MessageType.MessageCode.Value = "ADT"; // Message Structure Code (Query By Parameter)
			message.MSH.MessageType.TriggerEvent.Value = "A01"; // Trigger event (Event Query 22)
			message.MSH.ProcessingID.ProcessingID.Value = "P"; // Production
			message.MSH.ReceivingApplication.NamespaceID.Value = "CRTEST"; // Client Registry
			message.MSH.ReceivingFacility.NamespaceID.Value = "CR1"; // SAMPLE
			message.MSH.SendingApplication.NamespaceID.Value = "MOSCAR"; // What goes here?
			message.MSH.SendingFacility.NamespaceID.Value = "McMaster"; // You're at the college ... right?
			message.MSH.VersionID.VersionID.Value = "2.3.1";

			PID pid = message.PID;

			var cx = pid.GetPatientIdentifierList(0);

			cx.IDNumber.Value = Guid.NewGuid().ToString("N");
			cx.AssigningAuthority.UniversalID.Value = "1.3.6.1.4.1.33349.3.1.3.201402.1.0.0";
			cx.AssigningAuthority.UniversalIDType.Value = "ISO";

			pid.AdministrativeSex.Value = "M";
			pid.DateTimeOfBirth.Time.SetShortDate(new DateTime(new Random().Next(1900, 2014), new Random().Next(1, 12), new Random().Next(1, 28)));
			pid.GetPatientName(0).GivenName.Value = "James";
			pid.GetPatientName(0).FamilyName.Surname.Value = "Smith";
			pid.GetPatientAddress(0).StreetAddress.StreetOrMailingAddress.Value = "123 Main St West";
			pid.GetPatientAddress(0).City.Value = "Hamilton";
			pid.GetPatientAddress(0).StateOrProvince.Value = "Ontario";
			pid.GetPatientAddress(0).Country.Value = "Canada";

			MllpMessageSender sender = new MllpMessageSender(new Uri("mllp://crtest.marc-hi.ca:2100"));

			PipeParser parser = new PipeParser();

			var parsedMessage = parser.Encode(message);

			IMessage response = sender.SendAndReceive(message);
		}
	}
}