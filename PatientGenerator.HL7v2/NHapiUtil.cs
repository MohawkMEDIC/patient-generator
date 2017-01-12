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
using NHapi.Model.V25.Datatype;
using NHapi.Model.V25.Message;
using NHapi.Model.V25.Segment;
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

        private static IMessage CreateBaseMessage(PatientGenerator.Core.Common.Metadata metadata)
        {
            ADT_A01 message = new ADT_A01();

            message.MSH.AcceptAcknowledgmentType.Value = "AL";
            message.MSH.DateTimeOfMessage.Time.Value = DateTime.Now.ToString("yyyyMMddHHmmss");
            message.MSH.MessageControlID.Value = Guid.NewGuid().ToString();
            message.MSH.MessageType.MessageStructure.Value = "ADT_A01";
            message.MSH.MessageType.MessageCode.Value = "ADT";
            message.MSH.MessageType.TriggerEvent.Value = "A01";
            message.MSH.ProcessingID.ProcessingID.Value = "P";

            message.MSH.ReceivingApplication.NamespaceID.Value = metadata.ReceivingApplication;
            message.MSH.ReceivingFacility.NamespaceID.Value = metadata.ReceivingFacility;
            message.MSH.SendingApplication.NamespaceID.Value = metadata.SendingApplication;
            message.MSH.SendingFacility.NamespaceID.Value = metadata.SendingFacility;

            message.MSH.VersionID.VersionID.Value = "2.3.1";

            return message;
        }

        public static IMessage GenerateCandidateRegistry(DemographicOptions options)
        {
            ADT_A01 message = CreateBaseMessage(options.Metadata) as ADT_A01;

            PID pid = message.PID;

            var cx = pid.GetPatientIdentifierList(0);

            cx.IDNumber.Value = options.PersonIdentifier;
            cx.AssigningAuthority.UniversalID.Value = options.Metadata.AssigningAuthority;
            cx.AssigningAuthority.UniversalIDType.Value = "ISO";
        
            pid.AdministrativeSex.Value = options.Gender;

            if (options.DateOfBirthOptions != null)
            {
                if (options.DateOfBirthOptions.Exact.HasValue)
                {
                    pid.DateTimeOfBirth.Time.SetShortDate(options.DateOfBirthOptions.Exact.Value);
                }
            }

            for (int i = 0; i < options.OtherIdentifiers.Count; i++)
            {
                pid.GetAlternatePatientIDPID(i).IDNumber.Value = options.OtherIdentifiers[i].Value;
                pid.GetAlternatePatientIDPID(i).AssigningAuthority.UniversalID.Value = options.OtherIdentifiers[i].AssigningAuthority;
                pid.GetAlternatePatientIDPID(i).AssigningAuthority.UniversalIDType.Value = options.OtherIdentifiers[i].Type;
            }

            for (int i = 0; i < options.Names.Count; i++)
            {
                pid.GetPatientName(i).GivenName.Value = options.Names.ToArray()[i].FirstName;
                pid.GetPatientName(i).FamilyName.Surname.Value = options.Names.ToArray()[i].LastName;
                pid.GetPatientName(i).PrefixEgDR.Value = options.Names.ToArray()[i].Prefix;
                pid.GetPatientName(i).ProfessionalSuffix.Value = options.Names.ToArray()[i].Suffixes.FirstOrDefault();

                var middleNames = options.Names.Select(x => x.MiddleNames).ToArray()[i];

                if (middleNames.Count > 0)
                {
                    pid.GetPatientName(i).SecondAndFurtherGivenNamesOrInitialsThereof.Value = middleNames.Aggregate((a, b) => a + " " + b);
                }
            }

            for (int i = 0; i < options.Addresses.Count; i++)
            {
                pid.GetPatientAddress(i).StreetAddress.StreetOrMailingAddress.Value = options.Addresses.ToArray()[i].StreetAddress;
                pid.GetPatientAddress(i).City.Value = options.Addresses.ToArray()[i].City;
                pid.GetPatientAddress(i).StateOrProvince.Value = options.Addresses.ToArray()[i].StateProvince;
                pid.GetPatientAddress(i).ZipOrPostalCode.Value = options.Addresses.ToArray()[i].ZipPostalCode;
                pid.GetPatientAddress(i).Country.Value = options.Addresses.ToArray()[i].Country;
            }

            for (int i = 0; i < options.TelecomOptions.PhoneNumbers.Count; i++)
            {
                pid.GetPhoneNumberHome(i).TelephoneNumber.Value = options.TelecomOptions.PhoneNumbers[i];
            }

            for (int i = 0; i < options.RelatedPersons.Count; i++)
            {
                var nk1 = message.AddNK1();

                nk1.Relationship.AlternateIdentifier.Value = options.RelatedPersons[i].Relationship;
                nk1.GetPhoneNumber(i).TelephoneNumber.Value = options.RelatedPersons[i].Phone;

                for (int j = 0; j < options.RelatedPersons[i].Names.Count; j++)
                {
                    nk1.GetName(j).GivenName.Value = options.Names.ToArray()[j].FirstName;
                    nk1.GetName(j).FamilyName.Surname.Value = options.Names.ToArray()[j].LastName;
                    nk1.GetName(j).PrefixEgDR.Value = options.Names.ToArray()[j].Prefix;
                    nk1.GetName(j).ProfessionalSuffix.Value = options.Names.ToArray()[j].Suffixes.FirstOrDefault();
                }

                for (int k = 0; k < options.RelatedPersons[i].Address.Count; k++)
                {
                    nk1.GetAddress(k).StreetAddress.StreetOrMailingAddress.Value = options.Addresses.ToArray()[k].StreetAddress;
                    nk1.GetAddress(k).City.Value = options.Addresses.ToArray()[k].City;
                    nk1.GetAddress(k).StateOrProvince.Value = options.Addresses.ToArray()[k].StateProvince;
                    nk1.GetAddress(k).ZipOrPostalCode.Value = options.Addresses.ToArray()[k].ZipPostalCode;
                    nk1.GetAddress(k).Country.Value = options.Addresses.ToArray()[k].Country;
                }
            }

            return message;
        }

        public static IMessage GenerateCandidateRegistry(PatientGenerator.Core.Common.Patient patient, PatientGenerator.Core.Common.Metadata metadata)
        {
            ADT_A01 message = CreateBaseMessage(metadata) as ADT_A01;

            PID pid = message.PID;

            var cx = pid.GetPatientIdentifierList(0);

            cx.IDNumber.Value = patient.HealthCardNo;
            cx.AssigningAuthority.UniversalID.Value = metadata.AssigningAuthority;
            cx.AssigningAuthority.UniversalIDType.Value = "ISO";

            pid.AdministrativeSex.Value = patient.Gender;
            pid.DateTimeOfBirth.Time.SetShortDate(patient.DateOfBirth);

            pid.GetPatientName(0).GivenName.Value = patient.FirstName;
            pid.GetPatientName(0).FamilyName.Surname.Value = patient.LastName;

            pid.GetPatientAddress(0).StreetAddress.StreetOrMailingAddress.Value = patient.AddressLine;
            pid.GetPatientAddress(0).City.Value = patient.City;
            pid.GetPatientAddress(0).StateOrProvince.Value = patient.Province;
            pid.GetPatientAddress(0).ZipOrPostalCode.Value = patient.PostalCode;
            pid.GetPatientAddress(0).Country.Value = patient.Country;

            return message;
        }

        public static IEnumerable<IMessage> Sendv2Messages(IMessage message, List<LlpEndpoint> addresses)
        {
            List<IMessage> messages = new List<IMessage>();

            foreach (var endpoint in addresses)
            {
                MllpMessageSender sender = new MllpMessageSender(new Uri(endpoint.Address));

                PipeParser parser = new PipeParser();

                var parsedMessage = parser.Encode(message);

#if DEBUG
                Trace.TraceInformation("Request: " + Environment.NewLine);
                Trace.TraceInformation(parsedMessage.ToString());
#endif

                Trace.TraceInformation("Sending to endpoint: " + endpoint.ToString());

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