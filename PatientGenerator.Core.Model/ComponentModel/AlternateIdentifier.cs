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
 * Date: 2017-01-11
 */

namespace PatientGenerator.Core.Model.ComponentModel
{
	/// <summary>
	/// Represents assigning authority options.
	/// </summary>
	public class AlternateIdentifier
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AlternateIdentifier"/> class.
		/// </summary>
		public AlternateIdentifier()
		{
			this.Type = "ISO";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AlternateIdentifier"/> class.
		/// </summary>
		/// <param name="assigningAuthority">The assigning authority. (OID)</param>
		/// <param name="value">The value of the assigning authority.</param>
		public AlternateIdentifier(string assigningAuthority, string value) : this()
		{
			this.AssigningAuthority = assigningAuthority;
			this.Value = value;
		}

		/// <summary>
		/// Gets or sets the assigning authority.
		/// </summary>
		public string AssigningAuthority { get; set; }

		/// <summary>
		/// Gets or sets the type of the assigning authority. Defaults to "ISO".
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets the value of the assigning authority.
		/// </summary>
		public string Value { get; set; }
	}
}