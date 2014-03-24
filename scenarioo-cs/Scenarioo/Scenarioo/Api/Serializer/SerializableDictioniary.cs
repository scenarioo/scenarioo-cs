namespace Scenarioo.Api.Serializer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;

    using Scenarioo.Model.Docu.Entities.Generic;

    /// <summary>
    /// Enables to serialize generic collections and scenarioo specific generic types
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {

        /// <summary>
        /// Serializes all depending generic types from scenarioo and traverse recursively
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        public void SerializeDetails<T>(XmlWriter writer, T value)
        {

            // Primitive handles only a couple of datatypes byte, int, bool... 
            // string ist NOT a primitive type and it don`t will be recognized as such
            if (value.GetType().IsPrimitive)
            {
                CreateValueElement(writer, value.GetType().Name);
                writer.WriteName(value.ToString());
                writer.WriteEndElement();
            }
            else if (value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(ObjectList<>))
            {
                writer.WriteAttributeString("xsi:type", "objectList");
                SerializeList(writer, value);
            }
            else if (value.GetType() == typeof(KeyValuePair<string, object>))
            {
                SerializeKeyValuePair(writer, value);
            }
            else if (value.GetType() == typeof(Details))
            {
                writer.WriteStartElement("properties");
                foreach (var item in ((Details)(object)value).Properties)
                {
                    this.SerializeDetails(writer, item);
                }
                writer.WriteEndElement();

            }
            else if (value.GetType() == typeof(ObjectDescription))
            {
                var objDescription = ((ObjectDescription)(object)value);

                CreateValueElement(writer, "objectDescription");
                writer.WriteElementString("name", objDescription.Name);
                writer.WriteElementString("type", objDescription.Type);
                writer.WriteEndElement();

                if (objDescription.Details != null)
                {
                    this.SerializeDetails(writer, objDescription.Details);
                }
            }
            else if (value.GetType() == typeof(ObjectReference))
            {
                var objDescription = ((ObjectReference)(object)value);

                CreateValueElement(writer, "objectReference");
                writer.WriteElementString("name", objDescription.Name);
                writer.WriteElementString("type", objDescription.Type);
                writer.WriteEndElement();
            }
            else if (value is string)
            {
                CreateValueElement(writer, string.Format("{0}{1}", "xs:", value.GetType().Name.ToLower()));
                writer.WriteName(value.ToString());
                writer.WriteEndElement();
            }
        }

        private void SerializeKeyValuePair<T>(XmlWriter writer, T value)
        {
            var keyValuePair = ((KeyValuePair<string, object>)(object)value);

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

        private void CreateValueElement(XmlWriter writer, string value)
        {
            writer.WriteStartElement("value");
            writer.WriteAttributeString("xsi:type", value);
        }

    }

}
