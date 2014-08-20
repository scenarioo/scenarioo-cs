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

namespace Scenarioo.Model.Docu.Entities
{
    using System.Xml.Serialization;

    using Scenarioo.Model.Docu.Entities.Generic;

    [Serializable]
    [XmlRoot("page")]
    public class Page
    {
        private Labels labels;

        public Page()
        {
            this.Name = string.Empty;
        }

        public Page(string name)
        {
            this.Name = name;
        }

        // TODO: may have some impact on serialization?? Check if any annotaion is needed

        /// <summary>
        /// Gets or sets multiple labels to a scenario object.
        /// </summary>
        /// <returns>All labels of this object. Never null.</returns>
        [XmlElement("Labels")]
        public Labels Labels
        {
            get
            {
                return this.labels ?? (this.labels = new Labels());
            }

            set
            {
                this.labels = value;
            }
        }

        /// <summary>
        /// Gets or sets unique name of the page. For a webpage you usually use the relative application internal url-path to that page or
        /// the relative-file-path of the template file rendering this page. Try to use names that are as short as possible
        /// and do not include any url parameters. Names should be as stable as possible, do not use names that might change
        /// on each new build.
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets additional application specific details with additional metadata information's.
        /// See <see cref="Details"/>
        /// </summary>
        [XmlElement("details")]
        public Details Details { get; set; }
    }
}
