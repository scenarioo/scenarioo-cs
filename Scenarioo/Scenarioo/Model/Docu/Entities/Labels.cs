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

using Scenarioo.Annotations;

namespace Scenarioo.Model.Docu.Entities
{
    public class Labels : IList<string>
    {
        private IList<string> labels = new List<string>();

        public bool IsReadOnly { get; [UsedImplicitly] private set; }

        public int Count
        {
            get
            {
                return this.labels.Count;
            }
        }

        public string this[int index]
        {
            get
            {
                return this.labels[index];
            }

            set
            {
                this.labels[index] = value;
            }
        }

        /// <summary>
        /// Validates a label for validity. A label must only contain letters, numbers and/or '_', '-'
        /// </summary>
        public static bool IsValidLabel(string label)
        {
            return Regex.IsMatch(label, "^[ a-zA-Z0-9_-]+$");
        }

        public Labels Add(string label)
        {
            if (IsValidLabel(label))
            {
                this.labels.Add(label);
            }
            else
            {
                throw new ArgumentException("Invalid label name: '" + label + "'");
            }

            return this;
        }

        public void Clear()
        {
            this.labels.Clear();
        }

        public bool Contains(string item)
        {
            return this.labels.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            this.labels.CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            return this.labels.Remove(item);
        }

        public void Set(IEnumerable<string> labelsToSet)
        {
            var labelsCopy = new List<string>();
            foreach (var label in labelsToSet)
            {
                if (IsValidLabel(label))
                {
                    labelsCopy.Add(label);
                }
                else
                {
                    throw new ArgumentException("Invalid label name: '" + label + "'");
                }
            }

            this.labels = labelsCopy;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.labels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void ICollection<string>.Add(string item)
        {
            this.labels.Add(item);
        }

        public int IndexOf(string item)
        {
            return this.labels.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            this.labels.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.labels.RemoveAt(index);
        }
    }
}
