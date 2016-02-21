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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Tests
{
	[TestClass]
	public class HL7v2Tests
    {
		[TestCleanup]
		public void Cleanup()
		{

		}

		[TestInitialize]
		public void Initialize()
		{

		}

		[TestMethod]
		public void SendMessageTest()
		{
			var response = NHapiUtil.GenerateCandidateRegistry(new DemographicOptions
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
					},
					new AddressOptions
					{
						City = "Friedberg",
						Country = "Germany",
						StreetAddress = "Grüner Weg 6",
					}
				},
				DateOfBirthOptions = new DateOfBirthOptions
				{
					Exact = new DateTime(new Random().Next(1900, 2014), new Random().Next(1, 12), new Random().Next(1, 28))
				},
				Names = new List<NameOptions>
				{
					new NameOptions
					{
						FirstName = "Samantha",
						LastName = "Richtofen"
					}
				},
				PersonIdentifier = Guid.NewGuid().ToString("N"),
				ReceivingApplication = "CRTEST",
				ReceivingFacility = "Mohawk College of Applied Arts and Technology",
				SendingApplication = "SEEDER",
				SendingFacility = "SEEDING"
			});

			var messages = NHapiUtil.Sendv2Messages(response);

			foreach (var message in messages)
			{
				Assert.IsInstanceOfType(response, typeof(ACK));

				ACK ack = (ACK)response;

				Assert.AreEqual(((MSA)ack.Message.GetStructure("MSA")).AcknowledgementCode.Value, "AA");
			}
		}

		[TestMethod]
		public void SendMessageInvalidOidTest()
		{
			var response = NHapiUtil.GenerateCandidateRegistry(new DemographicOptions
			{
				AssigningAuthority = "this is not a valid assigning authority value",
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
					},
					new AddressOptions
					{
						City = "Friedberg",
						Country = "Germany",
						StreetAddress = "Grüner Weg 6",
					}
				},
				DateOfBirthOptions = new DateOfBirthOptions
				{
					Exact = new DateTime(new Random().Next(1900, 2014), new Random().Next(1, 12), new Random().Next(1, 28))
				},
				Names = new List<NameOptions>
				{
					new NameOptions
					{
						FirstName = "Lawrence",
						LastName = "Taylor"
					}
				},
				PersonIdentifier = Guid.NewGuid().ToString("N"),
				ReceivingApplication = "CRTEST",
				ReceivingFacility = "Mohawk College of Applied Arts and Technology",
				SendingApplication = "SEEDER",
				SendingFacility = "SEEDING"
			});

			Assert.IsInstanceOfType(response, typeof(ACK));

			ACK ack = (ACK)response;

			Assert.AreEqual(((MSA)ack.Message.GetStructure("MSA")).AcknowledgementCode.Value, "AR");
		}
    }
}
