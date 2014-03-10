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

namespace Scenarioo.Model.Docu.Entities
{
    using Scenarioo.Model.Docu.Entities.Generic;

    public class Page
    {

        private long serialVersionUID = 1L;

        private string Name { get; set; }

        private Details details = new Details();

        public Page()
        {
            this.Name = string.Empty;
        }

        public Page(string name)
        {
            this.Name = name;
        }

        public Details Details
        {
            set
            {
                this.details = value;
            }

            get
            {
                return this.details;
            }
        }

//        public int HashCode()
//        {
//            const int Prime = 31;
//            var result = 1;
//
//            result = Prime * result + ((this.Name == null) ? 0 : this.Name.GetHashCode());
//            return result;
//        }

//        public bool Equals(Object obj)
//        {
//            if (this == obj)
//            {
//                return true;
//            }
//            if (obj == null)
//            {
//                return false;
//            }
//            if (getClass() != obj.getClass())
//            {
//                return false;
//            }
//            Page other = (Page)obj;
//            if (name == null)
//            {
//                if (other.name != null)
//                {
//                    return false;
//                }
//            }
//            else if (!name.equals(other.name))
//            {
//                return false;
//            }
//            return true;
//        }
    }
}
