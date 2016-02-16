using PatientGenerator.Core.ComponentModel;
using PatientGenerator.Messaging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Messaging.Services
{
	[ServiceContract(Namespace = "http://marc-hi.ca/xmlns/patgensvc", ConfigurationName = "GenerationService.Generator")]
	public interface IGenerationService
	{
		[OperationContract]
		GenerationResponse GeneratePatients(DemographicOptions options);
	}
}
