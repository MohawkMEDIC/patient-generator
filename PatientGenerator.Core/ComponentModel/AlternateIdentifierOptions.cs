using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Core.ComponentModel
{
    /// <summary>
    /// Represents assigning authority options.
    /// </summary>
    public class AlternateIdentifierOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlternateIdentifierOptions"/> class.
        /// </summary>
        public AlternateIdentifierOptions()
        {
            this.Type = "ISO";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlternateIdentifierOptions"/> class.
        /// </summary>
        /// <param name="assigningAuthority">The assigning authority. (OID)</param>
        /// <param name="value">The value of the assigning authority.</param>
        public AlternateIdentifierOptions(string assigningAuthority, string value) : this()
        {
            this.AssigningAuthority = assigningAuthority;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the assigning authority.
        /// </summary>
        public string AssigningAuthority { get; set; }

        /// <summary>
        /// Gets or sets the value of the assigning authority.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the type of the assigning authority. Defaults to "ISO".
        /// </summary>
        public string Type { get; set; }

    }
}
