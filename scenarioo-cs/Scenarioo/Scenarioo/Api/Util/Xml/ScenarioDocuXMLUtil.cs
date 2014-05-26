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
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Scenarioo.Api.Util.Xml
{
    using Exception = System.Exception;

    /// <summary>
    /// Writing or reading of all ScenarioDocu entities to XML files and back
    /// </summary>
    public class ScenarioDocuXMLUtil
    {
        public static T UnmarshalXml<T>(FileStream fs) where T : class
        {

            if (fs == null)
            {
                throw new NullReferenceException("FileStream cannot be null");
            }

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                var deserializedObject = serializer.Deserialize(fs) as T;

                return deserializedObject;

            }
            catch (SerializationException ex)
            {
                throw new Exception(string.Format("Could not unmarshall object of type {0}", typeof(T).Name), ex);
            }
        }

        public static void MarshalXml<T>(T entity, FileStream st) where T : class
        {
            if (st == null || entity == null)
            {
                throw new NullReferenceException("FileStream cannot be null");
            }

            try
            {
                var utf8 = new UTF8Encoding(false);
                var serializer = new XmlSerializer(typeof(T));

                serializer.Serialize(
                    new XmlTextWriter(st, utf8) { Indentation = 4, Formatting = Formatting.Indented }, entity);

            }
            catch (SerializationException ex)
            {
                throw new Exception(string.Format("Could not marshall object of type {0}", typeof(T).Name), ex);
            }
        }
    }
}
