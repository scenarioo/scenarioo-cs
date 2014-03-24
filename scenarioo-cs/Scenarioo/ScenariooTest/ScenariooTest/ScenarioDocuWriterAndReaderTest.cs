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
    using System.Collections.Generic;

    using Scenarioo.Api;
    using Scenarioo.Api.Serializer;
    using Scenarioo.Model.Docu.Entities;
    using Scenarioo.Model.Docu.Entities.Generic;

    [TestClass]
    public class ScenarioDocuWriterAndReaderTest
    {
        private const string BranchName = "testBranch";

        private const string BuildName = "testBuild";
        private const string UseCaseName = "testUseCase";
        private const string ScenarioName = "testScenario";
        private const string ScenarioStepName = "step";
        private const string RootDirectory = @"c:\temp";

        private const string DetailsVersionKey = "version";

        private const int StepIndex = 1;

        private ScenarioDocuWriter writer;
        private ScenarioDocuReader reader;

        [TestInitialize]
        public void TestInit()
        {
            this.writer = new ScenarioDocuWriter(
                RootDirectory,
                BranchName,
                BuildName,
                UseCaseName,
                ScenarioName,
                ScenarioStepName,
                StepIndex);

            this.reader = new ScenarioDocuReader(RootDirectory);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
     
        }

        [TestMethod]
        public void WriteAndReadBranchDescription()
        {

            // GIVEN: a typical Branch
            var branch = new Branch {
                                           Name = BranchName,
                                           Description =
                                               "just a simple development Branch, might as well be the trunk."
                                       };


            // WHEN: the Branch was saved.
            writer.SaveBranchDescription(branch);

            // THEN: the Branch can be loaded successfully and correctly
            Branch BranchFromFile = reader.LoadBranch(BuildName, BranchName);
            Assert.AreEqual(BranchName, BranchFromFile.Name);
            Assert.AreEqual(branch.Description, BranchFromFile.Description);

        }

        [TestMethod]
        public void WriteAndReadBuildDescription()
        {

            // GIVEN: a typical Build
            var build = new Build
            {
                Name = BuildName,
                Date = new DateTime(),
                Revision = "10123",
                Status = "success",
           };

            build.AddDetail(DetailsVersionKey, "1.0.1");

            // WHEN: the Build was saved.
            writer.SaveBuildDescription(build);

            // THEN: the Build can be loaded successfully and correctly
            var buildFromFile = reader.LoadBuild(BuildName, BranchName);
            Assert.AreEqual(build.Name, buildFromFile.Name);

            Assert.AreEqual(build.Date, buildFromFile.Date);
            Assert.AreEqual(build.Revision, buildFromFile.Revision);
            Assert.AreEqual(build.Status, buildFromFile.Status);
            Assert.AreEqual(build.Details.Properties.Keys, buildFromFile.Details.Properties.Keys);
            Assert.AreEqual(build.Details.Properties.Values, buildFromFile.Details.Properties.Values);
        }

        [TestMethod]
        public void WriteAndReadUseCaseDescription()
        {
            // GIVEN: a typical use case
            var usecase = new UseCase
                              {
                                  Name = UseCaseName,
                                  Description = "this is a typical use case with a decription",
                                  Status = "success",
                                  Details =
                                      new Details()
                                          {
                                              Properties =
                                                  new SerializableDictionary<string, object>()
                                                      {
                                                        {
                                                            "webtestName",
                                                            "UseCaseWebTest"
                                                        }
                                                      }
                                          }
                              };

            // WHEN: the usecase was saved.
            writer.SaveUseCase(usecase);

            // THEN: the usecase can be loaded successfully and correctly
            var useCaseFromFile = reader.LoadUseCase(BuildName, BranchName, UseCaseName);
            Assert.AreEqual(usecase.Name, useCaseFromFile.Name);
            Assert.AreEqual(usecase.Description, useCaseFromFile.Description);
            Assert.AreEqual(usecase.Status, useCaseFromFile.Status);
            Assert.AreEqual(usecase.Details.Properties.Keys, useCaseFromFile.Details.Properties.Keys);
            Assert.AreEqual(usecase.Details.Properties.Values, useCaseFromFile.Details.Properties.Values);

        }

        [TestMethod]
        public void WriteAndReadScenarioDescription()
        {

            // GIVEN: a typical scenario
            var scenario = new Scenario
                               {
                                   Name = ScenarioName,
                                   Description = "this is a typical scenario with a decription",
                                   Status = "success",
                               };

            scenario.AddDetail("userRole", "customer");

            // WHEN: the scenario was saved.
            writer.SaveScenario(scenario);

            // THEN: the scenario can be loaded successfully and correctly
            var scenarioFromFile = reader.LoadScenario(BuildName, BranchName, UseCaseName, ScenarioName);
            Assert.AreEqual(scenario.Name, scenarioFromFile.Name);
            Assert.AreEqual(scenario.Description, scenarioFromFile.Description);
            Assert.AreEqual(scenario.Status, scenarioFromFile.Status);
            Assert.AreEqual(scenario.Details.Properties.Keys, scenarioFromFile.Details.Properties.Keys);
            Assert.AreEqual(scenario.Details.Properties.Values, scenarioFromFile.Details.Properties.Values);

        }

        [TestMethod]
        public void WriteAndReadStep()
        {

            // GIVEN: a typical step
            var step = new Step();
            var stepDescription = new StepDescription
                                      {
                                          Index =  StepIndex,
                                          Title = "Test Step",
                                          Status = "success"
                                      };
            step.StepDescription = stepDescription;

            step.StepHtml = new StepHtml { htmlSource = "<html>just some page text</html>" };
            step.Page = new Page { Name = "customer/overview.jsp"};

            var stepMetadata = new StepMetadata
                                   {
                                       VisibleText = "just some page text",
                                       Details =
                                           new Details()
                                               {
                                                   Properties =
                                                       new SerializableDictionary<string, object>()
                                                           {
                                                               {
                                                                   "mockedServicesConfiguration",
                                                                   "dummy_config_xy.properties"
                                                               }
                                                           }
                                               }
                                   };
            step.StepMetadata = stepMetadata;

            // WHEN: the step was saved.
            writer.SaveStep(step);

            // THEN: the step can be loaded successfully and correctly
            var stepFromFile = reader.LoadScenarioStep(
                BuildName,
                BranchName,
                UseCaseName,
                ScenarioName,
                ScenarioStepName,
                StepIndex);

            Assert.AreEqual(StepIndex, stepFromFile.StepDescription.Index);
            Assert.AreEqual(step.StepDescription.Title, stepFromFile.StepDescription.Title);
            Assert.AreEqual(step.StepDescription.Status,stepFromFile.StepDescription.Status);
            Assert.AreEqual(step.StepHtml.htmlSource, stepFromFile.StepHtml.htmlSource);
            Assert.AreEqual(step.Page.Name, stepFromFile.Page.Name);
            Assert.AreEqual(
                step.StepMetadata.Details.Properties.Keys,
                stepFromFile.StepMetadata.Details.Properties.Keys);
            Assert.AreEqual(step.StepMetadata.Details.Properties.Values, stepFromFile.StepMetadata.Details.Properties.Values);

        }

        [TestMethod]
        public void WriteAndReadGenericCollectionsInDetails()
        {
            // GIVEN: any object containing collections in details
            var scenario = new Scenario
                               {
                                   Name = ScenarioName,
                                   Description = "a scenario for testing collections in details"
                               };

            // Details (further maps with key value pairs for structured objects)
            var detailsMap = new Details();
            detailsMap.AddDetail("anyGenericObjectReference", new ObjectReference("serviceCall", "MainDB.getUsers"));
            detailsMap.AddDetail(
                "anyGenericObject",
                new ObjectDescription("configuration", "my_dummy_mocks_configuration.properties"));
            detailsMap.AddDetail("key1", "value1");
            detailsMap.AddDetail("key2", "value2");

            scenario.Details.AddDetail("map", detailsMap);

            // List of Strings
            var objList = new ObjectList<string> { "item1", "item2", "item3" };
            scenario.Details.AddDetail("list", objList);

            // WHEN: the object was saved.
            writer.SaveScenario(scenario);

            // THEN: the collections get loaded correctly again.
            //Scenario scenarioFromFile = reader.loadScenario(TEST_Branch_NAME, TEST_BUILD_NAME, TEST_CASE_NAME,
            //TEST_SCENARIO_NAME);
            //assertEquals(list, scenarioFromFile.getDetails().getDetail("list"));
            //assertEquals(detailsMap, scenarioFromFile.getDetails().getDetail("map"));
            //assertEquals(scenario.getDetails(), scenarioFromFile.getDetails());
        }

    }
}
