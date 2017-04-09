using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Core.ComponentModel
{
    public class RelatedPersonOptions
    {
        /// <summary>
        /// The name of the related person to the patient.
        /// </summary>
        public List<NameOptions> Names { get; set; }

        /// <summary>
        /// The relationship of this person to the patient. Stored as a relationship code.
        /// https://www.hl7.org/fhir/v2/0063/index.html
        /// </summary>
        public PatientRelationshipType Relationship { get; set; }

        /// <summary>
        /// The phone number of the related person to the patient.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The address of the related person to the patient.
        /// </summary>
        public List<AddressOptions> Address { get; set; }

        /// <summary>
        /// Definition for the available relationships to add to the patient.
        /// </summary>
        public enum PatientRelationshipType
        {
            MTH,
            FTH
        }
    }
}
