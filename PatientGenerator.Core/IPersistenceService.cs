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
 * Date: 2016-2-27
 */

using MARC.HI.EHRS.SVC.Core.Services;
using System.Threading.Tasks;
using PatientGenerator.Core.Model.ComponentModel;

namespace PatientGenerator.Core
{
	/// <summary>
	/// Represents a persistence service.
	/// </summary>
	public interface IPersistenceService
	{
		/// <summary>
		/// Saves the specified options.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		bool Save(DemographicOptions options);

		/// <summary>
		/// Saves the asynchronous.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		Task<bool> SaveAsync(DemographicOptions options);
	}
}