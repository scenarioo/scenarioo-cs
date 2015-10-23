/* scenarioo-api
 * Copyright (C) 2014, scenarioo.org Development Team
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * As a special exception, the copyright holders of this library give you 
 * permission to link this library with independent modules, according 
 * to the GNU General Public License with "Classpath" exception as provided
 * in the LICENSE file that accompanied this code.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Scenarioo.Api.Util.Xml;
using Scenarioo.Model.Docu.Entities.Generic;

namespace Scenarioo.Model.Docu.Entities
{
    /// <summary>
    /// Contains all the data collected from a web test for one step of one scenario/web test (except for the step image, which
    /// has to be stored separately
    /// </summary>
    [Serializable]
    [XmlRoot("step")]
    public class Step
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns;

        public Step()
        {
            ScreenAnnotations = new List<ScreenAnnotation>();

            Xmlns = new XmlSerializerNamespaces();
            Xmlns.Add("ns3", ScenarioDocuXMLFileUtil.ScenarioNameSpace);
            Xmlns.Add("xs", ScenarioDocuXMLFileUtil.XmlSchema);
        }

        /// <summary>
        /// Gets or sets information about the page this step belongs to (usually there are several steps that show the same UI page).
        /// This information is optional in case you do not have a page concept in your application.
        /// </summary>
        [XmlElement("page")]
        public Page Page { get; set; }

        /// <summary>
        /// Gets or sets most important description information about this step. Only put the most important values and information's about
        /// a step into this object.
        /// </summary>
        [XmlElement("stepDescription")]
        public StepDescription StepDescription { get; set; }

        /// <summary>
        /// Gets or sets optional information for web applications about the html output of current step.
        /// </summary>
        [XmlElement("html")]
        public StepHtml StepHtml { get; set; }

        /// <summary>
        /// Gets or sets additional metadata that will only be displayed on the step details page. Especially put huge
        /// additional detail data about a step into this object.
        /// </summary>
        [XmlElement("metadata")]
        public StepMetadata StepMetadata { get; set; }

        [XmlArray("screenAnnotations")]
        [XmlArrayItem("screenAnnotation")]
        public List<ScreenAnnotation> ScreenAnnotations { get; set; }
    }
}
