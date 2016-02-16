using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientGenerator.Core.ComponentModel;
using PatientGenerator.HL7v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Tests
{
	[TestClass]
	public class HL7v3Tests
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
			var message = EverestUtil.GenerateCandidateRegistry(new DemographicOptions
			{
				Addresses = new List<AddressOptions>
				{
					//new Core.AddressOptions
					//{
					//	City = "Brampton",
					//	Country = "Canada",
					//	StreetAddress = "123 Main Street West",
					//	StateProvince = "Ontario",
					//	ZipPostalCode = "L6X0C3"
					//},
					new AddressOptions
					{
						City = "New York City",
						Country = "United States of America",
						StreetAddress = "250 Madison Ave.",
						StateProvince = "New York",
						ZipPostalCode = "07008"
					},
					//new Core.AddressOptions
					//{
					//	City = "Friedberg",
					//	Country = "Germany",
					//	StreetAddress = "Grüner Weg 6",
					//	StateProvince = "Tempa",
					//	ZipPostalCode = "61169"
					//},
					new AddressOptions
					{
						City = "Salinas",
						Country = "United States of America",
						StreetAddress = "30 Mortensen Avenue",
						StateProvince = "California",
						ZipPostalCode = "93905"
					}
				},
				DateOfBirthOptions = new DateOfBirthOptions
				{
					Exact = new DateTime(new Random().Next(1900, 2014), new Random().Next(1, 12), new Random().Next(1, 28))
				},
				Gender = "M",
				Names = new List<NameOptions>
				{
					new NameOptions
					{
						FirstName = "Larry",
						LastName = "McDavid"
					}
				},
				PersonIdentifier = Guid.NewGuid().ToString("N")
			});

			Assert.IsTrue(EverestUtil.Sendv3Messages(message, "cr"));
		}
	}
}
