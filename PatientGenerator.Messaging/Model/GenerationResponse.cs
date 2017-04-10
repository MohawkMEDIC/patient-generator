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

using System.Collections.Generic;

namespace PatientGenerator.Messaging.Model
{
	/// <summary>
	/// Represents a generation response.
	/// </summary>
	public class GenerationResponse
	{
		/// <summary>
		/// Initializes a new instance of the GenerationResponse class.
		/// </summary>
		public GenerationResponse()
		{
			this.Messages = new List<string>();
		}

		/// <summary>
		/// When true, a problem occurred when generating patients.
		/// </summary>
		public bool HasErrors { get; set; }

		/// <summary>
		/// The messages, if any.
		/// </summary>
		public List<string> Messages { get; set; }
	}
}