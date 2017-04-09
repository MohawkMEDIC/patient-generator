/*
 * Copyright 2017-2017 Mohawk College of Applied Arts and Technology
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
 * Date: 2017-4-9
 */

using System.Xml.Serialization;

namespace PatientGenerator.FHIR.Configuration
{
	/// <summary>
	/// Represents authorization configuration.
	/// </summary>
	[XmlType("authorization")]
	public class AuthorizationConfiguration
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorizationConfiguration"/> class.
		/// </summary>
		public AuthorizationConfiguration()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorizationConfiguration"/> class.
		/// </summary>
		/// <param name="endpoint">The authorization endpoint.</param>
		/// <param name="applicationId">The application identifier.</param>
		/// <param name="applicationSecret">The application secret.</param>
		public AuthorizationConfiguration(string endpoint, string applicationId, string applicationSecret)
		{
			this.Endpoint = endpoint;
			this.ApplicationId = applicationId;
			this.ApplicationSecret = applicationSecret;
		}

		/// <summary>
		/// Gets or sets the application identifier.
		/// </summary>
		/// <value>The application identifier.</value>
		[XmlAttribute("id")]
		public string ApplicationId { get; set; }

		/// <summary>
		/// Gets or sets the application secret.
		/// </summary>
		/// <value>The application secret.</value>
		[XmlAttribute("secret")]
		public string ApplicationSecret { get; set; }

		/// <summary>
		/// Gets or sets the authorization endpoint.
		/// </summary>
		/// <value>The authorization endpoint.</value>
		[XmlAttribute("endpoint")]
		public string Endpoint { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		[XmlAttribute("password")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the scope.
		/// </summary>
		/// <value>The scope.</value>
		[XmlAttribute("scope")]
		public string Scope { get; set; }

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		[XmlAttribute("username")]
		public string Username { get; set; }
	}
}