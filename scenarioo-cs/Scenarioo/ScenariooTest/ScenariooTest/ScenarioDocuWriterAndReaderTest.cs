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
    using System.IO;

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
            // Remove all files...
            if (this.writer != null)
            {
                Directory.Delete(this.writer.DocuFiles.GetBuildDirectory(buildName), true);
            }
        }

        [TestMethod]
        public void WriteAndReadBranchDescription()
        {

            // GIVEN: a typical branch
            var branch = new Branch
                             {
                                 Name = branchName,
                                 Description = "just a simple development branch, might as well be the trunk."
                             };

            // WHEN: the branch was saved.
            writer.SaveBranchDescription(branch);

            // THEN: the branch can be loaded successfully and correctly
            Branch branchFromFile = reader.LoadBranch(buildName, branchName);
            Assert.AreEqual(branchName, branchFromFile.Name);
            Assert.AreEqual(branch.Description, branchFromFile.Description);

        }
    }
}
