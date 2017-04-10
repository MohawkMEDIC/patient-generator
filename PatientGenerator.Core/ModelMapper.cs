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

using System;
using System.Reflection;

namespace PatientGenerator.Core
{
	/// <summary>
	/// Instructs the ModelMapper to ignore this property.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class MappingIgnore : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MappingIgnore"/> class.
		/// </summary>
		public MappingIgnore()
		{
		}
	}

	/// <summary>
	/// A helper class to map basic properties between two objects (models).
	/// </summary>
	public static class ModelMapper
	{
		/// <summary>
		///  Map common properties between two models.
		///  Properties are mapped using their property name. The same name must exist between both models to be mapped.
		/// </summary>
		/// <typeparam name="T">The to model type.</typeparam>
		/// <typeparam name="F">The from model type.</typeparam>
		/// <param name="fromModel">The from model the to model will copy its property values to.</param>
		/// <returns>An instantiated to model with properties mapped from the from model.</returns>
		public static T Map<T, F>(F fromModel)
			where T : class, new()
			where F : class
		{
			// Create a new instance of the to model and call the Map method.
			return new T().Map(fromModel);
		}

		/// <summary>
		/// Map properties from a model to this model.
		/// Properties are mapped using their property name. The same name must exist between both models to be mapped.
		/// </summary>
		/// <typeparam name="T">The type of model being mapped to.</typeparam>
		/// <typeparam name="F">The type of model being mapped from.</typeparam>
		/// <param name="toModel">The model being mapped to.</param>
		/// <param name="fromModel">The model being mapped from.</param>
		/// <returns>To model populated with common properties.</returns>
		public static T Map<T, F>(this T toModel, F fromModel)
			where T : class
			where F : class
		{
			Type toType = typeof(T);
			BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
			PropertyInfo[] properties = toType.GetProperties(flags);

			// Loop through all public and non-static (instance) properties in the to model.
			foreach (PropertyInfo property in properties)
			{
				// Check if the to model wants this property ignored.
				if (!Attribute.IsDefined(property, typeof(MappingIgnore)))
				{
					var fromProperty = fromModel.GetType().GetProperty(property.Name);

					// Check if the from model contains the property to map.
					if (fromProperty != null)
					{
						// Check if the from property wants this property ignored.
						if (!Attribute.IsDefined(fromProperty, typeof(MappingIgnore)))
						{
							// Not checking for the same return type, let's trust the developer for now.

							// Get the value of the identical property from the from model.
							var fromValue = fromProperty.GetValue(fromModel, null);

							// Set the value of the to model to the value retrieved from the from model.
							property.SetValue(toModel, fromValue);
						}
					}
				}
			}

			return toModel;
		}
	}
}