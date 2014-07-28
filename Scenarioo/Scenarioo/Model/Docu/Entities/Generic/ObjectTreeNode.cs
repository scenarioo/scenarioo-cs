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
    using System.Linq;
    using System.Xml.Serialization;

    using Scenarioo.Model.Docu.Entities.Generic.Interfaces;

    [Serializable]
    public class ObjectTreeNode<T> : IObjectTreeNode<T>
    {
        /// <summary>
        /// Get the children as unmodifiable list with the expected item type.
        /// </summary>
        [XmlArray("children")]
        private readonly List<ObjectTreeNode<T>> children = new List<ObjectTreeNode<T>>();

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns;

        public ObjectTreeNode()
        {
        }

        public ObjectTreeNode(T item)
        {
            this.Item = item;
        }

        [XmlElement("details")]
        public Details Details { get; set; }

        [XmlElement("item")]
        public T Item { get; set; }

        public List<ObjectTreeNode<T>> Children
        {
            get
            {
                return this.children;
            }
        }

        /// <summary>
        /// Add a detail directly to the current node (this is different than the details on the contained item, because the
        /// same item could be referenced in different nodes having different details).
        /// </summary>
        /// <param name="key">Key of detail</param>
        /// <param name="value">Value of detail</param>
        public void AddDetail(string key, object value)
        {
            if (this.Details == null)
            {
                this.Details = new Details();
            }

            this.Details.AddDetail(key, value);
        }

        /// <summary>
        /// Add a child node. child nodes can be of different item type than the current node.
        /// </summary>
        /// <param name="child">Child to be added</param>
        public void AddChild(ObjectTreeNode<T> child)
        {
            this.children.Add(child);
        }

        public void AddChildren(IEnumerable<ObjectTreeNode<T>> childrenValue)
        {
            var objectTreeNodes = childrenValue as ObjectTreeNode<T>[] ?? childrenValue.ToArray();
            if (objectTreeNodes.Any())
            {
                this.children.AddRange(objectTreeNodes);
            }
        }

        public void AddChild(ObjectTreeNode<IObjectTreeNode<object>> childWithList)
        {
            this.children.Add(new ObjectTreeNode<T> { Item = (T)childWithList.Item, });
        }
    }
}
