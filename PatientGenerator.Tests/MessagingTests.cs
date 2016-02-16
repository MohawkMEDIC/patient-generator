using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientGenerator.Core.ComponentModel;
using PatientGenerator.Messaging.MessageReceiver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Tests
{
	[TestClass]
	public class MessagingTests
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
				PersonIdentifier = Guid.NewGuid().ToString("N"),
				ReceivingApplication = "CRTEST",
				ReceivingFacility = "Mohawk College of Applied Arts and Technology",
				SendingApplication = "SEEDER",
				SendingFacility = "SEEDING",
				UseHL7v2 = true
			};
		}

		[TestMethod]
		public void ValidMessageTestv2()
		{
			GenerationService service = new GenerationService();

			var result = service.GeneratePatients(options);

			Assert.IsFalse(result.HasErrors);
		}

		[TestMethod]
		public void ValidMessageTestv3()
		{
			GenerationService service = new GenerationService();

			options.ReceivingApplication = null;
			options.ReceivingFacility = null;
			options.SendingApplication = null;
			options.SendingFacility = null;
			options.UseHL7v2 = false;

			var result = service.GeneratePatients(options);

			Assert.IsFalse(result.HasErrors);
		}

		[TestMethod]
		public void MissingAssigningAuthorityTestv2()
		{
			GenerationService service = new GenerationService();

			options.AssigningAuthority = null;
			options.ReceivingApplication = null;
			options.ReceivingFacility = null;
			options.SendingApplication = null;
			options.SendingFacility = null;
			options.UseHL7v2 = true;

			var result = service.GeneratePatients(options);

			Assert.IsTrue(result.HasErrors);
		}

		[TestMethod]
		public void MissingAssigningAuthorityTestv3()
		{
			GenerationService service = new GenerationService();

			options.AssigningAuthority = null;
			options.ReceivingApplication = null;
			options.ReceivingFacility = null;
			options.SendingApplication = null;
			options.SendingFacility = null;
			options.UseHL7v2 = false;

			var result = service.GeneratePatients(options);

			Assert.IsTrue(result.HasErrors);
		}

		[TestMethod]
		public void MissingDobOptionsTest()
		{
			GenerationService service = new GenerationService();

			options.DateOfBirthOptions = null;

			var result = service.GeneratePatients(options);

			Assert.IsFalse(result.HasErrors);
		}
	}
}
