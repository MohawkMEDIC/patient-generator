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
 * Date: 2016-2-15
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHapi.Model.V231.Message;
using NHapi.Model.V231.Segment;
using PatientGenerator.Core.ComponentModel;
using PatientGenerator.HL7v2;
using System;
using System.Collections.Generic;

namespace PatientGenerator.Tests
{
	[TestClass]
	public class HL7v2Tests
	{
		private DemographicOptions options;

		[TestCleanup]
		public void Cleanup()
		{
			options = null;
		}

		[TestInitialize]
		public void Initialize()
		{
			options = new DemographicOptions
			{
				AssigningAuthority = "1.3.6.1.4.1.33349.3.1.2.99121.283",
				Addresses = new List<AddressOptions>
				{
					new AddressOptions
					{
						City = "Brampton",
						Country = "Canada",
						StreetAddress = "123 Main Street West",
						StateProvince = "Ontario",
						ZipPostalCode = "L6X0C3"
					},
					new AddressOptions
					{
						City = "New York City",
						Country = "United States of America",
						StreetAddress = "250 Madison Ave.",
						StateProvince = "New York",
						ZipPostalCode = "07008"
					},
					new AddressOptions
					{
						City = "Friedberg",
						Country = "Germany",
						StateProvince = "Elbonia",
						StreetAddress = "Grüner Weg 6",
						ZipPostalCode = "578233"
					}
				},
				DateOfBirthOptions = new DateOfBirthOptions
				{
					Exact = new DateTime(new Random().Next(1900, 2014), new Random().Next(1, 12), new Random().Next(1, 28))
				},
				Gender = "F",
				Names = new List<NameOptions>
				{
					new NameOptions
					{
						FirstName = "Samantha",
						LastName = "Richtofen"
					}
				},
				OtherIdentifiers = new Dictionary<string, string>
				{
					{
						"1.3.6.1.4.1.33349.3.1.2.2016.27.02.0." + new Random(DateTime.Now.Millisecond).Next(100, 10000), Guid.NewGuid().ToString("N")
					},
					{
						"1.3.6.1.4.1.33349.3.1.2.2016.27.02.1." + new Random(DateTime.Now.Second).Next(100, 10000), Guid.NewGuid().ToString("N")
					},
					{
						"1.3.6.1.4.1.33349.3.1.2.2016.27.02.2." + new Random(DateTime.Now.Minute).Next(100, 10000), Guid.NewGuid().ToString("N")
					},
					{
						"1.3.6.1.4.1.33349.3.1.2.2016.27.02.3." + new Random(DateTime.Now.Hour).Next(100, 10000), Guid.NewGuid().ToString("N")
					}
				},
				PersonIdentifier = Guid.NewGuid().ToString("N"),
				ReceivingApplication = "CRTEST",
				ReceivingFacility = "Mohawk College of Applied Arts and Technology",
				SendingApplication = "SEEDER",
				SendingFacility = "SEEDING",
				UseHL7v2 = true
			};
		}

		[TestMethod]
		public void SendMessageTest()
		{
			var response = NHapiUtil.GenerateCandidateRegistry(options);

			var messages = NHapiUtil.Sendv2Messages(response);

			foreach (var message in messages)
			{
				Assert.IsInstanceOfType(message, typeof(ACK));

				ACK ack = (ACK)message;

				Assert.IsInstanceOfType(ack.Message.GetStructure("MSA"), typeof(MSA));

				MSA msa = (MSA)ack.Message.GetStructure("MSA");

				Assert.AreEqual(msa.AcknowledgementCode.Value, "AA");
			}
		}

		[TestMethod]
		public void SendMessageInvalidOidTest()
		{
			options.AssigningAuthority = "this is not a valid oid";

			var response = NHapiUtil.GenerateCandidateRegistry(options);

			var messages = NHapiUtil.Sendv2Messages(response);

			foreach (var message in messages)
			{
				Assert.IsInstanceOfType(message, typeof(ACK));

				ACK ack = (ACK)message;

				Assert.IsInstanceOfType(ack.Message.GetStructure("MSA"), typeof(MSA));

				MSA msa = (MSA)ack.Message.GetStructure("MSA");

				Assert.AreEqual(msa.AcknowledgementCode.Value, "AR");
			}
		}
	}
}