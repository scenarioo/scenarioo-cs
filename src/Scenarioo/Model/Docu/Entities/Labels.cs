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
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scenarioo.Model.Docu.Entities
{
    /// <summary>
    /// Custom implementation for IList to allow validation on labels.
    /// </summary>
    public class Labels : IList<string>
    {
        private readonly IList<string> _list = new List<string>();

        public Labels()
        {
        }

        public Labels(params string[] labels)
        {
            foreach (var label in labels)
            {
                Add(label);
            }
        }

        /// <summary>
        /// Validates a label for validity. A label must only contain letters, numbers and/or '_', '-'
        /// </summary>
        public static void AssertLabel(string label)
        {
            if (!Regex.IsMatch(label, "^[ a-zA-Z0-9_-]+$"))
            {
                throw new ArgumentException("Invalid label name: '" + label + "'");
            }
        }

        #region IList

        public IEnumerator<string> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string item)
        {
            AssertLabel(item);
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(string item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            return _list.Remove(item);
        }

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _list.IsReadOnly;
            }
        }

        public int IndexOf(string item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            AssertLabel(item);
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public string this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                AssertLabel(value);
                _list[index] = value;
            }
        }

        #endregion
    }

}
