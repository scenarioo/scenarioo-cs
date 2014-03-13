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

    using Scenarioo.Api;
    using Scenarioo.Model.Docu.Entities;

    [TestClass]
    public class ScenarioDocuWriterAndReaderTest
    {
        private const string BranchName = "testBranch";

        private const string BuildName = "testBuild";
        private const string UseCaseName = "testUseCase";
        private const string ScenarioName = "testScenario";
        private const string RootDirectory = @"c:\temp";

        private const string DetailsVersionKey = "version";

        private int TestStepIndex = 1;
        
        private ScenarioDocuWriter writer;
        private ScenarioDocuReader reader;

        [TestInitialize]
        public void TestInit()
        {
            this.writer = new ScenarioDocuWriter(RootDirectory, BranchName, BuildName, UseCaseName, ScenarioName);
            this.reader = new ScenarioDocuReader(RootDirectory);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
     
        }

        [TestMethod]
        public void WriteAndReadBranchDescription()
        {

            // GIVEN: a typical branch
            var branch = new branch()
                                       {
                                           name = BranchName,
                                           description =
                                               "just a simple development branch, might as well be the trunk."
                                       };


            // WHEN: the branch was saved.
            writer.SaveBranchDescription(branch);

            // THEN: the branch can be loaded successfully and correctly
            branch branchFromFile = reader.LoadBranch(BuildName, BranchName);
            Assert.AreEqual(BranchName, branchFromFile.name);
            Assert.AreEqual(branch.description, branchFromFile.description);

        }

        [TestMethod]
        public void WriteAndReadBuildDescription()
        {

            // GIVEN: a typical build
            var build = new build
            {
                name = BuildName,
                date = new DateTime(),
                revision = "10123",
                status = "success",
                details =
                    new[] { new buildEntry() { key = DetailsVersionKey, value = "1.0.1" } }
            };

            // WHEN: the build was saved.
            writer.SaveBuildDescription(build);

            // THEN: the build can be loaded successfully and correctly
            build buildFromFile = reader.LoadBuild(BuildName, BranchName);
            Assert.AreEqual(build.name, buildFromFile.name);

            Assert.AreEqual(build.date, buildFromFile.date);
            Assert.AreEqual(build.revision, buildFromFile.revision);
            Assert.AreEqual(build.status, buildFromFile.status);
            Assert.AreEqual(build.details[0].key, buildFromFile.details[0].key);
            Assert.AreEqual(build.details[0].value, buildFromFile.details[0].value);
        }

        [TestMethod]
        public void WriteAndReadUseCaseDescription()
        {

            // GIVEN: a typical use case
            var usecase = new useCase
                              {
                                  name = UseCaseName,
                                  description = "this is a typical use case with a decription",
                                  status = "success",
                                  details =
                                      new[]
                                          { new useCaseEntry() { key = "webtestName", value = "UseCaseWebTest" } }
                              };

            // WHEN: the usecase was saved.
            writer.SaveUseCase(usecase);

            // THEN: the usecase can be loaded successfully and correctly
            var useCaseFromFile = reader.LoadUseCase(BuildName, BranchName, UseCaseName);
            Assert.AreEqual(usecase.name, useCaseFromFile.name);
            Assert.AreEqual(usecase.description, useCaseFromFile.description);
            Assert.AreEqual(usecase.status, useCaseFromFile.status);
            Assert.AreEqual(usecase.details[0].key, useCaseFromFile.details[0].key);
            Assert.AreEqual(usecase.details[0].value, useCaseFromFile.details[0].value);

        }

        [TestMethod]
        public void WriteAndReadScenarioDescription()
        {

            // GIVEN: a typical scenario
            var scenario = new scenario
                               {
                                   name = ScenarioName,
                                   description = "this is a typical scenario with a decription",
                                   status = "success",
                                   details =
                                       new[] { new scenarioEntry() { key = "userRole", value = "customer" } }
                               };

            // WHEN: the scenario was saved.
            writer.SaveScenario(scenario);

            // THEN: the scenario can be loaded successfully and correctly
            scenario scenarioFromFile = reader.LoadScenario(BuildName, BranchName, UseCaseName, ScenarioName);
            Assert.AreEqual(scenario.name, scenarioFromFile.name);
            Assert.AreEqual(scenario.description, scenarioFromFile.description);
            Assert.AreEqual(scenario.status, scenarioFromFile.status);
            Assert.AreEqual(scenario.details[0].key, scenarioFromFile.details[0].key);
            Assert.AreEqual(scenario.details[0].value, scenarioFromFile.details[0].value);

        }
    }
}
