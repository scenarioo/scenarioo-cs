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

using NUnit.Framework;

using Scenarioo.Model.Docu.Entities;

using Shouldly;

namespace ScenariooTest
{
    [TestFixture]
    public class LabelsTest
    {
        [Test]
        [TestCase("test-1")]
        [TestCase("test 1")]
        [TestCase("tEst_1")]
        public void Valid_Labels(string label)
        {
            var target = new Labels(label);
            target.First().ShouldBe(label);
        }

        [Test]
        [TestCase("test.1", "test-1")]
        [TestCase("test_1 Ã¨", "test_1 A-")]
        [TestCase("t,est", "t-est")]
        public void Invalid_Labels_Get_Normalized_With_Add(string label, string expected)
        {
            var target = new Labels();
            target.Add(label);

            target.First().ShouldBe(expected);
        }

        [Test]
        [TestCase("test+1", "test-1")]
        [TestCase("test.2", "test-2")]
        [TestCase("+\"/&%°`^?=)(.,{}![]", "-------------------")]
        public void Invalid_Labels_Get_Normalized_With_Indexer(string invalidLabel, string expected)
        {
            var labels = new Labels();
            labels.Add("bla");
            labels[0] = invalidLabel;
            
            labels[0].ShouldBe(expected);
        }
    }
}