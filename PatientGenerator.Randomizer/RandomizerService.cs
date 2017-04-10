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
 * Date: 2016-3-12
 */

using PatientGenerator.Core;
using PatientGenerator.Core.Model.Common;
using PatientGenerator.Randomizer.Common;
using System;
using System.Configuration;
using System.Diagnostics;

namespace PatientGenerator.Randomizer
{
	/// <summary>
	/// Represents a randomizer service.
	/// </summary>
	/// <seealso cref="Randomizer.RandomizerBase{CommonData}" />
	/// <seealso cref="IRandomizerService" />
	public class RandomizerService : RandomizerBase<CommonData>, IRandomizerService
	{
		/// <summary>
		/// The common data instance.
		/// </summary>
		private readonly CommonData commonData;

		/// <summary>
		/// The random instance.
		/// </summary>
		private readonly Random random = new Random();

		/// <summary>
		/// The trace source.
		/// </summary>
		private readonly TraceSource traceSource = new TraceSource("PatientGenerator.Randomizer");

		/// <summary>
		/// Initializes a new instance of the <see cref="RandomizerService"/> class.
		/// </summary>
		public RandomizerService()
		{
			commonData = LoadData(ConfigurationManager.AppSettings["CommonFile"]);
		}

		/// <summary>
		/// Gets the random patient.
		/// </summary>
		/// <returns>Returns a random patient.</returns>
		public Patient GetRandomPatient()
		{
			var nameGenderPair = commonData.GivenNames[random.Next(commonData.GivenNames.Count)];

			var firstName = nameGenderPair.Name;
			var middleName = commonData.GivenNames[random.Next(commonData.GivenNames.Count)].Name;
			var lastName = commonData.FamilyNames[random.Next(commonData.FamilyNames.Count - random.Next(1, commonData.FamilyNames.Count))];

			string postal = null;

			for (var i = 0; i < 6; i++)
			{
				if (i % 2 == 0)
					postal += (char)((byte)'A' + random.Next(20));
				else
					postal += random.Next(10).ToString();
				if (i == 2)
					postal += '-';
			}

			var patient = new Patient
			{
				AddressLine = random.Next(400).ToString("##0") + " " + commonData.StreetNames[random.Next(commonData.StreetNames.Count)],
				City = commonData.Cities[random.Next(commonData.Cities.Count)],
				Country = "Canada",
				DateOfBirth = new DateTime(random.Next(1930, 2014), random.Next(1, 11), random.Next(1, 28)),
				Email = (firstName + "." + lastName + random.Next(1000) + "@example.com").ToLower(),
				FirstName = firstName,
				Gender = nameGenderPair.GenderCode,
				HealthCardNo = OHIPUtilities.OHIPUtil.GenerateHealthNumber(random),
				Language = "eng",
				LastName = lastName,
				MiddleName = middleName,
				PhoneNo = "905575" + random.Next(1000, 9999),
				PostalCode = postal,
				Province = "Ontario"
			};

			traceSource.TraceEvent(TraceEventType.Verbose, 0, patient.FirstName + " " + patient.LastName + " " + patient.DateOfBirth.ToString("yyyy-MM-dd") + " " + patient.HealthCardNo);

			return patient;
		}
	}
}