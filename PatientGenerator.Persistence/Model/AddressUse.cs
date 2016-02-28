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
 * Date: 2016-2-27
 */

using System.ComponentModel;

namespace PatientGenerator.Persistence.Model
{
	public enum AddressUse
	{
		HomeAddress = 0,
		PrimaryHome = 1,
		VacationHome = 2,
		WorkPlace = 3,
		Direct = 4,
		Public = 5,
		BadAddress = 6,
		PhysicalVisit = 7,
		PostalAddress = 8,
		TemporaryAddress = 9,
		Alphabetic = 10,
		Ideographic = 11,
		Syllabic = 12,
		Soundex = 13,
		Phonetic = 14
	}
}