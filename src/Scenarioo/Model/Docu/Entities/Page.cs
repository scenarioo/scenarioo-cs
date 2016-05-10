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

using System.Collections.Generic;

using Scenarioo.Model.Docu.Entities.Generic;

namespace Scenarioo.Model.Docu.Entities
{
    public class Page
    {
        /// <summary>
        /// Gets or sets unique name of the page. For a webpage you usually use the relative application internal url-path to that page or
        /// the relative-file-path of the template file rendering this page. Try to use names that are as short as possible
        /// and do not include any url parameters. Names should be as stable as possible, do not use names that might change
        /// on each new build.
        /// </summary>
        public string Name { get; set; }

        public string Id { get; set; }

        public List<DocuObject> Properties { get; set; }

        /// <summary>
        /// Gets or sets multiple _labels to a scenario object.
        /// </summary>
        /// <returns>All _labels of this object. Never null.</returns>
        public Labels Labels { get; set; }

        /// <summary>
        /// Gets or sets all visible text of a step here to provide possibility to search inside visible step text.
        /// But currently the client web application does not yet support full text search anyway and also not to display
        /// this visible text anywhere in the web application.
        /// </summary>
        public string VisibleText { get; set; }

        public List<ScreenAnnotation> ScreenAnnotations { get; set; }

        public Page()
        {
            Labels = new Labels();
            Properties = new List<DocuObject>();
            ScreenAnnotations = new List<ScreenAnnotation>();
        }

        public Page(string name)
            : this()
        {
            Name = name;
        }
    }
}
