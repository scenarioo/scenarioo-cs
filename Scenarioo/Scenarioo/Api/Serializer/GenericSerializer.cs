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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using Scenarioo.Api.Util.Xml;
using Scenarioo.Model.Docu.Entities.Generic;
using Scenarioo.Model.Docu.Entities.Generic.Interfaces;

using XmlAttribute = Scenarioo.Api.Util.Xml.XmlAttribute;
using XmlElement = Scenarioo.Api.Util.Xml.XmlElement;

namespace Scenarioo.Api.Serializer
{
    /// <summary>
    /// Responsible for serializing generic collections and specific generic types
    /// to ensure conformity against Java. Scenario server shall be enabled to deserialize 
    /// the xml via JAXB. The approach to generate code from given schema does not provides "complexity" covering properly 
    /// cases like a generic is child of a generic. So this implementation takes place as a first step.
    /// </summary>
    public class GenericSerializer
    {
        private readonly List<XmlElement> _elementObjectBindings = new List<XmlElement>();
        private readonly List<XmlAttribute> _attributeObjectBindings = new List<XmlAttribute>();
        private readonly List<XmlTag> _xmlTagObjectBindings = new List<XmlTag>();

        public string DetailElementName { get; set; }
        public bool SuppressProperties { get; set; }
        public bool IsTreeNodeRootElement = true;

        public void AddElementObjectBinding(XmlElement element, object type)
        {
            _elementObjectBindings.RemoveAll(e => e.BindingObject == type);
            _elementObjectBindings.Add(element);
        }

        public void AddAttributeObjectBinding(XmlAttribute attribute)
        {
            _attributeObjectBindings.RemoveAll(e => ReferenceEquals(e.BindingObject.GetType(), attribute.BindingObject.GetType()));
            _attributeObjectBindings.Add(attribute);
        }

        public void AddXmlTag(XmlTag tag, object type)
        {
            _xmlTagObjectBindings.RemoveAll(t => t.BindingObject == type);
            _xmlTagObjectBindings.Add(tag);
        }

        private XmlTag ResolveXmlTag(object type)
        {
            var firstOrDefault = _xmlTagObjectBindings.FirstOrDefault(e => ReferenceEquals(e.BindingObject, type));
            if (firstOrDefault != null)
            {
                return firstOrDefault;
            }

            return null;
        }

        private string ResolveElementName(object type)
        {
            var firstOrDefault = _elementObjectBindings.FirstOrDefault(e => ReferenceEquals(e.BindingObject, type));
            if (firstOrDefault != null)
            {
                return firstOrDefault.Name;
            }

            return string.Empty;
        }

        /// <summary>
        /// Serializes all depending generic types and traverse recursively.
        /// </summary>
        /// <param name="writer">Stream output in which will be serialized</param>
        /// <param name="value">Any value which shall be serialized</param>
        public void SerializeDetails<T>(XmlWriter writer, T value)
        {
            // Primitive handles only a couple of datatypes byte, int, bool... 
            // Explanation: string ist NOT a primitive type and it don`t will be recognized as such
            if (value.GetType().IsPrimitive)
            {
                writer.WriteStartElement("value");
                writer.WriteAttributeString("xsi:type", value.GetType().Name);
                writer.WriteName(value.ToString());
                writer.WriteEndElement();
            }
            else if (value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(ObjectList<>))
            {
                writer.WriteStartElement(ResolveElementName(typeof(ObjectList<>)));
                WriteResolvedAttributes(writer, _attributeObjectBindings, typeof(ObjectList<>));
                writer.WriteAttributeString("xmlns" + ":xsi", ScenarioDocuXMLFileUtil.SchemaInstanceNamespace);
                writer.WriteAttributeString("xsi:type", "objectList");
                SerializeList(writer, value);
                writer.WriteEndElement();
            }
            else if (value.GetType() == typeof(KeyValuePair<string, object>))
            {
                SerializeKeyValuePair(writer, value);
            }
            else if (value.GetType() == typeof(Details))
            {
                SerializeDetails(writer, (Details)(object)value);
            }
            else if (value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(ObjectTreeNode<>))
            {
                var elementCreated = WriteXmlTag(writer, value);

                SerializeObjectTreeNode<object>(writer, value);

                if (elementCreated)
                {
                    writer.WriteEndElement();
                }
            }
            else if (value.GetType() == typeof(ObjectDescription))
            {
                SuppressProperties = true;
                var objDescription = (ObjectDescription)(object)value;
                SerializeObjectDescription(
                    writer,
                    objDescription.Name,
                    objDescription.Type,
                    "objectDescription",
                    ResolveElementName(typeof(ObjectDescription)),
                    objDescription);
                SuppressProperties = false;
            }
            else if (value.GetType() == typeof(ObjectReference))
            {
                var objectReference = (ObjectReference)(object)value;
                SerializeObjectReference(
                    writer,
                    objectReference.Name,
                    objectReference.Type,
                    "objectReference",
                    ResolveElementName(typeof(ObjectDescription)));
            }
            else if (value is string)
            {
                SerializeString(writer, value);
            }
        }

        /// <summary>
        /// Serializes the object tree node.
        /// </summary>
        private void SerializeObjectTreeNode<T>(XmlWriter writer, object objTreeNode)
        {
            // Changes the element name, because it depends on context where objectTree is placed (e.g. value, item)
            AddElementObjectBinding(new XmlElement(typeof(ObjectDescription), "item"), typeof(ObjectDescription));
            AddElementObjectBinding(new XmlElement(typeof(ObjectReference), "item"), typeof(ObjectReference));
            AddElementObjectBinding(new XmlElement(typeof(ObjectList<>), "item"), typeof(ObjectList<>));
            SuppressProperties = true;
            DetailElementName = "item";

            var value = (IObjectTreeNode<object>)objTreeNode;

            if (value.Item != null)
            {
                // Write generic Item only for root in TreeNode and not for
                // further childs in TreeNode
                if (IsTreeNodeRootElement)
                {
                    SerializeGenericItem(writer, "item", value.Item);
                }
            }

            if (value.Details != null)
            {
                SerializeDetails(writer, value.Details);
            }

            foreach (var child in value.Children)
            {
                writer.WriteStartElement("children");

                if (child.Item != null)
                {
                    SerializeDetails(writer, child.Item);
                }

                if (child.Details != null)
                {
                    SerializeDetails(writer, child.Details);
                }

                if (child.Children.Count > 0)
                {
                    // Write value namespace element only for root
                    _xmlTagObjectBindings.RemoveAll(e => ReferenceEquals(e.BindingObject, value.GetType()));

                    // Don't create item-tag with full qualified namespace
                    IsTreeNodeRootElement = false;

                    SerializeDetails(writer, child);
                }

                writer.WriteEndElement();
            }

            IsTreeNodeRootElement = true;
        }

        /// <summary>
        /// Serializes a key-value pair
        /// </summary>
        private void SerializeKeyValuePair<T>(XmlWriter writer, T value)
        {
            var keyValuePair = (KeyValuePair<string, object>)(object)value;

            writer.WriteStartElement("entry");
            writer.WriteElementString("key", keyValuePair.Key);
            SerializeDetails(writer, keyValuePair.Value);
            writer.WriteEndElement();
        }

        private void SerializeList<T>(XmlWriter writer, T value)
        {
            foreach (var item in (IList)value)
            {
                writer.WriteStartElement("items");
                writer.WriteAttributeString("xsi:type", string.Format("{0}{1}", "xs:", item.GetType().Name.ToLower()));
                writer.WriteValue(item.ToString());
                writer.WriteEndElement();
            }
        }

        private void SerializeString<T>(XmlWriter writer, T value)
        {
            writer.WriteStartElement("value");
            writer.WriteAttributeString(
                "xsi:type",
                string.Format("{0}{1}", "xs:", value.GetType().Name.ToLower()));
            writer.WriteString(value.ToString());
            writer.WriteEndElement();
        }

        private void SerializeObjectReference(XmlWriter writer, string xsName, string xsType, string objectName, string elementName)
        {
            writer.WriteStartElement(elementName);
            writer.WriteAttributeString("xsi:type", objectName);
            writer.WriteElementString("name", xsName);
            writer.WriteElementString("type", xsType);

            writer.WriteEndElement();
        }

        private void SerializeObjectDescription(XmlWriter writer, string xsName, string xsType, string objectName, string elementName, ObjectDescription objDescription)
        {
            writer.WriteStartElement(elementName);
            writer.WriteAttributeString("xsi:type", objectName);
            writer.WriteElementString("name", xsName);
            writer.WriteElementString("type", xsType);

            if (objDescription.Details.Properties.Any())
            {
                SerializeDetails(writer, objDescription.Details);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Serializes a detail
        /// </summary>
        private void SerializeDetails(XmlWriter writer, Details details)
        {
            if (details == null)
            {
                return;
            }

            if (!SuppressProperties)
            {
                AddElementObjectBinding(new XmlElement(typeof(ObjectDescription), "value"), typeof(ObjectDescription));
                AddElementObjectBinding(new XmlElement(typeof(Details), DetailElementName), typeof(Details));
                AddAttributeObjectBinding(new XmlAttribute(typeof(Details), "xsi:type", "details"));
                writer.WriteStartElement(
                    ResolveElementName(typeof(Details)));
                WriteResolvedAttributes(writer, _attributeObjectBindings, typeof(Details));
            }
            else
            {
                AddElementObjectBinding(new XmlElement(typeof(Details), "details"), typeof(Details));
                _attributeObjectBindings.RemoveAll(e => e.BindingObject == typeof(Details));
                writer.WriteStartElement(ResolveElementName(typeof(Details)));
                WriteResolvedAttributes(writer, _attributeObjectBindings, typeof(Details));
            }

            if (!SuppressProperties)
            {
                writer.WriteStartElement("properties");
            }

            foreach (var item in details.Properties)
            {
                SerializeDetails(writer, item);
            }

            writer.WriteEndElement();

            if (!SuppressProperties)
            {
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Serializes a Item
        /// </summary>
        private void SerializeGenericItem<T>(XmlWriter writer, string elementName, T value)
        {
            writer.WriteStartElement(elementName);
            writer.WriteAttributeString(
                "xsi:type",
                string.Format("{0}{1}", "xs:", value.GetType().Name.ToLower()));
            writer.WriteString(value.ToString());
            writer.WriteEndElement();
        }

        private void WriteResolvedAttributes(XmlWriter writer, IEnumerable<XmlAttribute> attributes, object attributeBindedObject)
        {
            foreach (var attribute in
                attributes.Where(b => b.BindingObject == attributeBindedObject))
            {
                writer.WriteAttributeString(attribute.Name, attribute.Value);
            }
        }

        private bool WriteXmlTag<T>(XmlWriter writer, T value)
        {
            var xmlTag = ResolveXmlTag(value.GetType());

            if (xmlTag != null)
            {
                writer.WriteStartElement(xmlTag.Element.Name);

                foreach (var attirbute in xmlTag.Attributes)
                {
                    writer.WriteAttributeString(attirbute.Name, attirbute.Value);
                }

                return true;
            }

            return false;
        }
    }
}
