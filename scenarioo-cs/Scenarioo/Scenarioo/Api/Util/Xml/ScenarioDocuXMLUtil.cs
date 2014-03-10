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
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Exception = System.Exception;

    public class ScenarioDocuXMLUtil
    {
        // TODO: better using LINQ to XML??
        public static T Unmarshal<T>(FileStream fs) where T : class
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                return serializer.Deserialize(fs) as T;
            }
            catch (SerializationException ex)
            {
                throw new Exception(string.Format("Could not unmarshall object of type {0}", typeof(T).Name), ex);
            }
        }

        public static void Marshal<T>(T entity, FileStream st) where T : class
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(st, entity);
            }
            catch (SerializationException ex)
            {
                throw new Exception(string.Format("Could not Marshall object of type {0}", typeof(T).Name), ex);
            }
        }
    }
}
