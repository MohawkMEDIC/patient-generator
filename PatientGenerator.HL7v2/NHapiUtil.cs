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
using NHapi.Model.V231.Datatype;
using NHapi.Model.V231.Message;
using NHapi.Model.V231.Segment;
using PatientGenerator.Core.ComponentModel;
using PatientGenerator.HL7v2.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace PatientGenerator.HL7v2
{
	/// <summary>
	/// Represents a utility for generating HL7v2 messages using the nHAPI framework.
	/// </summary>
	public static class NHapiUtil
    {
		/// <summary>
		/// The configuration.
		/// </summary>
		private static HL7v2ConfigurationSection configuration = ConfigurationManager.GetSection("medic.patientgen.hl7v2") as HL7v2ConfigurationSection;

		/// <summary>
		/// The trace source.
		/// </summary>
		private static readonly TraceSource traceSource = new TraceSource("PatientGenerator.HL7v2");

		/// <summary>
		/// Creates the base message.
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <returns>IMessage.</returns>
		private static IMessage CreateBaseMessage(PatientGenerator.Core.Common.Metadata metadata)
        {
            var message = new ADT_A01();

            message.MSH.AcceptAcknowledgmentType.Value = "AL";
            message.MSH.DateTimeOfMessage.TimeOfAnEvent.Value = DateTime.Now.ToString("yyyyMMddHHmmss");
            message.MSH.MessageControlID.Value = Guid.NewGuid().ToString();
            message.MSH.MessageType.MessageStructure.Value = "ADT_A01";
            message.MSH.MessageType.MessageType.Value = "ADT";
            message.MSH.MessageType.TriggerEvent.Value = "A01";
            message.MSH.ProcessingID.ProcessingID.Value = "P";

            message.MSH.ReceivingApplication.NamespaceID.Value = metadata.ReceivingApplication;
            message.MSH.ReceivingFacility.NamespaceID.Value = metadata.ReceivingFacility;
            message.MSH.SendingApplication.NamespaceID.Value = metadata.SendingApplication;
            message.MSH.SendingFacility.NamespaceID.Value = metadata.SendingFacility;

            message.MSH.VersionID.VersionID.Value = "2.3.1";

            return message;
        }

		/// <summary>
		/// Generates the candidate registry.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns>IMessage.</returns>
		public static IMessage GenerateCandidateRegistry(DemographicOptions options)
        {
            var message = CreateBaseMessage(options.Metadata) as ADT_A01;

            var pid = message.PID;
            
            var cx = pid.GetPatientIdentifierList(0);

            cx.ID.Value = options.PersonIdentifier;
            cx.AssigningAuthority.UniversalID.Value = options.Metadata.AssigningAuthority;
            cx.AssigningAuthority.UniversalIDType.Value = "ISO";
        
            pid.Sex.Value = options.Gender;

	        if (options.DateOfBirthOptions?.Exact != null)
	        {
		        pid.DateTimeOfBirth.TimeOfAnEvent.SetShortDate(options.DateOfBirthOptions.Exact.Value);
	        }

	        for (var i = 0; i < options.OtherIdentifiers.Count; i++)
            {
                pid.GetAlternatePatientIDPID(i).ID.Value = options.OtherIdentifiers[i].Value;
                pid.GetAlternatePatientIDPID(i).AssigningAuthority.UniversalID.Value = options.OtherIdentifiers[i].AssigningAuthority;
                pid.GetAlternatePatientIDPID(i).AssigningAuthority.UniversalIDType.Value = options.OtherIdentifiers[i].Type;
            }

            for (var i = 0; i < options.Names.Count; i++)
            {
                pid.GetPatientName(i).GivenName.Value = options.Names.ToArray()[i].FirstName;
                pid.GetPatientName(i).FamilyLastName.FamilyName.Value = options.Names.ToArray()[i].LastName;
                pid.GetPatientName(i).PrefixEgDR.Value = options.Names.ToArray()[i].Prefix;
                pid.GetPatientName(i).SuffixEgJRorIII.Value = options.Names.ToArray()[i].Suffixes.FirstOrDefault();

                var middleNames = options.Names.Select(x => x.MiddleNames).ToArray()[i];

                if (middleNames.Count > 0)
                {
                    pid.GetPatientName(i).MiddleInitialOrName.Value = middleNames.Aggregate((a, b) => a + " " + b);
                }
            }

            for (var i = 0; i < options.Addresses.Count; i++)
            {
                pid.GetPatientAddress(i).StreetAddress.Value = options.Addresses.ToArray()[i].StreetAddress;
                pid.GetPatientAddress(i).City.Value = options.Addresses.ToArray()[i].City;
                pid.GetPatientAddress(i).StateOrProvince.Value = options.Addresses.ToArray()[i].StateProvince;
                pid.GetPatientAddress(i).ZipOrPostalCode.Value = options.Addresses.ToArray()[i].ZipPostalCode;
                pid.GetPatientAddress(i).Country.Value = options.Addresses.ToArray()[i].Country;
            }

            for (var i = 0; i < options.TelecomOptions.PhoneNumbers.Count; i++)
            {
                pid.GetPhoneNumberHome(i).AnyText.Value = options.TelecomOptions.PhoneNumbers[i];
            }
            
            return message;
        }

		/// <summary>
		/// Generates the candidate registry.
		/// </summary>
		/// <param name="patient">The patient.</param>
		/// <param name="metadata">The metadata.</param>
		/// <returns>IMessage.</returns>
		public static IMessage GenerateCandidateRegistry(PatientGenerator.Core.Common.Patient patient, PatientGenerator.Core.Common.Metadata metadata)
        {
            var message = CreateBaseMessage(metadata) as ADT_A01;

            var pid = message.PID;

            var cx = pid.GetPatientIdentifierList(0);

            cx.ID.Value = patient.HealthCardNo;
            cx.AssigningAuthority.UniversalID.Value = metadata.AssigningAuthority;
            cx.AssigningAuthority.UniversalIDType.Value = "ISO";

            pid.Sex.Value = patient.Gender;
            pid.DateTimeOfBirth.TimeOfAnEvent.SetShortDate(patient.DateOfBirth);

            pid.GetPatientName(0).GivenName.Value = patient.FirstName;
            pid.GetPatientName(0).FamilyLastName.FamilyName.Value = patient.LastName;

            pid.GetPatientAddress(0).StreetAddress.Value = patient.AddressLine;
            pid.GetPatientAddress(0).City.Value = patient.City;
            pid.GetPatientAddress(0).StateOrProvince.Value = patient.Province;
            pid.GetPatientAddress(0).ZipOrPostalCode.Value = patient.PostalCode;
            pid.GetPatientAddress(0).Country.Value = patient.Country;

            return message;
        }

		/// <summary>
		/// Sendv2s the messages.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="addresses">The addresses.</param>
		/// <returns>IEnumerable&lt;IMessage&gt;.</returns>
		public static IEnumerable<IMessage> Sendv2Messages(IMessage message, IEnumerable<LlpEndpoint> addresses)
        {
            var messages = new List<IMessage>();

            foreach (var endpoint in addresses)
            {
                var sender = new MllpMessageSender(new Uri(endpoint.Address));

                var parser = new PipeParser();

                var parsedMessage = parser.Encode(message);

				traceSource.TraceEvent(TraceEventType.Verbose, 0, "Request: " + Environment.NewLine);
	            traceSource.TraceEvent(TraceEventType.Verbose, 0, parsedMessage);

				traceSource.TraceEvent(TraceEventType.Verbose, 0, "Sending to endpoint: " + endpoint);

                var response = sender.SendAndReceive(message);

                var parsedResponse = parser.Encode(response);

				traceSource.TraceEvent(TraceEventType.Verbose, 0, "Response: " + Environment.NewLine);
				traceSource.TraceEvent(TraceEventType.Verbose, 0, parsedResponse);

                messages.Add(response);
            }

            return messages;
        }

		/// <summary>
		/// Sends HL7v2 messages using SSL.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="addresses">The addresses.</param>
		/// <param name="thumbprint">The thumbprint.</param>
		/// <returns>Returns a list of sent messages.</returns>
		public static IEnumerable<IMessage> Sendv2MessagesBySsl(IMessage message, IEnumerable<LlpEndpoint> addresses, string thumbprint)
        {
            var messages = new List<IMessage>();

            foreach (var endpoint in addresses)
            {
                var sender = new SslMessageSender(new Uri(endpoint.Address));

                var parser = new PipeParser();

                var parsedMessage = parser.Encode(message);

				traceSource.TraceEvent(TraceEventType.Verbose, 0, "Request: " + Environment.NewLine);
				traceSource.TraceEvent(TraceEventType.Verbose, 0, parsedMessage);

				traceSource.TraceEvent(TraceEventType.Verbose, 0, "Sending to endpoint: " + endpoint);
                
                var response = sender.SendAndReceive(message, thumbprint);

                var parsedResponse = parser.Encode(response);

				traceSource.TraceEvent(TraceEventType.Verbose, 0, "Response: " + Environment.NewLine);
				traceSource.TraceEvent(TraceEventType.Verbose, 0, parsedResponse);

                messages.Add(response);
            }

            return messages;
        }
    }
}