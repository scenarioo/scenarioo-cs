﻿/* scenarioo-api
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

using Scenarioo.Api.Util.Xml;
using Scenarioo.Model.Docu.Entities.Generic;

namespace Scenarioo.Model.Docu.Entities
{
    [Serializable]
    [XmlRoot("build")]
    public class Build
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns;

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("revision")]
        public string Revision { get; set; }

        [XmlElement("date")]
        public DateTime Date { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("details")]
        public Details Details { get; set; }

        public Build()
        {
            Xmlns = new XmlSerializerNamespaces();
            Xmlns.Add("ns3", ScenarioDocuXMLFileUtil.ScenarioNameSpace);
            Xmlns.Add("xs", ScenarioDocuXMLFileUtil.XmlSchema);
        }

        public Build(string name)
            : this()
        {
            this.Name = name;
        }

        public void AddDetail(string key, object value)
        {
            if (this.Details == null)
            {
                this.Details = new Details();    
            }

            this.Details.AddDetail(key, value);
        }
    }
}
