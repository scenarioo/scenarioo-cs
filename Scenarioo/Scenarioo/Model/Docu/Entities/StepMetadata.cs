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
using System.Xml.Serialization;

using Scenarioo.Model.Docu.Entities.Generic;

namespace Scenarioo.Model.Docu.Entities
{
    /// <summary>
    /// Metadata for a step. This is a container for all additional detail data about a step that is only displayed on
    /// _details page for a step.
    /// </summary>
    [Serializable]
    [XmlRoot("metadata")]
    public class StepMetadata
    {
        private Details _details = new Details();

        /// <summary>
        ///  Gets or sets additional application specific _details with additional metadata information's.
        /// </summary>
        [XmlElement("_details")]
        public Details Details
        {
            get { return _details; }
            set { _details = value; }
        }

        /// <summary>
        /// Gets or sets all visible text of a step here to provide possibility to search inside visible step text.
        /// But currently the client web application does not yet support full text search anyway and also not to display
        /// this visible text anywhere in the web application.
        /// </summary>
        [XmlElement("visibleText")]
        public string VisibleText { get; set; }

        /// <summary>
        /// Add application specific _details as key-value-data-items.
        /// </summary>
        public void AddDetail(string key, object value)
        {
            Details.AddDetail(key, value);
        }
    }
}
