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

namespace Scenarioo.Model.Docu.Entities.Generic
{
    /// <summary>
    /// Description of an application specific object to store in the documentation.
    /// </summary>
    [Serializable]
    [XmlRoot("objectDescription")]
    public class ObjectDescription
    {
        public string Name { get; set; }

        public string Type { get; set; }

        private Details details = new Details();

        public Details Details
        {
            get
            {
                return this.details;
            }
            set
            {
                this.details = value;
            }
        }

        public ObjectDescription()
        {

        }

        public ObjectDescription(string type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        public void AddDetail(string key, object value)
        {
            this.Details.AddDetail(key, value);
        }
    }
}
