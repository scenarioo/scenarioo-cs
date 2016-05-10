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
            Should.NotThrow(() => Labels.AssertLabel(label));
        }

        [Test]
        [TestCase("test.1")]
        [TestCase("test_1 Ã¨")]
        [TestCase("t,est")]
        public void Invalid_Labels(string label)
        {
            Should.Throw<ArgumentException>(() => Labels.AssertLabel(label));
        }

        [Test]
        [TestCase("test+1")]
        [TestCase("test.2")]
        public void Add_Throws_If_Label_Is_Not_Valid(string invalidLabel)
        {
            var labels = new Labels();

            Should.Throw<ArgumentException>(() => labels.Add(invalidLabel));
        }

        [Test]
        [TestCase(".invalid")]
        public void Indexer_Throws_If_Label_Is_Not_Valid(string invalidLabel)
        {
            var labels = new Labels();

            Should.Throw<ArgumentException>(() => labels[0] = invalidLabel);
        }
    }
}