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

			Assert.IsInstanceOfType(response, typeof(ACK));

			ACK ack = (ACK)response;

			Assert.AreEqual(((MSA)ack.Message.GetStructure("MSA")).AcknowledgementCode.Value, "AA");
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
