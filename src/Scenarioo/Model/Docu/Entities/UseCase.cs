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

using Scenarioo.Model.Docu.Entities.Generic;

namespace Scenarioo.Model.Docu.Entities
{
    public class UseCase
    {
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a unique name for this use case.
        /// Make sure to use descriptive names that stay stable as much as possible between multiple builds, such that you
        /// can compare use cases and its scenarios between different builds.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets more detailed description for current scenario (additionally to descriptive name).
        /// </summary>
        public string Description { get; set; }

        public Labels Labels { get; set; }

        /// <summary>
        /// Gets or sets status of current step for setting additional application-specific states.
        /// Status of the scenario (did this test fail or succeed?).
        /// Usual values are "success" or "failed".
        /// But you can use application specific additional values, like "not implemented", "unknown" etc. where it makes
        /// sense. Those additional values will be displayed in warning-style by the web application.
        /// </summary>
        public string Status { get; set; }

        public DocuObjectMap Properties { get; set; }
        public DocuObjectMap Sections { get; set; }

        public UseCase()
        {
            Labels = new Labels();
            Sections = new DocuObjectMap();
            Properties = new DocuObjectMap();
        }

        public UseCase(string name)
            : this()
        {
            Name = name;
        }
    }
}
