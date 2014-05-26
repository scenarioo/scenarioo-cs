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
    /// <summary>
    /// Contains an element which will be binded to an object type. This is relevant
    /// to decide, which element has to be binded to an object type during serialization
    /// </summary>
    public class XmlElement
    {
        public string Name { get; set; }

        public string ElementString { get; set; }

        public object BindingObject { get; set; }

        public string ObjectName { get; set; }

        public XmlElement()
        {

        }
        
        public XmlElement(object bindingObject, string name, string elementString)
        {
            this.Name = name;
            this.ElementString = elementString;
            this.BindingObject = bindingObject;
        }

        public XmlElement(object bindingObject, string name)
        {
            this.Name = name;
            this.BindingObject = bindingObject;
        }
    }
}
