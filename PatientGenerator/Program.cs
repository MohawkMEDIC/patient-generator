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
 * Date: 2016-2-12
 */

using MARC.HI.EHRS.SVC.Core;
using MohawkCollege.Util.Console.Parameters;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace PatientGenerator
{
	[Guid("D50C83BB-37D9-4B55-B79A-FE016D6DB81F")]
	internal class Program
	{
		/// <summary>
		/// The trace source.
		/// </summary>
		private static TraceSource traceSource = new TraceSource("PatientGenerator");

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		internal static void Main(string[] args)
		{
			// Parser
			var parser = new ParameterParser<ConsoleParameters>();

			// Trace copyright information
			var entryAsm = Assembly.GetEntryAssembly();

			var hasConsole = true;

			traceSource.TraceEvent(TraceEventType.Information, 0, "Patient Generator Startup : v{0}", entryAsm.GetName().Version);
			traceSource.TraceEvent(TraceEventType.Information, 0, "Patient Generator Working Directory : {0}", entryAsm.Location);
			traceSource.TraceEvent(TraceEventType.Information, 0, "Operating System: {0} {1}", Environment.OSVersion.Platform, Environment.OSVersion.VersionString);
			traceSource.TraceEvent(TraceEventType.Information, 0, "CLI Version: {0}", Environment.Version);

			try
			{
				var parameters = parser.Parse(args);

				// What to do?
				if (parameters.ShowHelp)
				{
					parser.WriteHelp(Console.Out);
				}
				else if (parameters.ConsoleMode)
				{
					Console.WriteLine("Patient Generator {0}", entryAsm.GetName().Version);
					Console.WriteLine("{0}", entryAsm.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright);

					ServiceUtil.Start(typeof(Program).GUID);

					Console.WriteLine("Press [ENTER] to stop...");
					Console.ReadLine();

					ServiceUtil.Stop();
				}
				else
				{
					hasConsole = false;
					ServiceBase[] servicesToRun = new ServiceBase[] { new PatientGenerator() };
					ServiceBase.Run(servicesToRun);
				}
			}
			catch (Exception e)
			{
#if DEBUG
				Trace.TraceError(e.ToString());
				if (hasConsole)
					Console.WriteLine(e.ToString());
#else
                Trace.TraceError("Error encountered: {0}. Will terminate", e.Message);
#endif
			}
		}
	}
}