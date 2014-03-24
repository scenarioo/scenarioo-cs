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
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    using Scenarioo.Model.Docu.Entities.Generic.Interface;

    [Serializable]
    public class ObjectTreeNode<T> : IObjectTreeNode<T>
    {
        [XmlElement("details")]
        public Details Details { get; set; }

        [XmlElement("children")]
        private List<ObjectTreeNode<T>> children;

        [XmlElement("item")]
        public T Item { get; set; }

        public ObjectTreeNode()
        {

        }

        public List<ObjectTreeNode<T>> Children
        {
            get
            {
                return this.children;
            }
        }

        public ObjectTreeNode(T item)
        {
            this.Item = item;
        }

        public void AddDetail(string key, object value)
        {
            if (this.Details == null)
            {
                this.Details = new Details();
            }

            this.Details.AddDetail(key, value);
        }

        public void AddChild(ObjectTreeNode<T> child)
        {
            if (this.children == null)
            {
                this.children = new List<ObjectTreeNode<T>>();
            }

            this.children.Add(child);
        }

        public void AddChildren(List<ObjectTreeNode<T>> children)
        {
            if (children != null && children.Any())
            {
                this.children.AddRange(children);
            }
        }

    }

}
