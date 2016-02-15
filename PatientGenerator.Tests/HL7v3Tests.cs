using Microsoft.VisualStudio.TestTools.UnitTesting;
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
		[TestInitialize]
		public void Initialize()
		{

		}

		[TestCleanup]
		public void Cleanup()
		{

		}

		[TestMethod]
		public void SendMessageTest()
		{
			var message = EverestUtil.GenerateCandidateRegistry(new Core.DemographicOptions
			{
				Addresses = new List<Core.AddressOptions>
				{
					new Core.AddressOptions
					{
						City = "Brampton",
						Country = "Canada",
						StreetAddress = "123 Main Street West",
						StateProvince = "Ontario",
						ZipPostalCode = "L6X0C3"
					},
					new Core.AddressOptions
					{
						City = "Amritsar",
						Country = "India",
						StreetAddress = "42 Mal Road",
						StateProvince = "Punjab",
						ZipPostalCode = "143502"
					},
					new Core.AddressOptions
					{
						City = "New York City",
						Country = "United States of America",
						StreetAddress = "250 Madison Ave.",
						StateProvince = "New York",
						ZipPostalCode = "07008"
					},
					new Core.AddressOptions
					{
						City = "Friedberg",
						Country = "Germany",
						StreetAddress = "Grüner Weg 6",
						ZipPostalCode = "61169"
					},
					new Core.AddressOptions
					{
						City = "Salinas",
						Country = "United States of America",
						StreetAddress = "30 Mortensen Avenue",
						StateProvince = "California",
						ZipPostalCode = "93905"
					}
				},
				Names = new List<Core.NameOptions>
				{
					new Core.NameOptions
					{
						FirstName = "Barry",
						LastName = "Sanders"
					},
					new Core.NameOptions
					{
						FirstName = "Paul",
						LastName = "Brown"
					}
				}
			});

			EverestUtil.Sendv3Messages(message, "cr");
		}
	}
}
