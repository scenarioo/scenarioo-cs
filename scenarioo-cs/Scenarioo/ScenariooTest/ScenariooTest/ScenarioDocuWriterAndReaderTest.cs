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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScenariooTest
{
    using System;
    using System.IO;
    using System.Threading;

    using Scenarioo.Api;
    using Scenarioo.Model.Docu.Entities;

    [TestClass]
    public class ScenarioDocuWriterAndReaderTest
    {

        private const string branchName = "testBranch";
        private const string buildName = "testBuild";
        private const string testCaseName = "testCase";
        private const string testScenarioName = "testScenario";
        private const string rootDirectory = @"c:\temp";

        private const string detailsVersionKey = "version";

        private int TestStepIndex = 1;
        
        private ScenarioDocuWriter writer;
        private ScenarioDocuReader reader;

        [TestInitialize]
        public void TestInit()
        {
            this.writer = new ScenarioDocuWriter(rootDirectory, branchName, buildName);
            this.reader = new ScenarioDocuReader(rootDirectory);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
     
        }

        [TestMethod]
        public void WriteAndReadBranchDescription()
        {

            // GIVEN: a typical branch
            var brunchFromSchema = new branch()
                                       {
                                           name = branchName,
                                           description =
                                               "just a simple development branch, might as well be the trunk."
                                       };


            // WHEN: the branch was saved.
            writer.SaveBranchDescription(brunchFromSchema);

            // THEN: the branch can be loaded successfully and correctly
            branch branchFromFile = reader.LoadBranch(buildName, branchName);
            Assert.AreEqual(branchName, branchFromFile.name);
            Assert.AreEqual(brunchFromSchema.description, branchFromFile.description);

        }

        [TestMethod]
        public void WriteAndReadBuildDescription()
        {

            // GIVEN: a typical build
            var buildFromSchema = new build
            {
                name = buildName,
                date = new DateTime(),
                revision = "10123",
                status = "success",
                details =
                    new[] { new buildEntry() { key = detailsVersionKey, value = "1.0.1" } }
            };

            // WHEN: the build was saved.
            writer.SaveBuildDescription(buildFromSchema);

            // Preventing that async write process is slower than read process
            Thread.Sleep(1);

            // THEN: the build can be loaded successfully and correctly
            build buildFromFile = reader.LoadBuild(buildName, branchName);
            Assert.AreEqual(buildFromSchema.name, buildFromFile.name);

            Assert.AreEqual(buildFromSchema.date, buildFromFile.date);
            Assert.AreEqual(buildFromSchema.revision, buildFromFile.revision);
            Assert.AreEqual(buildFromSchema.status, buildFromFile.status);
            Assert.AreEqual(buildFromSchema.details[0].key, buildFromFile.details[0].key);
            Assert.AreEqual(buildFromSchema.details[0].value, buildFromFile.details[0].value);
        }

    }
}
