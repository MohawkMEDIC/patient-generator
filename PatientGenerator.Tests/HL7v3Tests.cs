/*
 * Copyright 2016-2017 Mohawk College of Applied Arts and Technology
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

using MARC.Everest.RMIM.CA.R020402.Interactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientGenerator.Core.Model.ComponentModel;
using PatientGenerator.HL7v3;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PatientGenerator.Tests
{
	/// <summary>
	/// Contains tests for HL7v3 message generation.
	/// </summary>
	[TestClass]
	public class HL7v3Tests
	{
		/// <summary>
		/// The demographic options.
		/// </summary>
		private DemographicOptions options;

		/// <summary>
		/// Runs cleanup after each test execution.
		/// </summary>
		[TestCleanup]
		public void Cleanup()
		{
			options = null;
		}

		/// <summary>
		/// Runs initialization before each test execution.
		/// </summary>
		[TestInitialize]
		public void Initialize()
		{
			options = new DemographicOptions
			{
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
				Gender = "M",
				Metadata = new Core.Model.Metadata
				{
					AssigningAuthority = "1.3.6.1.4.1.33349.3.1.2.99121.283",
					ReceivingApplication = "CRTEST",
					ReceivingFacility = "Mohawk College of Applied Arts and Technology",
					SendingApplication = "SEEDER",
					SendingFacility = "SEEDING"
				},
				Names = new List<NameOptions>
				{
					new NameOptions
					{
						FirstName = "David",
						LastName = "Nutley",
						MiddleNames = new List<string>
						{
							"Kimberly",
							"Rob",
							"Nutley"
						},
						Prefix = "Ms.",
						Suffixes = new List<string>
						{
							"PhD",
							"MSc"
						}
					}
				},
				OtherIdentifiers = new List<AlternateIdentifierOptions>
				{
					new AlternateIdentifierOptions("1.3.6.1.4.1.33349.3.1.3.12", Guid.NewGuid().ToString("N")),
					new AlternateIdentifierOptions("1.2.840.114350.1.13.99998.8734", Guid.NewGuid().ToString("N")),
					new AlternateIdentifierOptions("1.3.6.1.4.1.33349.3.1.2.99121.9992", new Random(DateTime.Now.Second).Next(100, 10000).ToString()),
					new AlternateIdentifierOptions("1.3.6.1.4.1.33349.3.1.3.201203.1.0", new Random(DateTime.Now.Millisecond).Next(100, 10000).ToString())
				},
				PersonIdentifier = Guid.NewGuid().ToString("N")
			};
		}

		/// <summary>
		/// Tests that the address is empty when no address options are provided.
		/// </summary>
		[TestMethod]
		public void TestEmptyAddress()
		{
			options.Addresses.Clear();

			options.Names.Clear();

			options.Names.Add(new NameOptions
			{
				FirstName = "Harry",
				LastName = "Homeless",
			});

			var actual = EverestUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual, typeof(PRPA_IN101201CA));

			PRPA_IN101201CA message = (PRPA_IN101201CA)actual;

			Assert.AreEqual("Mohawk College of Applied Arts and Technology", message.Receiver.Device.Id.Extension);
			Assert.AreEqual("SEEDING", message.Sender.Device.Id.Extension);
			Assert.AreEqual("Harry", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Given).Value);
			Assert.AreEqual("Homeless", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Family).Value);
		}

		/// <summary>
		/// Tests that the alternate identifiers are empty when no alternate identifiers are provided.
		/// </summary>
		[TestMethod]
		public void TestEmptyAlternateIdentifiers()
		{
			options.OtherIdentifiers.Clear();
			options.Names.Clear();

			options.Names.Add(new NameOptions
			{
				FirstName = "Walter",
				LastName = "Gretzky",
			});

			var actual = EverestUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual, typeof(PRPA_IN101201CA));

			PRPA_IN101201CA message = (PRPA_IN101201CA)actual;

			Assert.AreEqual("Mohawk College of Applied Arts and Technology", message.Receiver.Device.Id.Extension);
			Assert.AreEqual("SEEDING", message.Sender.Device.Id.Extension);
			Assert.AreEqual("Walter", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Given).Value);
			Assert.AreEqual("Gretzky", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Family).Value);
			Assert.IsNotNull(message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.BirthTime.Value);
			Assert.AreEqual(1, message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id.Count);
		}

		/// <summary>
		/// Tests that the date of birth is empty when no date of birth is provided.
		/// </summary>
		[TestMethod]
		public void TestEmptyDateOfBirth()
		{
			options.DateOfBirthOptions = null;
			options.Names.Clear();

			options.Names.Add(new NameOptions
			{
				FirstName = "Strawberry",
				LastName = "Shortcake",
			});

			var actual = EverestUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual, typeof(PRPA_IN101201CA));

			PRPA_IN101201CA message = (PRPA_IN101201CA)actual;

			Assert.AreEqual("Mohawk College of Applied Arts and Technology", message.Receiver.Device.Id.Extension);
			Assert.AreEqual("SEEDING", message.Sender.Device.Id.Extension);
			Assert.AreEqual("Strawberry", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Given).Value);
			Assert.AreEqual("Shortcake", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Family).Value);
			Assert.IsNull(message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.BirthTime.Value);
		}

		/// <summary>
		/// Tests that the name is empty when no name is provided.
		/// </summary>
		[TestMethod]
		public void TestEmptyName()
		{
			options.Names.Clear();

			var actual = EverestUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual, typeof(PRPA_IN101201CA));

			PRPA_IN101201CA message = (PRPA_IN101201CA)actual;

			Assert.AreEqual("Mohawk College of Applied Arts and Technology", message.Receiver.Device.Id.Extension);
			Assert.AreEqual("SEEDING", message.Sender.Device.Id.Extension);
			Assert.AreEqual(0, message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Count);
			Assert.IsNotNull(message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.BirthTime.Value);
		}

		/// <summary>
		/// Tests that the gender is female when female is provided.
		/// </summary>
		[TestMethod]
		public void TestGenderFemale()
		{
			options.Gender = "F";

			var actual = EverestUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual, typeof(PRPA_IN101201CA));

			PRPA_IN101201CA message = (PRPA_IN101201CA)actual;

			Assert.AreEqual("Mohawk College of Applied Arts and Technology", message.Receiver.Device.Id.Extension);
			Assert.AreEqual("SEEDING", message.Sender.Device.Id.Extension);
			Assert.IsNotNull(message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.BirthTime.Value);

			Assert.AreEqual("female", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.AdministrativeGenderCode.DisplayName.Value);
		}

		/// <summary>
		/// Tests that the gender is male when male is provided.
		/// </summary>
		[TestMethod]
		public void TestGenderMale()
		{
			options.Gender = "M";

			var actual = EverestUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual, typeof(PRPA_IN101201CA));

			PRPA_IN101201CA message = (PRPA_IN101201CA)actual;

			Assert.AreEqual("Mohawk College of Applied Arts and Technology", message.Receiver.Device.Id.Extension);
			Assert.AreEqual("SEEDING", message.Sender.Device.Id.Extension);
			Assert.IsNotNull(message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.BirthTime.Value);

			Assert.AreEqual("male", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.AdministrativeGenderCode.DisplayName.Value);
		}

		/// <summary>
		/// Tests that the gender is undifferentiated when an undifferentiated gender is provided.
		/// </summary>
		[TestMethod]
		public void TestGenderUndifferentiated()
		{
			options.Gender = "UN";

			var actual = EverestUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual, typeof(PRPA_IN101201CA));

			PRPA_IN101201CA message = (PRPA_IN101201CA)actual;

			Assert.AreEqual("Mohawk College of Applied Arts and Technology", message.Receiver.Device.Id.Extension);
			Assert.AreEqual("SEEDING", message.Sender.Device.Id.Extension);
			Assert.IsNotNull(message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.BirthTime.Value);

			Assert.AreEqual("undifferentiated", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.AdministrativeGenderCode.DisplayName.Value);
		}

		/// <summary>
		/// Tests that multiple addresses are recorded when multiple addresses are provided.
		/// </summary>
		[TestMethod]
		public void TestMultipleAddresses()
		{
			options.Addresses.Clear();

			options.Addresses.Add(new AddressOptions
			{
				City = "Chicago",
				Country = "United States of America",
				StateProvince = "Illinois"
			});

			options.Addresses.Add(new AddressOptions
			{
				City = "Brampton",
				Country = "Canada",
				StreetAddress = "123 Main Street West",
				StateProvince = "Ontario"
			});

			options.Addresses.Add(new AddressOptions
			{
				City = "New York City",
				Country = "United States of America",
				StateProvince = "New York"
			});

			options.Addresses.Add(new AddressOptions
			{
				Country = "Germany",
				StreetAddress = "Grüner Weg 6",
				ZipPostalCode = "578233"
			});

			options.Names.Clear();

			options.Names.Add(new NameOptions
			{
				FirstName = "Joel",
				LastName = "Quinnville",
			});

			var actual = EverestUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual, typeof(PRPA_IN101201CA));

			PRPA_IN101201CA message = (PRPA_IN101201CA)actual;

			Assert.AreEqual("Mohawk College of Applied Arts and Technology", message.Receiver.Device.Id.Extension);
			Assert.AreEqual("SEEDING", message.Sender.Device.Id.Extension);
			Assert.AreEqual("Joel", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Given).Value);
			Assert.AreEqual("Quinnville", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Family).Value);
			Assert.IsNotNull(message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.BirthTime.Value);
			Assert.AreEqual(4, message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id.Count);

			var firstAddress = message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Addr.Select(x => x.Part).ElementAt(0);

			Assert.AreEqual("Chicago", firstAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.City).Value);
			Assert.AreEqual("United States of America", firstAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.Country).Value);
			Assert.AreEqual("Illinois", firstAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.State).Value);
			Assert.IsNull(firstAddress.FirstOrDefault(x => x.Type == MARC.Everest.DataTypes.AddressPartType.StreetAddressLine));
			Assert.IsNull(firstAddress.FirstOrDefault(x => x.Type == MARC.Everest.DataTypes.AddressPartType.PostalCode));

			var secondAddress = message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Addr.Select(x => x.Part).ElementAt(1);

			Assert.AreEqual("Brampton", secondAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.City).Value);
			Assert.AreEqual("Canada", secondAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.Country).Value);
			Assert.AreEqual("Ontario", secondAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.State).Value);
			Assert.AreEqual("123 Main Street West", secondAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.StreetAddressLine).Value);
			Assert.IsNull(secondAddress.FirstOrDefault(x => x.Type == MARC.Everest.DataTypes.AddressPartType.PostalCode));

			var thirdAddress = message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Addr.Select(x => x.Part).ElementAt(2);

			Assert.AreEqual("New York City", thirdAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.City).Value);
			Assert.AreEqual("United States of America", thirdAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.Country).Value);
			Assert.AreEqual("New York", thirdAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.State).Value);
			Assert.IsNull(thirdAddress.FirstOrDefault(x => x.Type == MARC.Everest.DataTypes.AddressPartType.StreetAddressLine));
			Assert.IsNull(thirdAddress.FirstOrDefault(x => x.Type == MARC.Everest.DataTypes.AddressPartType.PostalCode));

			var fourthAddress = message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Addr.Select(x => x.Part).ElementAt(3);

			Assert.IsNull(fourthAddress.FirstOrDefault(x => x.Type == MARC.Everest.DataTypes.AddressPartType.City));
			Assert.AreEqual("Germany", fourthAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.Country).Value);
			Assert.IsNull(fourthAddress.FirstOrDefault(x => x.Type == MARC.Everest.DataTypes.AddressPartType.State));
			Assert.AreEqual("Grüner Weg 6", fourthAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.StreetAddressLine).Value);
			Assert.AreEqual("578233", fourthAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.PostalCode).Value);
		}

		/// <summary>
		/// Tests that multiple alternate identifiers are recorded when multiple alternate identifiers are provided.
		/// </summary>
		[TestMethod]
		public void TestMultipleAlternateIdentifiers()
		{
			options.Names.Clear();

			options.Names.Add(new NameOptions
			{
				FirstName = "Walter",
				LastName = "Gretzky",
			});

			options.OtherIdentifiers.Add(new AlternateIdentifierOptions("1.3.6.1.4.1.33349.3.1.5.100.0", "8385-171-683-CX"));
			options.OtherIdentifiers.Add(new AlternateIdentifierOptions("1.3.6.1.4.1.33349.3.1.5.101.1", "1192-571-546-CX"));
			options.OtherIdentifiers.Add(new AlternateIdentifierOptions("1.3.6.1.4.1.33349.3.1.5.102.2", "2115-060-045-CX"));

			var actual = EverestUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual, typeof(PRPA_IN101201CA));

			PRPA_IN101201CA message = (PRPA_IN101201CA)actual;

			Assert.AreEqual("Mohawk College of Applied Arts and Technology", message.Receiver.Device.Id.Extension);
			Assert.AreEqual("SEEDING", message.Sender.Device.Id.Extension);
			Assert.AreEqual("Walter", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Given).Value);
			Assert.AreEqual("Gretzky", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Family).Value);
			Assert.IsNotNull(message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.BirthTime.Value);
			Assert.AreEqual(7, message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id.Count);

			Assert.AreEqual("8385-171-683-CX", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id.Where(x => x.Root == "1.3.6.1.4.1.33349.3.1.5.100.0").Select(x => x.Extension).First());
			Assert.AreEqual("1192-571-546-CX", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id.Where(x => x.Root == "1.3.6.1.4.1.33349.3.1.5.101.1").Select(x => x.Extension).First());
			Assert.AreEqual("2115-060-045-CX", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id.Where(x => x.Root == "1.3.6.1.4.1.33349.3.1.5.102.2").Select(x => x.Extension).First());
		}

		/// <summary>
		/// Tests that a partial addresses is recorded when a partial address is provided.
		/// </summary>
		[TestMethod]
		public void TestPartialAddress()
		{
			options.Addresses.Clear();

			options.Addresses.Add(new AddressOptions
			{
				City = "Chicago",
				Country = "United States of America",
				StateProvince = "Illinois"
			});

			options.Names.Clear();

			options.Names.Add(new NameOptions
			{
				FirstName = "Joel",
				LastName = "Quinnville",
			});

			var actual = EverestUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual, typeof(PRPA_IN101201CA));

			PRPA_IN101201CA message = (PRPA_IN101201CA)actual;

			Assert.AreEqual("Mohawk College of Applied Arts and Technology", message.Receiver.Device.Id.Extension);
			Assert.AreEqual("SEEDING", message.Sender.Device.Id.Extension);
			Assert.AreEqual("Joel", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Given).Value);
			Assert.AreEqual("Quinnville", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Family).Value);
			Assert.IsNotNull(message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.BirthTime.Value);
			Assert.AreEqual(4, message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id.Count);
			Assert.AreEqual("Chicago", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Addr.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.City).Value);
			Assert.AreEqual("United States of America", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Addr.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.Country).Value);
			Assert.AreEqual("Illinois", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Addr.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.State).Value);
		}

		/// <summary>
		/// Tests the when two addresses are returned when two addresses are provided.
		/// </summary>
		[TestMethod]
		public void TestTwoAddresses()
		{
			options.Addresses.Clear();

			options.Addresses.Add(new AddressOptions
			{
				City = "Chicago",
				Country = "United States of America",
				StateProvince = "Illinois"
			});

			options.Addresses.Add(new AddressOptions
			{
				City = "Brampton",
				Country = "Canada",
				StreetAddress = "123 Main Street West",
				StateProvince = "Ontario"
			});

			options.Names.Clear();

			options.Names.Add(new NameOptions
			{
				FirstName = "Joel",
				LastName = "Quinnville",
			});

			var actual = EverestUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual, typeof(PRPA_IN101201CA));

			PRPA_IN101201CA message = (PRPA_IN101201CA)actual;

			Assert.AreEqual("Mohawk College of Applied Arts and Technology", message.Receiver.Device.Id.Extension);
			Assert.AreEqual("SEEDING", message.Sender.Device.Id.Extension);
			Assert.AreEqual("Joel", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Given).Value);
			Assert.AreEqual("Quinnville", message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Name.Select(x => x.Part).First().First(x => x.Type == MARC.Everest.DataTypes.EntityNamePartType.Family).Value);
			Assert.IsNotNull(message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.BirthTime.Value);
			Assert.AreEqual(4, message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.Id.Count);

			var firstAddress = message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Addr.Select(x => x.Part).ElementAt(0);

			Assert.AreEqual("Chicago", firstAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.City).Value);
			Assert.AreEqual("United States of America", firstAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.Country).Value);
			Assert.AreEqual("Illinois", firstAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.State).Value);
			Assert.IsNull(firstAddress.FirstOrDefault(x => x.Type == MARC.Everest.DataTypes.AddressPartType.StreetAddressLine));
			Assert.IsNull(firstAddress.FirstOrDefault(x => x.Type == MARC.Everest.DataTypes.AddressPartType.PostalCode));

			var secondAddress = message.controlActEvent.Subject.RegistrationRequest.Subject.registeredRole.IdentifiedPerson.Addr.Select(x => x.Part).ElementAt(1);

			Assert.AreEqual("Brampton", secondAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.City).Value);
			Assert.AreEqual("Canada", secondAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.Country).Value);
			Assert.AreEqual("Ontario", secondAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.State).Value);
			Assert.AreEqual("123 Main Street West", secondAddress.First(x => x.Type == MARC.Everest.DataTypes.AddressPartType.StreetAddressLine).Value);
			Assert.IsNull(secondAddress.FirstOrDefault(x => x.Type == MARC.Everest.DataTypes.AddressPartType.PostalCode));
		}
	}
}