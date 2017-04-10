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
using System.IO;
using System.Xml.Serialization;

namespace PatientGenerator.Randomizer
{
	/// <summary>
	/// Represents a base randomizer service.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class RandomizerBase<T> where T : class
	{
		/// <summary>
		/// Loads the data.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <returns>Returns the loaded data.</returns>
		/// <exception cref="System.ArgumentNullException">filename</exception>
		protected virtual T LoadData(string filename)
		{
			if (filename == null)
			{
				throw new ArgumentNullException($"{nameof(filename)} cannot be null");
			}

			var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetDirectoryName(filename), Path.GetFileName(filename));

			using (var fileStream = File.OpenRead(file))
			{
				var xsz = new XmlSerializer(typeof(T));

				return xsz.Deserialize(fileStream) as T;
			}
		}
	}
}