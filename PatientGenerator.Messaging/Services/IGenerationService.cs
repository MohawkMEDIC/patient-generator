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
 * Date: 2016-2-15
 */

using PatientGenerator.Core.ComponentModel;
using PatientGenerator.Messaging.Model;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading.Tasks;

namespace PatientGenerator.Messaging.Services
{
	/// <summary>
	/// Represents a patient generation service interface.
	/// </summary>
	[ServiceContract(Namespace = "http://marc-hi.ca/xmlns/patgensvc")]
	public interface IGenerationService
	{
		/// <summary>
		/// Generates patients using the provided options.
		/// </summary>
		/// <param name="options">The options to use to generate patients.</param>
		/// <returns>Returns a GenerationResponse.</returns>
		[OperationContract(Name = "GeneratePatients", Action = "GeneratePatients")]
		GenerationResponse GeneratePatients(DemographicOptions options);

		/// <summary>
		/// Generates patients using the provided options.
		/// </summary>
		/// <param name="options">The options to use to generate patients.</param>
		/// <returns>Returns a GenerationResponse.</returns>
		[OperationContract(Name = "GeneratePatientsAsync", Action = "GeneratePatientsAsync")]
		Task <GenerationResponse> GeneratePatientsAsync(DemographicOptions options);

		/// <summary>
		/// Gets records that have been generated for a session.
		/// </summary>
		/// <returns>Returns a list of records that have been generated for a session.</returns>
		[OperationContract(Name = "GetRecords", Action = "GetRecords")]
		object GetRecords();

		/// <summary>
		/// Get the progress for a patient generation session.
		/// </summary>
		/// <returns>Returns the progress.</returns>
		[OperationContract(Name = "Progress", Action = "Progress")]
		ProgressResponse Progress();
	}
}