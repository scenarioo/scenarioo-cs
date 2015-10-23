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
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    using Scenarioo.Api.Serializer;
    using Scenarioo.Api.Util.Xml;

    using XmlAttribute = Scenarioo.Api.Util.Xml.XmlAttribute;
    using XmlElement = Scenarioo.Api.Util.Xml.XmlElement;

    /// <summary>
    /// Collection of application specific additional information to store and display in documentation.
    /// The client of the API can add any key-value-data to this detail information's.
    /// Following type of objects are possible values inside the details:
    ///  - String: for usual text information
    ///  - ObjectDescription: for describing an object (described through a type and a name and additional
    ///    key-value-details again)
    ///  - ObjectReference: for referencing an ObjectDescription that was already stored elsewhere, only
    ///    through its type and name without storing all the details information again.
    ///  - ObjectList: for a list of such value items (same types allowed as content types again)
    ///  - ObjectTreeNode: for complex tree structures (containing same content types as possible node objects
    ///    again
    /// </summary>
    [Serializable]
    [XmlRoot("details")]
    public class Details : IXmlSerializable
    {
        public Details()
        {
            this.Properties = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Properties { get; set; }

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

                // Configure serializer behavior for element and attributes
                var genericSerializer = new GenericSerializer();
                genericSerializer.AddAttributeObjectBinding(
                    new XmlAttribute(typeof(Details), "xmlns:xsi", ScenarioDocuXMLFileUtil.SchemaInstanceNamespace));

                // Prepare elment and attribute names
                genericSerializer.AddAttributeObjectBinding(new XmlAttribute(typeof(Details), "xsi:type", "details"));
                genericSerializer.AddElementObjectBinding(new XmlElement(typeof(Details), "value"), typeof(Details));
                genericSerializer.AddElementObjectBinding(
                    new XmlElement(typeof(ObjectDescription), "value"), typeof(ObjectDescription));
                genericSerializer.AddElementObjectBinding(
                    new XmlElement(typeof(ObjectReference), "value"), typeof(ObjectReference));
                genericSerializer.AddElementObjectBinding(
                    new XmlElement(typeof(ObjectList<>), "value"), typeof(ObjectList<>));

                // Prepare xml tags
                genericSerializer.AddXmlTag(
                    new XmlTag(
                        new XmlElement(typeof(ObjectTreeNode<object>), "value"),
                        new List<XmlAttribute>
                            {
                                new XmlAttribute(
                                    "xmlns:xsi", ScenarioDocuXMLFileUtil.SchemaInstanceNamespace),
                                new XmlAttribute("xsi:type", "objectTreeNode")
                            },
                        typeof(ObjectTreeNode<object>)),
                    typeof(ObjectTreeNode<object>));

                genericSerializer.SuppressProperties = false;
                genericSerializer.DetailElementName = "value";

                genericSerializer.SerializeDetails(writer, value);

                writer.WriteEndElement();
            }
        }
    }
}
