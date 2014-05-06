namespace Scenarioo.Api.Serializer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using Scenarioo.Api.Util.Xml;
    using Scenarioo.Model.Docu.Entities.Generic;
    using Scenarioo.Model.Docu.Entities.Generic.Interfaces;

    using XmlAttribute = Scenarioo.Api.Util.Xml.XmlAttribute;
    using XmlElement = Scenarioo.Api.Util.Xml.XmlElement;

    /// <summary>
    /// Enables to serialize generic collections and scenarioo specific generic types
    /// </summary>
    public class GenericSerializer
    {

        private readonly List<XmlElement> _elementObjectBindings = new List<XmlElement>();

        private readonly List<XmlAttribute> _attributeObjectBindings = new List<XmlAttribute>();

        public string DetailElementName { get; set; }

        public bool SuppressProperties { get; set; }

        public void AddElementObjectBinding(XmlElement element, object type)
        {
            this._elementObjectBindings.RemoveAll(e => e.BindingObject == type);
            this._elementObjectBindings.Add(element);
        }

        public void AddAttributeObjectBinding(XmlAttribute attribute)
        {
            this._attributeObjectBindings.RemoveAll(e => ReferenceEquals(e.BindingObject.GetType(), attribute.BindingObject.GetType()));
            this._attributeObjectBindings.Add(attribute);
        }

        private string ResolveElementName(object type)
        {
            var firstOrDefault = this._elementObjectBindings.FirstOrDefault(e => ReferenceEquals(e.BindingObject, type));
            if (firstOrDefault != null)
            {
                return firstOrDefault.Name;
            }
            return string.Empty;
        }

        /// <summary>
        /// Serializes all depending generic types from scenarioo and traverse recursively
        /// </summary>
        /// <param name="writer">Streamoutput in which will be serialized</param>
        /// <param name="value">Any value which shall be serialized</param>
        public void SerializeDetails<T>(XmlWriter writer, T value)
        {

            // Primitive handles only a couple of datatypes byte, int, bool... 
            // string ist NOT a primitive type and it don`t will be recognized as such
            if (value.GetType().IsPrimitive)
            {
                writer.WriteStartElement("value");
                writer.WriteAttributeString("xsi:type", value.GetType().Name);
                writer.WriteName(value.ToString());
                writer.WriteEndElement();
            }
            else if (value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(ObjectList<>))
            {
                writer.WriteStartElement(this.ResolveElementName(typeof(ObjectList<>)));
                this.WriteResolvedAttributes(writer, this._attributeObjectBindings, typeof(ObjectList<>));
                writer.WriteAttributeString("xmlns" + ":xsi", ScenarioDocuXMLFileUtil.SchemaInstanceNamespace);
                writer.WriteAttributeString("xsi:type", "objectList");
                this.SerializeList(writer, value);
                writer.WriteEndElement();
            }
            // ReSharper disable once OperatorIsCanBeUsed
            else if (value.GetType() == typeof(KeyValuePair<string, object>))
            {
                this.SerializeKeyValuePair(writer, value);
            }
            else if (value.GetType() == typeof(Details))
            {
                this.SerializeDetails(writer, (Details)(object)value);
            }
            else if (value.GetType().IsGenericType
                     && value.GetType().GetGenericTypeDefinition() == typeof(ObjectTreeNode<>))
            {
                writer.WriteStartElement("value");
                writer.WriteAttributeString("xmlns:xsi", ScenarioDocuXMLFileUtil.SchemaInstanceNamespace);
                writer.WriteAttributeString("xsi:type", "objectTreeNode");

                this.SerializeObjectTreeNode<object>(writer, value);

                writer.WriteEndElement();
            }
            else if (value.GetType() == typeof(ObjectDescription))
            {
                this.SuppressProperties = true;
                var objDescription = (ObjectDescription)(object)value;
                this.SerializeObjectDescription(
                    writer,
                    objDescription.Name,
                    objDescription.Type,
                    "objectDescription",
                    this.ResolveElementName(typeof(ObjectDescription)),
                    objDescription);
                this.SuppressProperties = false;
            }
            else if (value.GetType() == typeof(ObjectReference))
            {
                var objectReference = (ObjectReference)(object)value;
                this.SerializeObjectReference(
                    writer,
                    objectReference.Name,
                    objectReference.Type,
                    "objectReference",
                    this.ResolveElementName(typeof(ObjectDescription)));
            }
            else if (value is string)
            {
               SerializeString(writer, value);
            }

        }

        /// <summary>
        /// The serialize object tree node.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="objTreeNode">
        /// The obj tree node.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        private void SerializeObjectTreeNode<T>(XmlWriter writer, object objTreeNode)
        {
            this.AddElementObjectBinding(new XmlElement(typeof(ObjectDescription), "item"), typeof(ObjectDescription));
            this.AddElementObjectBinding(new XmlElement(typeof(ObjectReference), "item"), typeof(ObjectReference));
            this.AddElementObjectBinding(new XmlElement(typeof(ObjectList<>), "item"), typeof(ObjectList<>));
            this.SuppressProperties = true;
            this.DetailElementName = "item";


            var value = (IObjectTreeNode<object>)objTreeNode;


            // ReSharper disable once CompareNonConstrainedGenericWithNull
            if (value.Item != null)
            {
                this.SerializeGenericItem(writer, "item", value.Item);
            }

            if (value.Details != null)
            {
                this.SerializeDetails(writer, value.Details);
            }

            foreach (var child in value.Children)
            {
                writer.WriteStartElement("children");

                if (child.Item != null)
                {
                    this.SerializeDetails(writer, child.Item);
                }

                if (child.Details != null)
                {
                    this.SerializeDetails(writer, child.Details);
                }

                if (child.Children.Count > 0)
                {
                    this.SerializeDetails(writer, child);
                }

                writer.WriteEndElement();
            }
        }

        // Serialization methods
        private void SerializeKeyValuePair<T>(XmlWriter writer, T value)
        {
            var keyValuePair = (KeyValuePair<string, object>)(object)value;

            writer.WriteStartElement("entry");
            writer.WriteElementString("key", keyValuePair.Key);
            this.SerializeDetails(writer, keyValuePair.Value);
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
                this.SerializeDetails(writer, objDescription.Details);
            }

            writer.WriteEndElement();
        }

        private void SerializeDetails(XmlWriter writer, Details details)
        {
            if (details == null)
            {
                return;
            }

            if (!this.SuppressProperties)
            {
                this.AddElementObjectBinding(new XmlElement(typeof(ObjectDescription), "value"), typeof(ObjectDescription));
                this.AddElementObjectBinding(new XmlElement(typeof(Details), this.DetailElementName), typeof(Details));
                this.AddAttributeObjectBinding(new XmlAttribute(typeof(Details), "xsi:type", "details"));
                writer.WriteStartElement(
                    this.ResolveElementName(typeof(Details)));
                this.WriteResolvedAttributes(writer, this._attributeObjectBindings, typeof(Details));
            }
            else
            {
                this.AddElementObjectBinding(new XmlElement(typeof(Details), "details"), typeof(Details));
                this._attributeObjectBindings.RemoveAll(e => e.BindingObject == typeof(Details));
                writer.WriteStartElement(this.ResolveElementName(typeof(Details)));
                this.WriteResolvedAttributes(writer, this._attributeObjectBindings, typeof(Details));
            }
            
            if (!this.SuppressProperties)
            {
                writer.WriteStartElement("properties");
            }

            foreach (var item in details.Properties)
            {
                this.SerializeDetails(writer, item);
            }


            writer.WriteEndElement();


            if (!this.SuppressProperties)
            {
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// The serialize generic item.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="elementName">
        /// The element name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
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
    }
}
