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
using System;
using System.IO;
using System.Text;

using Scenarioo.Api;
using Scenarioo.Api.Files;
using Scenarioo.Model.Docu.Entities;
using Scenarioo.Model.Docu.Entities.Generic;
using Scenarioo.Model.Docu.Entities.Generic.Interfaces;

namespace ScenariooTest
{
    [TestClass]
    public class ScenarioDocuWriterAndReaderTest
    {
        private const string BranchName = "testBranch";

        private const string BuildName = "testBuild";

        private const string UseCaseName = "testCase";

        private const string ScenarioName = "testScenario";

        private const string ScenarioStepName = "step";

        private const string RootDirectory = @"c:\temp";

        private const string DetailsVersionKey = "version";

        private const int StepIndex = 1;

        private ScenarioDocuWriter _writer;

        private ScenarioDocuReader _reader;

        private ScenarioDocuFiles _docuFiles;


        [TestInitialize]
        public void TestInit()
        {

            this._writer = new ScenarioDocuWriter(
                RootDirectory,
                BranchName,
                BuildName);

            this._reader = new ScenarioDocuReader(RootDirectory);

            this._docuFiles = new ScenarioDocuFiles(RootDirectory);

        }

        [TestCleanup]
        public void TestCleanUp()
        {

        }

        [TestMethod]
        public void WriteAndReadBranchDescription()
        {

            // GIVEN: a typical Branch
            var branch = new Branch
                             {
                                 Name = BranchName,
                                 Description = "just a simple development Branch, might as well be the trunk."
                             };


            // WHEN: the Branch was saved. The files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this._writer.SaveBranchDescription(branch);
            this._writer.Flush();            

            // THEN: the Branch can be loaded successfully and correctly
            var branchFromFile = this._reader.LoadBranch(BranchName);
            Assert.AreEqual(BranchName, branchFromFile.Name);
            Assert.AreEqual(branch.Description, branchFromFile.Description);

        }

        [TestMethod]
        public void WriteAndReadBuildDescription()
        {

            // GIVEN: a typical Build
            var build = new Build { Name = BuildName, Date = DateTime.Today, Revision = "10123", Status = "success" };

            build.AddDetail(DetailsVersionKey, "1.0.1");

            // WHEN: the Build was saved.
            this._writer.SaveBuildDescription(build);

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this._writer.Flush();

            // THEN: the Build can be loaded successfully and correctly
//            var buildFromFile = reader.LoadBuild(BuildName, BranchName);
//            Assert.AreEqual(build.Name, buildFromFile.Name);
            
//            Assert.AreEqual(build.Date, buildFromFile.Date);
//            Assert.AreEqual(build.Revision, buildFromFile.Revision);
//            Assert.AreEqual(build.Status, buildFromFile.Status);
//            Assert.AreEqual(build.Details.Properties.Keys, buildFromFile.Details.Properties.Keys);
//            Assert.AreEqual(build.Details.Properties.Values, buildFromFile.Details.Properties.Values);
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
                              };

            usecase.AddDetail("webtestName", "UseCaseWebTest");

            // WHEN: the usecase was saved.
            this._writer.SaveUseCase(usecase);

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this._writer.Flush();

            // THEN: the usecase can be loaded successfully and correctly
            //            var useCaseFromFile = reader.LoadUseCase(BuildName, BranchName, UseCaseName);
            //            Assert.AreEqual(usecase.Name, useCaseFromFile.Name);
            //            Assert.AreEqual(usecase.Description, useCaseFromFile.Description);
            //            Assert.AreEqual(usecase.Status, useCaseFromFile.Status);
            //            Assert.AreEqual(usecase.Details.Properties.Keys, useCaseFromFile.Details.Properties.Keys);
            //            Assert.AreEqual(usecase.Details.Properties.Values, useCaseFromFile.Details.Properties.Values);

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
            this._writer.SaveScenario(UseCaseName, scenario);

            // THEN: the files are not created directly but asynchronously. WaitAll will wait until all Tasks are finished.
            this._writer.Flush();

            // THEN: the scenario can be loaded successfully and correctly
            //            var scenarioFromFile = reader.LoadScenario(BuildName, BranchName, UseCaseName, ScenarioName);
            //            Assert.AreEqual(scenario.Name, scenarioFromFile.Name);
            //            Assert.AreEqual(scenario.Description, scenarioFromFile.Description);
            //            Assert.AreEqual(scenario.Status, scenarioFromFile.Status);
            //            Assert.AreEqual(scenario.Details.Properties.Keys, scenarioFromFile.Details.Properties.Keys);
            //            Assert.AreEqual(scenario.Details.Properties.Values, scenarioFromFile.Details.Properties.Values);

        }

        [TestMethod]
        public void WriteAndReadStep()
        {

            // GIVEN: a typical step
            var step = new Step();
            var stepDescription = new StepDescription { Index = StepIndex, Title = "Test Step", Status = "success" };
            step.StepDescription = stepDescription;

            step.StepHtml = new StepHtml { HtmlSource = "<html>just some page text</html>" };
            step.Page = new Page { Name = "customer/overview.jsp" };

            step.StepMetadata = new StepMetadata
                                   {
                                       VisibleText = "just some page text",
                                   };

            step.StepMetadata.AddDetail("mockedServicesConfiguration", "dummy_config_xy.properties");

            // WHEN: the step was saved.
            this._writer.SaveStep(UseCaseName, ScenarioName, step);

            // THEN: the files are not created directly but asynchronously. WaitAll will wait until all Tasks are finished.
            this._writer.Flush();

            // THEN: the step can be loaded successfully and correctly
            //            var stepFromFile = reader.LoadStep(
            //                BuildName,
            //                BranchName,
            //                UseCaseName,
            //                ScenarioName,
            //                ScenarioStepName,
            //                StepIndex);

            //            Assert.AreEqual(StepIndex, stepFromFile.StepDescription.Index);
            //            Assert.AreEqual(step.StepDescription.Title, stepFromFile.StepDescription.Title);
            //            Assert.AreEqual(step.StepDescription.Status,stepFromFile.StepDescription.Status);
            //            Assert.AreEqual(step.StepHtml.htmlSource, stepFromFile.StepHtml.htmlSource);
            //            Assert.AreEqual(step.Page.Name, stepFromFile.Page.Name);
            //            Assert.AreEqual(
            //                step.StepMetadata.Details.Properties.Keys,
            //                stepFromFile.StepMetadata.Details.Properties.Keys);
            //            Assert.AreEqual(step.StepMetadata.Details.Properties.Values, stepFromFile.StepMetadata.Details.Properties.Values);
        }
        
        /// <summary>
        /// Tests writing and reading of a scenario docu file containing some basic collections that need to be supported.
        /// </summary>
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

            scenario.AddDetail("map", detailsMap);

            // List of Strings
            var objList = new ObjectList<string> { "item1", "item2", "item3" };
            scenario.Details.AddDetail("list", objList);

            // WHEN: the object was saved.
            this._writer.SaveScenario(UseCaseName, scenario);

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this._writer.Flush();

            // THEN: the collections get loaded correctly again.
            //Scenario scenarioFromFile = reader.loadScenario(TEST_Branch_NAME, TEST_BUILD_NAME, TEST_CASE_NAME,
            //TEST_SCENARIO_NAME);
            //assertEquals(list, scenarioFromFile.getDetails().getDetail("list"));
            //assertEquals(detailsMap, scenarioFromFile.getDetails().getDetail("map"));
            //assertEquals(scenario.getDetails(), scenarioFromFile.getDetails());
        }

        [TestMethod]
        public void WriteTreeStructureInDetails()
        {
            // GIVEN: any object containing collections in details
            var scenario = new Scenario { Name = ScenarioName, Description = "a scenario for testing trees in details" };

            // A tree containing most important item types that need to be supported
            // (same types are allowed for items in tree nodes as for values in Details --> reuse code from
            // SerializableDictionary!).

            // Root node with string as item
            var rootNode = new ObjectTreeNode<object> { Item = "Root" };
            rootNode.AddDetail(
                "detailKey",
                "Tree nodes can have again details, use same serialization as already tested!");

            // node one with object description as item
            var childWithObject = new ObjectTreeNode<object>();
            var objDescription = new ObjectDescription("serviceCall", "AddressWebService.getAdress");
            objDescription.AddDetail("justADetail", "just an example");
            childWithObject.Item = objDescription;
            rootNode.AddChild(childWithObject);

            // node two with object reference as item
            var childWithObjectRef = new ObjectTreeNode<object>();
            var objRef = new ObjectReference("serviceCall", "AddressWebService.getAdress");
            childWithObjectRef.Item = objRef;
            rootNode.AddChild(childWithObjectRef);

           // node three with List of Strings as item
            var childWithList = new ObjectTreeNode<IObjectTreeNode<object>>();
            var list = new ObjectList<object> { "item1", "item2", "item3" };
            childWithList.Item = list;
            rootNode.AddChild(childWithList);

            // node four with details as item
            var childWithDetails = new ObjectTreeNode<object>();
            var detailsMap = new Details();
            detailsMap.AddDetail("key1", "value1");
            detailsMap.AddDetail("key2", "value2");
            detailsMap.AddDetail("anyGenericObjectReference", new ObjectReference("serviceCall", "MainDB.getUsers"));
            detailsMap.AddDetail(
                "anyGenericObject",
                new ObjectDescription("configuration", "my_dummy_mocks_configuration.properties"));
            childWithDetails.Item = detailsMap;
            rootNode.AddChild(childWithDetails);

            scenario.AddDetail("exampleTree", rootNode);

            // WHEN: the object was saved.
            this._writer.SaveScenario(UseCaseName, scenario);

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this._writer.Flush();
        }


        /// <summary>
        /// Test that the files are written asynchronously and that the flush method waits correctly for the last file to bewritten.
        /// </summary>
        [TestMethod]
        public void AsyncWriteOfMultipleFilesAndFlush()
        {
            // GIVEN: a lot of large steps to write, that have not yet been written 
            var steps = new Step[10];
            for (var index = 0; index < 10; index++)
            {
                steps[index] = this.CreateBigDataStepForLoadTestAsyncWriting(index + 1);
            }

            var expectedFileForSteps = this._docuFiles.GetScenarioStepFile(
                BranchName,
                BuildName,
                UseCaseName,
                ScenarioName,
                10);

            if (File.Exists(expectedFileForSteps))
            {
                File.Delete(expectedFileForSteps);
            }

            Assert.IsFalse(File.Exists(expectedFileForSteps));

            // WHEN: saving those steps, 
            foreach (var step in steps)
            {
                this._writer.SaveStep(UseCaseName, ScenarioName, step);
            }

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this._writer.Flush();
            Assert.IsTrue(File.Exists(expectedFileForSteps));
        }

        private Step CreateBigDataStepForLoadTestAsyncWriting(int index)
        {

            var step = new Step();

            // Description
            var stepDescription = new StepDescription
                                      {
                                          Index = index,
                                          ScreenshotFileName =
                                              this._docuFiles.GetScreenshotFile(
                                                  BranchName,
                                                  BuildName,
                                                  UseCaseName,
                                                  ScenarioName,
                                                  index),
                                          Title =
                                              "this is a step with a lot of data in it such that writing should take realy long for testing async writing\n"
                                      };

            step.StepDescription = stepDescription;

            // Metdata with a lot of details 
            step.StepMetadata = CreateBigMetadata();

            // Page 
            step.Page = new Page("test.jsp");

            // HTML (lot of dummy data, just to generate big data for writing) 
            step.StepHtml = this.CreateBigHtml();

            return step;
        }

        /// <summary>
        /// Simply generate a big dummy html for load testing of writing.
        /// </summary>
        /// <returns>Created Html</returns>
        public StepHtml CreateBigHtml()
        {

            var builder = new StringBuilder();
            builder.Append("<html><head></head><body>");
            builder.Append(
                "<p>This is just a dummy html code with lot of content to generate a lot of big data to write for load testing.<p>");
            for (int i = 0; i < 1000; i++)
            {
                builder.Append(
                    "<div class=\"dummyParagraph" + i
                    + "\">This is just a dummy html code with lot of content to generate a lot of big data to write for load testing.</div>");
            }
            builder.Append("</body></html>");

            var html = new StepHtml { HtmlSource = builder.ToString() };
            return html;
        }

        /// <summary>
        /// Simply generate StepMetadata object with a lot of details to get a big step for load testing of writing.
        /// </summary>
        /// <returns>Generated StepMetadata</returns>
        private StepMetadata CreateBigMetadata()
        {
            var stepMetadata = new StepMetadata();
            for (var i = 0; i < 1000; i++)
            {
                stepMetadata.Details.AddDetail("detail" + i, "just a detail to produce a lot of data that needs marshalling and writing.");
            }

            return stepMetadata;
        }
    }
}
