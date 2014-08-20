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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Scenarioo.Model.Docu.Entities;

namespace ScenariooTest
{
    [TestClass]
    public class LabelsTest
    {
        [TestMethod]
        public void AddChaining()
        {
            var labels = new Labels();
            labels.Add("test-1").Add("test-2");

            Assert.AreEqual(2, labels.Count);
        }

        [TestMethod]
        public void Set()
        {
            var labelsToSet = new Labels { "valid", "valid 2" };
            var labels = labelsToSet;

            Assert.AreEqual(2, labels.Count);
        }

        [TestMethod]
        public void Validation()
        {
            Assert.IsTrue(Labels.IsValidLabel("test-1"));
            Assert.IsTrue(Labels.IsValidLabel("test 1"));
            Assert.IsTrue(Labels.IsValidLabel("tEst_1"));

            Assert.IsFalse(Labels.IsValidLabel("test.1"));
            Assert.IsFalse(Labels.IsValidLabel("test_1 Ã¨"));
            Assert.IsFalse(Labels.IsValidLabel("t,est"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ImmediateValidationForAdd()
        {
            var labels = new Labels();
            labels.Add("test-1").Add("test.2");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ImmediateValidationForSet()
        {
            var labels = new Labels();
            var labelsToSet = new Labels { "valid", ".invalid" };
            labels.Set(labelsToSet);
        }
    }
}
