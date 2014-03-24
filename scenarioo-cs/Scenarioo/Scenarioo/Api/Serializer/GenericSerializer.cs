namespace Scenarioo.Api.Serializer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using Scenarioo.Api.Util.Xml;
    using Scenarioo.Model.Docu.Entities.Generic;
    using Scenarioo.Model.Docu.Entities.Generic.Interface;

    /// <summary>
    /// Enables to serialize generic collections and scenarioo specific generic types
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class GenericSerializer
    {
        /// <summary>
        /// Serializes all depending generic types from scenarioo and traverse recursively
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="key"></param>
        public static void Serializer<T>(XmlWriter writer, T value, string key)
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
                writer.WriteStartElement("value");
                writer.WriteAttributeString("xmlns" + ":xsi", ScenarioDocuXMLFileUtil.SchemaInstanceNamespace);
                writer.WriteAttributeString("xsi:type", "objectList");
                SerializeList(writer, value);
                writer.WriteEndElement();
            }
            else if (value.GetType() == typeof(KeyValuePair<string, object>))
            {
                SerializeKeyValuePair(writer, value, key);
            }
            else if (value.GetType() == typeof(Details))
            {
                writer.WriteStartElement("value");
                writer.WriteAttributeString("xmlns:xsi", ScenarioDocuXMLFileUtil.SchemaInstanceNamespace);
                writer.WriteAttributeString("xsi:type", "details");
                writer.WriteStartElement("properties");
                foreach (var item in ((Details)(object)value).Properties)
                {
                    Serializer(writer, item, key);
                }
                writer.WriteEndElement();
                writer.WriteEndElement();

            }
            else if (value.GetType().IsGenericType
                     && value.GetType().GetGenericTypeDefinition() == typeof(ObjectTreeNode<>))
            {

                var objTreeNode = ((IObjectTreeNode<T>)value).Children;

                foreach (var child in objTreeNode)
                {
                    if (child.Children != null)
                    {
                        Serializer(writer, child, key);
                    }

                    // ReSharper disable once CompareNonConstrainedGenericWithNull
                    if (child.Item != null)
                    {
                        Serializer(writer, child.Item, key);
                    }
                }
            }
            else if (value.GetType() == typeof(ObjectDescription))
            {
                var objDescription = ((ObjectDescription)(object)value);

                writer.WriteStartElement("value");
                writer.WriteAttributeString("xsi:type", "objectDescription");
                writer.WriteElementString("name", objDescription.Name);
                writer.WriteElementString("type", objDescription.Type);
                writer.WriteEndElement();

                if (objDescription.Details.Properties.Any())
                {
                    Serializer(writer, objDescription.Details, key);
                }
            }
            else if (value.GetType() == typeof(ObjectReference))
            {
                var objDescription = ((ObjectReference)(object)value);

                writer.WriteStartElement("value");
                writer.WriteAttributeString("xsi:type", "objectReference");
                writer.WriteElementString("name", objDescription.Name);
                writer.WriteElementString("type", objDescription.Type);
                writer.WriteEndElement();
            }
            else if (value is string)
            {
                writer.WriteStartElement("value");
                writer.WriteAttributeString(
                    "xsi:type",
                    string.Format("{0}{1}", "xs:", value.GetType().Name.ToLower()));
                writer.WriteString(value.ToString());
                writer.WriteEndElement();
            }
        }

        private static void SerializeKeyValuePair<T>(XmlWriter writer, T value, string key)
        {
            var keyValuePair = ((KeyValuePair<string, object>)(object)value);

            writer.WriteStartElement("entry");
            writer.WriteElementString("key", keyValuePair.Key);
            Serializer(writer, keyValuePair.Value, key);
            writer.WriteEndElement();
        }

        private static void SerializeList<T>(XmlWriter writer, T value)
        {
            foreach (var item in (IList)value)
            {
                writer.WriteStartElement("items");
                writer.WriteAttributeString("xsi:type", string.Format("{0}{1}", "xs:", item.GetType().Name.ToLower()));
                writer.WriteValue(item.ToString());
                writer.WriteEndElement();
            }
        }

    }

}
