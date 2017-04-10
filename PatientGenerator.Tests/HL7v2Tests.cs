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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHapi.Model.V231.Segment;
using PatientGenerator.Core.Model.ComponentModel;
using PatientGenerator.HL7v2;
using System;
using System.Collections.Generic;

namespace PatientGenerator.Tests
{
	[TestClass]
	public class HL7v2Tests
	{
		private Demographic options;

		[TestCleanup]
		public void Cleanup()
		{
			options = null;
		}

		[TestInitialize]
		public void Initialize()
		{
			options = new Demographic
			{
				Addresses = new List<Address>
				{
					new Address
					{
						City = "Brampton",
						Country = "Canada",
						StreetAddress = "123 Main Street West",
						StateProvince = "Ontario",
						ZipPostalCode = "L6X0C3"
					},
					new Address
					{
						City = "New York City",
						Country = "United States of America",
						StreetAddress = "250 Madison Ave.",
						StateProvince = "New York",
						ZipPostalCode = "07008"
					},
					new Address
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
				Metadata = new Core.Model.Metadata
				{
					AssigningAuthority = "1.3.6.1.4.1.33349.3.1.2.99121.283",
					ReceivingApplication = "CRTEST",
					ReceivingFacility = "Mohawk College of Applied Arts and Technology",
					SendingApplication = "SEEDER",
					SendingFacility = "SEEDING",
					UseHL7v2 = true
				},
				Names = new List<Name>
				{
					new Name
					{
						FirstName = "Samantha",
						LastName = "Richtofen"
					}
				},
				OtherIdentifiers = new List<AlternateIdentifier>
				{
					new AlternateIdentifier("1.3.6.1.4.1.33349.3.1.2.2016.27.02.0." + new Random(DateTime.Now.Millisecond).Next(100, 10000), Guid.NewGuid().ToString("N")),
					new AlternateIdentifier("1.3.6.1.4.1.33349.3.1.2.2016.27.02.1." + new Random(DateTime.Now.Second).Next(100, 10000), Guid.NewGuid().ToString("N")),
					new AlternateIdentifier("1.3.6.1.4.1.33349.3.1.2.2016.27.02.2." + new Random(DateTime.Now.Minute).Next(100, 10000), Guid.NewGuid().ToString("N")),
					new AlternateIdentifier("1.3.6.1.4.1.33349.3.1.2.2016.27.02.3." + new Random(DateTime.Now.Hour).Next(100, 10000), Guid.NewGuid().ToString("N"))
				},
				PersonIdentifier = Guid.NewGuid().ToString("N")
			};
		}

		[TestMethod]
		public void TestEmptyAddress()
		{
			options.Addresses.Clear();

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);

			Assert.IsInstanceOfType(actual.GetStructure("PID"), typeof(PID));

			PID pid = (PID)actual.GetStructure("PID");

			Assert.AreEqual("1.3.6.1.4.1.33349.3.1.2.99121.283", pid.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value);

			var addresses = pid.GetPatientAddress();

			foreach (var item in addresses)
			{
				Assert.IsNull(item.City.Value);
				Assert.IsNull(item.Country.Value);
				Assert.IsNull(item.StateOrProvince.Value);
				Assert.IsNull(item.StreetAddress.Value);
				Assert.IsNull(item.ZipOrPostalCode.Value);
			}
		}

		[TestMethod]
		public void TestEmptyAssigningAuthority()
		{
			options.Metadata.AssigningAuthority = string.Empty;

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);

			Assert.IsInstanceOfType(actual.GetStructure("PID"), typeof(PID));

			PID pid = (PID)actual.GetStructure("PID");

			Assert.AreEqual(string.Empty, pid.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value);
		}

		[TestMethod]
		public void TestEmptyDateOfBirth()
		{
			options.DateOfBirthOptions = null;

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);

			Assert.IsInstanceOfType(actual.GetStructure("PID"), typeof(PID));

			PID pid = (PID)actual.GetStructure("PID");

			Assert.AreEqual("1.3.6.1.4.1.33349.3.1.2.99121.283", pid.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value);
			Assert.IsNull(pid.DateTimeOfBirth.TimeOfAnEvent.Value);
		}

		[TestMethod]
		public void TestEmptyName()
		{
			options.Names.Clear();

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);

			Assert.IsInstanceOfType(actual.GetStructure("PID"), typeof(PID));

			PID pid = (PID)actual.GetStructure("PID");

			Assert.AreEqual("1.3.6.1.4.1.33349.3.1.2.99121.283", pid.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value);

			var names = pid.GetPatientName();

			foreach (var item in names)
			{
				Assert.IsNull(item.GivenName.Value);
				Assert.IsNull(item.FamilyLastName.FamilyName.Value);
			}
		}

		[TestMethod]
		public void TestEmptyReceivingApplication()
		{
			options.Metadata.ReceivingApplication = string.Empty;

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual(string.Empty, msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);
		}

		[TestMethod]
		public void TestEmptyReceivingFacility()
		{
			options.Metadata.ReceivingFacility = string.Empty;

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual(string.Empty, msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);
		}

		[TestMethod]
		public void TestEmptySendingApplication()
		{
			options.Metadata.SendingApplication = string.Empty;

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual(string.Empty, msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);
		}

		[TestMethod]
		public void TestEmptySendingFacility()
		{
			options.Metadata.SendingFacility = string.Empty;

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual(string.Empty, msh.SendingFacility.NamespaceID.Value);
		}

		[TestMethod]
		public void TestFullName()
		{
			options.Names.Clear();

			options.Names.Add(new Name
			{
				Prefix = "Dr.",
				FirstName = "Sammy",
				MiddleNames = new List<string>
				{
					"J",
					"Hall"
				},
				LastName = "Richtofen",
				Suffixes = new List<string>
				{
					"MSc"
				}
			});

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);

			Assert.IsInstanceOfType(actual.GetStructure("PID"), typeof(PID));

			PID pid = (PID)actual.GetStructure("PID");

			Assert.AreEqual("1.3.6.1.4.1.33349.3.1.2.99121.283", pid.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value);

			var name = pid.GetPatientName(0);

			Assert.AreEqual("Dr.", name.PrefixEgDR.Value);
			Assert.AreEqual("Sammy", name.GivenName.Value);
			Assert.AreEqual("J Hall", name.MiddleInitialOrName.Value);
			Assert.AreEqual("Richtofen", name.FamilyLastName.FamilyName.Value);
			Assert.AreEqual("MSc", name.SuffixEgJRorIII.Value);
		}

		[TestMethod]
		public void TestInvalidOid()
		{
			options.Metadata.AssigningAuthority = "this is not a valid oid";

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);

			Assert.IsInstanceOfType(actual.GetStructure("PID"), typeof(PID));

			PID pid = (PID)actual.GetStructure("PID");

			Assert.AreEqual("this is not a valid oid", pid.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value);
		}

		[TestMethod]
		public void TestMultipleAddresses()
		{
			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);

			Assert.IsInstanceOfType(actual.GetStructure("PID"), typeof(PID));

			PID pid = (PID)actual.GetStructure("PID");

			Assert.AreEqual("1.3.6.1.4.1.33349.3.1.2.99121.283", pid.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value);

			var firstAddress = pid.GetPatientAddress(0);

			Assert.AreEqual("Brampton", firstAddress.City.Value);
			Assert.AreEqual("Canada", firstAddress.Country.Value);
			Assert.AreEqual("Ontario", firstAddress.StateOrProvince.Value);
			Assert.AreEqual("123 Main Street West", firstAddress.StreetAddress.Value);
			Assert.AreEqual("L6X0C3", firstAddress.ZipOrPostalCode.Value);

			var secondAddress = pid.GetPatientAddress(1);

			Assert.AreEqual("New York City", secondAddress.City.Value);
			Assert.AreEqual("United States of America", secondAddress.Country.Value);
			Assert.AreEqual("New York", secondAddress.StateOrProvince.Value);
			Assert.AreEqual("250 Madison Ave.", secondAddress.StreetAddress.Value);
			Assert.AreEqual("07008", secondAddress.ZipOrPostalCode.Value);

			var thirdAddress = pid.GetPatientAddress(2);

			Assert.AreEqual("Friedberg", thirdAddress.City.Value);
			Assert.AreEqual("Germany", thirdAddress.Country.Value);
			Assert.AreEqual("Elbonia", thirdAddress.StateOrProvince.Value);
			Assert.AreEqual("Grüner Weg 6", thirdAddress.StreetAddress.Value);
			Assert.AreEqual("578233", thirdAddress.ZipOrPostalCode.Value);
		}

		[TestMethod]
		public void TestMultipleNames()
		{
			options.Names.Add(new Name
			{
				FirstName = "Sammy",
				LastName = "Richtofen"
			});

			options.Names.Add(new Name
			{
				FirstName = "Sally",
				LastName = "Sam"
			});

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);

			Assert.IsInstanceOfType(actual.GetStructure("PID"), typeof(PID));

			PID pid = (PID)actual.GetStructure("PID");

			Assert.AreEqual("1.3.6.1.4.1.33349.3.1.2.99121.283", pid.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value);

			var firstName = pid.GetPatientName(0);

			Assert.AreEqual("Samantha", firstName.GivenName.Value);
			Assert.AreEqual("Richtofen", firstName.FamilyLastName.FamilyName.Value);

			var secondName = pid.GetPatientName(1);

			Assert.AreEqual("Sammy", secondName.GivenName.Value);
			Assert.AreEqual("Richtofen", secondName.FamilyLastName.FamilyName.Value);

			var thirdName = pid.GetPatientName(2);

			Assert.AreEqual("Sally", thirdName.GivenName.Value);
			Assert.AreEqual("Sam", thirdName.FamilyLastName.FamilyName.Value);
		}

		[TestMethod]
		public void TestNamePrefix()
		{
			options.Names.Add(new Name
			{
				Prefix = "Dr.",
				FirstName = "Sammy",
				LastName = "Richtofen"
			});

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			//Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);

			Assert.IsInstanceOfType(actual.GetStructure("PID"), typeof(PID));

			PID pid = (PID)actual.GetStructure("PID");

			Assert.AreEqual("1.3.6.1.4.1.33349.3.1.2.99121.283", pid.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value);

			var firstName = pid.GetPatientName(0);

			Assert.AreEqual("Samantha", firstName.GivenName.Value);
			Assert.AreEqual("Richtofen", firstName.FamilyLastName.FamilyName.Value);

			var secondName = pid.GetPatientName(1);

			Assert.AreEqual("Dr.", secondName.PrefixEgDR.Value);
			Assert.AreEqual("Sammy", secondName.GivenName.Value);
			Assert.AreEqual("Richtofen", secondName.FamilyLastName.FamilyName.Value);
		}

		[TestMethod]
		public void TestNameSuffix()
		{
			options.Names.Clear();

			options.Names.Add(new Name
			{
				FirstName = "Sammy",
				LastName = "Richtofen",
				Suffixes = new List<string>
				{
					"MSc"
				}
			});

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);

			Assert.IsInstanceOfType(actual.GetStructure("PID"), typeof(PID));

			PID pid = (PID)actual.GetStructure("PID");

			Assert.AreEqual("1.3.6.1.4.1.33349.3.1.2.99121.283", pid.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value);

			var name = pid.GetPatientName(0);

			Assert.AreEqual("Sammy", name.GivenName.Value);
			Assert.AreEqual("Richtofen", name.FamilyLastName.FamilyName.Value);
			Assert.AreEqual("MSc", name.SuffixEgJRorIII.Value);
		}

		[TestMethod]
		public void TestNullAssigningAuthority()
		{
			options.Metadata.AssigningAuthority = null;

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);

			Assert.IsInstanceOfType(actual.GetStructure("PID"), typeof(PID));

			PID pid = (PID)actual.GetStructure("PID");

			Assert.IsNull(pid.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value);
		}

		[TestMethod]
		public void TestNullReceivingApplication()
		{
			options.Metadata.ReceivingApplication = null;

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.IsNull(msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);
		}

		[TestMethod]
		public void TestNullReceivingFacility()
		{
			options.Metadata.ReceivingFacility = null;

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.IsNull(msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);
		}

		[TestMethod]
		public void TestNullSendingApplication()
		{
			options.Metadata.SendingApplication = null;

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.IsNull(msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);
		}

		[TestMethod]
		public void TestNullSendingFacility()
		{
			options.Metadata.SendingFacility = null;

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.IsNull(msh.SendingFacility.NamespaceID.Value);
		}

		[TestMethod]
		public void TestPartialAddress()
		{
			options.Addresses.Clear();

			options.Addresses.Add(new Address
			{
				City = "Brampton",
				Country = "Canada",
				StreetAddress = "123 Main Street West",
				StateProvince = "Ontario"
			});

			options.Addresses.Add(new Address
			{
				City = "New York City",
				Country = "United States of America",
				StateProvince = "New York",
				ZipPostalCode = "07008"
			});

			options.Addresses.Add(new Address
			{
				Country = "Germany",
				StateProvince = "Elbonia",
				StreetAddress = "Grüner Weg 6",
				ZipPostalCode = "578233"
			});

			var actual = NHapiUtility.GenerateCandidateRegistry(options);

			Assert.IsInstanceOfType(actual.GetStructure("MSH"), typeof(MSH));

			MSH msh = (MSH)actual.GetStructure("MSH");

			Assert.AreEqual("CRTEST", msh.ReceivingApplication.NamespaceID.Value);
			Assert.AreEqual("Mohawk College of Applied Arts and Technology", msh.ReceivingFacility.NamespaceID.Value);
			Assert.AreEqual("SEEDER", msh.SendingApplication.NamespaceID.Value);
			Assert.AreEqual("SEEDING", msh.SendingFacility.NamespaceID.Value);

			Assert.IsInstanceOfType(actual.GetStructure("PID"), typeof(PID));

			PID pid = (PID)actual.GetStructure("PID");

			Assert.AreEqual("1.3.6.1.4.1.33349.3.1.2.99121.283", pid.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value);

			var firstAddress = pid.GetPatientAddress(0);

			Assert.AreEqual("Brampton", firstAddress.City.Value);
			Assert.AreEqual("Canada", firstAddress.Country.Value);
			Assert.AreEqual("Ontario", firstAddress.StateOrProvince.Value);
			Assert.AreEqual("123 Main Street West", firstAddress.StreetAddress.Value);
			Assert.IsNull(firstAddress.ZipOrPostalCode.Value);

			var secondAddress = pid.GetPatientAddress(1);

			Assert.AreEqual("New York City", secondAddress.City.Value);
			Assert.AreEqual("United States of America", secondAddress.Country.Value);
			Assert.AreEqual("New York", secondAddress.StateOrProvince.Value);
			Assert.IsNull(secondAddress.StreetAddress.Value);
			Assert.AreEqual("07008", secondAddress.ZipOrPostalCode.Value);

			var thirdAddress = pid.GetPatientAddress(2);

			Assert.IsNull(thirdAddress.City.Value);
			Assert.AreEqual("Germany", thirdAddress.Country.Value);
			Assert.AreEqual("Elbonia", thirdAddress.StateOrProvince.Value);
			Assert.AreEqual("Grüner Weg 6", thirdAddress.StreetAddress.Value);
			Assert.AreEqual("578233", thirdAddress.ZipOrPostalCode.Value);
		}
	}
}