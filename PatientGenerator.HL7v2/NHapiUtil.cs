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
using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Model.V25.Message;
using NHapi.Model.V25.Segment;
using PatientGenerator.Core;
using PatientGenerator.Core.ComponentModel;
using PatientGenerator.HL7v2.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace PatientGenerator.HL7v2
{
	public static class NHapiUtil
	{
		private static HL7v2ConfigurationSection configuration = ConfigurationManager.GetSection("medic.patientgen.hl7v2") as HL7v2ConfigurationSection;

		public static IMessage GenerateCandidateRegistry(DemographicOptions patient)
		{
			ADT_A01 message = new ADT_A01();

			message.MSH.AcceptAcknowledgmentType.Value = "AL";
			message.MSH.DateTimeOfMessage.Time.Value = DateTime.Now.ToString("yyyyMMddHHmmss");
			message.MSH.MessageControlID.Value = Guid.NewGuid().ToString();
			message.MSH.MessageType.MessageStructure.Value = "ADT_A01";
			message.MSH.MessageType.MessageCode.Value = "ADT";
			message.MSH.MessageType.TriggerEvent.Value = "A01";
			message.MSH.ProcessingID.ProcessingID.Value = "P";

			message.MSH.ReceivingApplication.NamespaceID.Value = patient.ReceivingApplication;
			message.MSH.ReceivingFacility.NamespaceID.Value = patient.ReceivingFacility;
			message.MSH.SendingApplication.NamespaceID.Value = patient.SendingApplication;
			message.MSH.SendingFacility.NamespaceID.Value = patient.SendingFacility;

			message.MSH.VersionID.VersionID.Value = "2.3.1";

			PID pid = message.PID;

			var cx = pid.GetPatientIdentifierList(0);

			cx.IDNumber.Value = patient.PersonIdentifier;
			cx.AssigningAuthority.UniversalID.Value = patient.AssigningAuthority;
			cx.AssigningAuthority.UniversalIDType.Value = "ISO";

			pid.AdministrativeSex.Value = "M";
			pid.DateTimeOfBirth.Time.SetShortDate(patient.DateOfBirthOptions.Exact.Value);

			pid.GetPatientName(0).GivenName.Value = patient.Names.Select(x => x.FirstName).FirstOrDefault();
			pid.GetPatientName(0).FamilyName.Surname.Value = patient.Names.Select(x => x.LastName).FirstOrDefault();

			for (int i = 0; i < patient.Addresses.Count; i++)
			{
				pid.GetPatientAddress(i).StreetAddress.StreetOrMailingAddress.Value = patient.Addresses.ToArray()[i].StreetAddress;
				pid.GetPatientAddress(i).City.Value = patient.Addresses.ToArray()[i].City;
				pid.GetPatientAddress(i).StateOrProvince.Value = patient.Addresses.ToArray()[i].StateProvince;
				pid.GetPatientAddress(i).ZipOrPostalCode.Value = patient.Addresses.ToArray()[i].ZipPostalCode;
				pid.GetPatientAddress(i).Country.Value = patient.Addresses.ToArray()[i].Country;
			}

			return message;
		}

		public static IEnumerable<IMessage> Sendv2Messages(IMessage message)
		{
			List<IMessage> messages = new List<IMessage>();

			foreach (var endpoint in configuration.Endpoints)
			{
				MllpMessageSender sender = new MllpMessageSender(new Uri(endpoint.Address));

				PipeParser parser = new PipeParser();

				var parsedMessage = parser.Encode(message);

#if DEBUG
				Trace.TraceInformation("Request: " + Environment.NewLine);
				Trace.TraceInformation(parsedMessage.ToString());
#endif

				IMessage response = sender.SendAndReceive(message);

				var parsedResponse = parser.Encode(response);

#if DEBUG
				Trace.TraceInformation("Response: " + Environment.NewLine);
				Trace.TraceInformation(parsedResponse.ToString());
#endif
				messages.Add(response);
			}

			return messages;
		}
	}
}