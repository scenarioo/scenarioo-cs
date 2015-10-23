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
using System.Xml.Serialization;

namespace Scenarioo.Model.Docu.Entities
{
    /// <summary>
    /// Only for internal use, wherever ths class is used on the model, this is for internal use only, will be removed in
    /// next major version of the API.
    /// </summary>
    [Serializable]
    [XmlRoot("stepIdentification")]
    class StepIdentification
    {
        [XmlElement("useCaseName")]
        public string UseCaseName { get; set; }

        [XmlElement("scenarioName")]
        public string ScenarioName { get; set; }

        [XmlElement("pageName")]
        public string PageName { get; set; }

        [XmlElement("index")]
        public int Index { get; set; }

        [XmlElement("occurence")]
        public int Occurence { get; set; }

        [XmlElement("relativeIndex")]
        public int RelativeIndex { get; set; }

        public StepIdentification(
            string useCaseName,
            string scenarioName,
            string pageName,
            int index,
            int occurence,
            int relativeIndex)
        {
            UseCaseName = useCaseName;
            ScenarioName = scenarioName;
            PageName = pageName;
            Index = index;
            Occurence = occurence;
            RelativeIndex = relativeIndex;
        }
    }
}
