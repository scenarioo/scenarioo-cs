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

namespace Scenarioo.Model.Docu.Entities.Generic
{
    using System.Xml;
    using System.Xml.Serialization;

    using Scenarioo.Api.Serializer;
    using Scenarioo.Api.Util.Xml;

    [Serializable]
    [XmlRoot("details")]
    public class Details: IXmlSerializable
    {

        public SerializableDictionary<string, object> Properties { get; set; }

        public Details()
        {
            this.Properties = new SerializableDictionary<string, object>();
        }

        public void AddDetail(string key, object value)
        {
            if (value != null)
            {
                this.Properties.Add(key, value);
            }
            else
            {
                this.Properties.Remove(key);
            }
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {

            foreach (var key in this.Properties.Keys)
            {
                writer.WriteStartElement("entry");

                writer.WriteStartElement("key");
                writer.WriteName(key);
                writer.WriteEndElement();

                var value = this.Properties[key];

                writer.WriteStartElement("value");
                writer.WriteAttributeString("xmlns:xsi", ScenarioDocuXMLFileUtil.schemaInstanceNamespace);

                if (value.GetType() == typeof(Details))
                {
                    writer.WriteAttributeString("xsi:type", "details");
                }

                this.Properties.SerializeDetails(writer, value);

                writer.WriteEndElement();
                writer.WriteEndElement();
            }

        }
    }
}
