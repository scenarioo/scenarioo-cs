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

namespace Scenarioo.Api.Util.Xml
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using Exception = System.Exception;

    public class ScenarioDocuXMLUtil
    {
        public static T Unmarshal<T>(FileStream fs) where T : class
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

        // public static async Task Marshal<T>(T entity, FileStream st) where T : class
        public static void Marshal<T>(T entity, FileStream st) where T : class
        {
            if (st == null || entity == null)
            {
                throw new NullReferenceException("FileStream cannot be null");
            }

            try
            {
                var utf8 = new UTF8Encoding(false);
                var textWriter = new XmlTextWriter(st, utf8) { Indentation = 4, Formatting = Formatting.Indented };
                var serializer = new XmlSerializer(typeof(T));
                // await Task.Run(() => serializer.Serialize(st, entity));
                serializer.Serialize(textWriter, entity);
            }
            catch (SerializationException ex)
            {
                throw new Exception(string.Format("Could not marshall object of type {0}", typeof(T).Name), ex);
            }
        }
    }
}
