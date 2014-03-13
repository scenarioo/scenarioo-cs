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
using System.Collections.Generic;

namespace Scenarioo.Model.Docu.Entities.Generic
{
    using System.Xml;
    using System.Xml.Serialization;

    [Serializable]
    public class Details: IXmlSerializable
    {
        public Dictionary<string, object> Properties { get; set; }

        public Details()
        {
            this.Properties = new Dictionary<string, object>();
        }

        public void AddDetail(string key, Object value)
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

        public void WriteXml(XmlWriter writer)
        {
            if (this.Properties == null)
            {
                return;
            }

            var serializer = new XmlSerializer(this.Properties.GetType());
            serializer.Serialize(writer, this.Properties);
        }

        public void ReadXml(XmlReader reader)
        {
            if (this.Properties == null)
            {
                this.Properties = new Dictionary<string, object>();
            }
            this.AddDetail(reader.ReadElementString("Key"), reader.ReadElementContentAsObject());
        }
    }
}
