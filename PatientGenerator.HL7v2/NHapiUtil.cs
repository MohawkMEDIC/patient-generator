using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Model.V25.Message;
using NHapi.Model.V25.Segment;
using PatientGenerator.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PatientGenerator.HL7v2
{
	public class NHapiUtil
	{
		public NHapiUtil()
		{
		}

		//"1.3.6.1.4.1.33349.3.1.2.99121.283"

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

			message.MSH.ReceivingApplication.NamespaceID.Value = "CR"; // options.ReceivingApplication;
			message.MSH.ReceivingFacility.NamespaceID.Value = "AeHIN"; // options.ReceivingFacility;
			message.MSH.SendingApplication.NamespaceID.Value = "SEEDER"; // options.SendingApplication;
			message.MSH.SendingFacility.NamespaceID.Value = "SEEDING"; // options.SendingFacility;

			message.MSH.VersionID.VersionID.Value = "2.3.1";

			PID pid = message.PID;

			var cx = pid.GetPatientIdentifierList(0);

			cx.IDNumber.Value = Guid.NewGuid().ToString("N"); // options.PersonIdentifier;
			cx.AssigningAuthority.UniversalID.Value = "1.3.6.1.4.1.33349.3.1.2.99121.283"; // options.AssigningAuthority;
			cx.AssigningAuthority.UniversalIDType.Value = "ISO";

			pid.AdministrativeSex.Value = "M";
			pid.DateTimeOfBirth.Time.SetShortDate(new DateTime(new Random().Next(1900, 2014), new Random().Next(1, 12), new Random().Next(1, 28)));

			for (int i = 0; i < options.Names.Count; i++)
			{
				pid.GetPatientName(i).GivenName.Value = options.Names.Select(x => x.FirstName).FirstOrDefault();
				pid.GetPatientName(i).FamilyName.Surname.Value = options.Names.Select(x => x.LastName).FirstOrDefault();
			}

			for (int i = 0; i < options.Addresses.Count; i++)
			{
				pid.GetPatientAddress(i).StreetAddress.StreetOrMailingAddress.Value = options.Addresses.ToArray()[i].StreetAddress;
				pid.GetPatientAddress(i).City.Value = options.Addresses.ToArray()[i].City;
				pid.GetPatientAddress(i).StateOrProvince.Value = options.Addresses.ToArray()[i].StateProvince;
				pid.GetPatientAddress(i).ZipOrPostalCode.Value = options.Addresses.ToArray()[i].ZipPostalCode;
				pid.GetPatientAddress(i).Country.Value = options.Addresses.ToArray()[i].Country;
			}

			MllpMessageSender sender = new MllpMessageSender(new Uri("mllp://il.aehin.marc-hi.ca:2100"));

			PipeParser parser = new PipeParser();

			var parsedMessage = parser.Encode(message);

			IMessage response = sender.SendAndReceive(message);
		}
	}
}