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
    public class ScreenAnnotation
    {
        public ScreenRegion Region { get; set; }

        /// <summary>
        /// (optional) Set the visual style of the annotation (if not set, the same style as 'default' will be used).
        /// </summary>
        public ScreenAnnotationStyle Style { get; set; }

        /// <summary>
        /// (optional) Set a separate title text, that is only displayed inside the popup window (and not on the screen).
        /// This text is displayed instead of 'screenText' inside the popup.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// (optional) Set the text to display inside the screen on the annotation (should be short, use description 
        /// for longer texts). Too long texts might be truncated inside the screenshot view in the documentation.
        /// </summary>
        public string ScreenText { get; set; }

        /// <summary>
        /// (optional) Set a longer textual description for this annotation, this annotation is displayed below 
        /// the shorter 'text' inside an info popup that can be opened on the annotation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// (optional) Set a click action to define what happens, when the annotation is clicked on.
        /// </summary>
        public ScreenAnnotationClickAction? ClickAction { get; set; }

        /// <summary>
        /// (optional, but mandatory in case that clickAction is {@link ScreenAnnotationClickAction#toUrl})
        /// The URL to browse to when the annotation is clicked on. The URL will be opened in a separate browser tab.
        /// </summary>
        public string ClickActionUrl { get; set; }

        public DocuObjectMap Properties { get; set; }

        public ScreenAnnotation()
        {
        }

        /// <summary>
        /// Serializer black magic: 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeScreenAnnotationClickAction()
        {
            return ClickAction.HasValue;
        }

        public ScreenAnnotation(ScreenRegion region)
            : this()
        {
            Region = region;
        }
    }
}