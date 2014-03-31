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

namespace Scenarioo.Model.Docu.Entities.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Scenarioo.Model.Docu.Entities.Generic.Interfaces;

    public class ObjectList<T> : List<T>, IObjectTreeNode<T>
    {
        private IList<T> items = new List<T>();

        public IList<T> Items
        {
            get
            {
                return this.items;
            }

            set
            {
                this.items = value;
            }
        }

        public ObjectList(IList<T> items)
        {
            this.items = items;
        }

        public ObjectList(IEnumerable<T> items)
        {
            this.items = items.ToList();
        }

        public ObjectList()
        {
            
        }

        public override int GetHashCode()
        {
            const int Prime = 31;
		    var result = 1;
		    result = Prime * result + ((this.Items == null) ? 0 : this.Items.GetHashCode());
		    return result;
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            var other = (ObjectList<T>) obj;

            if (items == null)
            {
                if (other.items != null)
                {
                    return false;
                }
            }
            else if (!items.Equals(other.items))
            {
                return false;
            }
            return true;
        }

        public Details Details
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public List<ObjectTreeNode<T>> Children
        {
            get { throw new NotImplementedException(); }
        }

        public T Item
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
