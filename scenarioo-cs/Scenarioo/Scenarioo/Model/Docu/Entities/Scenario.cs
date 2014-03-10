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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scenarioo.Model.Docu.Entities
{
    using Scenarioo.Model.Docu.Entities.Generic;

    public class Scenario
    {

        private static long serialVersionUID = 1L;

        private string Name { get; set; }

        private string Description { get; set; }

        private string Status { get; set; }

        private Details details = new Details();

        private ScenarioCalculatedData calculatedData = new ScenarioCalculatedData();

        public Scenario()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Status = string.Empty;
        }

        public Scenario(string name, string description, int numberOfPages, int numberOfSteps)
            : base()
        {
            this.Name = name;
            this.Description = description;
        }

        public Details Details
        {
            get
            {
                return this.details;
            }

            set
            {
                this.details = value;
            }
        }

        public ScenarioCalculatedData CalculatedData
        {
            get
            {
                return this.calculatedData;
            }

            set
            {
                this.calculatedData = value;
            }
        }

        public void AddDetail(string key, Object value)
        {
            this.details.AddDetail(key, value);
        }

    }
}
