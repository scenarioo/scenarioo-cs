﻿/* scenarioo-api
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

using System.Collections.Generic;

using Newtonsoft.Json;

using Scenarioo.Model.Docu.Entities.Generic;

namespace Scenarioo.Model.Docu.Entities
{
    /// <summary>
    /// Contains all the data collected from a web test for one step of one scenario/web test (except for the step image, which
    /// has to be stored separately
    /// </summary>
    public class Step
    {
        public int Index { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        
        /// <summary>
        /// Gets or sets information about the page this step belongs to (usually there are several steps that show the same UI page).
        /// This information is optional in case you do not have a page concept in your application.
        /// </summary>
        public Page Page { get; set; }

        /// <summary>
        /// Gets or sets optional information for web applications about the html output of current step.
        /// </summary>
        [JsonIgnore]
        public string StepHtml { get; set; }

        public List<ScreenAnnotation> ScreenAnnotations { get; set; }

        public DocuObjectMap Properties { get; set; }

        public DocuObjectMap PropertyGroups { get; set; }

        public Labels Labels { get; set; }

        public Step()
        {
            Labels = new Labels();
            Properties = new DocuObjectMap();
            PropertyGroups = new DocuObjectMap();
            ScreenAnnotations = new List<ScreenAnnotation>();
        }
    }
}
