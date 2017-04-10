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

using System;

namespace PatientGenerator.Messaging.Model
{
	/// <summary>
	/// Represents a progress response.
	/// </summary>
	public class ProgressResponse
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProgressResponse"/> class.
		/// </summary>
		public ProgressResponse()
		{
		}

		/// <summary>
		/// Gets or sets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count { get; set; }

		/// <summary>
		/// Gets or sets the creation timestamp.
		/// </summary>
		/// <value>The creation timestamp.</value>
		public DateTime CreationTimestamp { get; set; }

		/// <summary>
		/// Gets or sets the total.
		/// </summary>
		/// <value>The total.</value>
		public int Total { get; set; }
	}
}