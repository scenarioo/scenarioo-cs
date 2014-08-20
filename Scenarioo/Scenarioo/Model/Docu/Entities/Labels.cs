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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scenarioo.Model.Docu.Entities
{
    [Serializable]
    public class Labels : IXmlSerializable
    {
        private IList<string> labels = new List<string>();

        public int Count
        {
            get
            {
                return this.labels.Count;
            }
        }

        public IList<string> LabelList
        {
            get
            {
                return this.labels;
            }
        }
        
        /// <summary>
        /// Validates a label for validity. A label must only contain letters, numbers and/or '_', '-'
        /// </summary>
        public static bool IsValidLabel(string label)
        {
            return Regex.IsMatch(label, "^[ a-zA-Z0-9_-]+$");
        }

        public Labels AddLabel(string label)
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

        public void AddLabels(IEnumerable<string> labelsToSet)
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
        
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var label in this.labels)
            {
                writer.WriteElementString("label", label);
            }
        }
    }
}
