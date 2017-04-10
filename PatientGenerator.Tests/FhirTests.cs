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
 * Date: 2016-2-21
 */

using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientGenerator.Core.Model.ComponentModel;
using PatientGenerator.FHIR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PatientGenerator.Tests
{
	[TestClass]
	public class FhirTests
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
				Addresses = new List<Core.Model.ComponentModel.Address>
				{
					new Core.Model.ComponentModel.Address
					{
						City = "Brampton",
						Country = "Canada",
						StreetAddress = "123 Main Street West",
						StateProvince = "Ontario",
						ZipPostalCode = "L6X0C3"
					},
					new Core.Model.ComponentModel.Address
					{
						City = "New York City",
						Country = "United States of America",
						StreetAddress = "250 Madison Ave.",
						StateProvince = "New York",
						ZipPostalCode = "07008"
					},
					new Core.Model.ComponentModel.Address
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
					SendingFacility = "SEEDING"
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

			var actual = FhirUtility.GenerateCandidateRegistry(options);

			Assert.IsNull(actual.Address.Select(x => x.City).FirstOrDefault());
			Assert.IsNull(actual.Address.Select(x => x.Country).FirstOrDefault());
			Assert.IsNull(actual.Address.Select(x => x.Line).FirstOrDefault());
			Assert.IsNull(actual.Address.Select(x => x.State).FirstOrDefault());
			Assert.IsNull(actual.Address.Select(x => x.PostalCode).FirstOrDefault());
		}

		[TestMethod]
		public void TestEmptyAlternateIdentifiers()
		{
			options.OtherIdentifiers.Clear();

			var actual = FhirUtility.GenerateCandidateRegistry(options);

			Assert.AreEqual(0, actual.Identifier.Count);
		}

		[TestMethod]
		public void TestEmptyDateOfBirth()
		{
			options.DateOfBirthOptions = null;

			var actual = FhirUtility.GenerateCandidateRegistry(options);

			Assert.IsNull(actual.BirthDate);
		}

		[TestMethod]
		public void TestEmptyEmail()
		{
			var actual = FhirUtility.GenerateCandidateRegistry(options);

			Assert.AreEqual(0, actual.Telecom.Count);
		}

		[TestMethod]
		public void TestEmptyGender()
		{
			options.Gender = null;

			var actual = FhirUtility.GenerateCandidateRegistry(options);

			Assert.AreEqual(AdministrativeGender.Unknown, actual.Gender);
		}

		[TestMethod]
		public void TestEmptyName()
		{
			options.Names.Clear();

			var actual = FhirUtility.GenerateCandidateRegistry(options);

			Assert.AreEqual(0, actual.Name.Count);
		}

		[TestMethod]
		public void TestEmptyPhone()
		{
			var actual = FhirUtility.GenerateCandidateRegistry(options);

			Assert.AreEqual(0, actual.Telecom.Count);
		}
	}
}