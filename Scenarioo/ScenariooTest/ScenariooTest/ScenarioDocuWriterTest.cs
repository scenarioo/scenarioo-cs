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
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Scenarioo.Api;
using Scenarioo.Api.Files;
using Scenarioo.Model.Docu.Entities;
using Scenarioo.Model.Docu.Entities.Generic;
using Scenarioo.Model.Docu.Entities.Generic.Interfaces;

namespace ScenariooTest
{
    [TestClass]
    public class ScenarioDocuWriterTest
    {
        private const string BranchName = "testBranch";

        private const string BuildName = "testBuild";

        private const string UseCaseName = "testCase";

        private const string ScenarioName = "testScenario";

        private const string DetailsVersionKey = "version";

        private const int StepIndex = 1;
        
        private string rootDirectory;

        private ScenarioDocuWriter writer;

        private ScenarioDocuReader reader;

        private ScenarioDocuFiles docuFiles;

        [TestInitialize]
        public void TestInit()
        {
            // Sets outcome directory
            this.rootDirectory = @"C:\Temp\testBranch"; // Path.Combine(Directory.GetCurrentDirectory(), "testoutcome");

            if (!Directory.Exists(this.rootDirectory))
            {
                Directory.CreateDirectory(this.rootDirectory);
            }

            this.writer = new ScenarioDocuWriter(
                this.rootDirectory,
                BranchName,
                BuildName);

            this.reader = new ScenarioDocuReader(this.rootDirectory);

            this.docuFiles = new ScenarioDocuFiles(this.rootDirectory);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            //if (Directory.Exists(this.rootDirectory))
            //{
            //    Directory.Delete(this.rootDirectory, true);
            //}
        }

        [TestMethod]
        public void WriteBranchDescription()
        {
            // GIVEN: a typical Branch
            var branch = new Branch
                             {
                                 Name = BranchName,
                                 Description = "just a simple development Branch, might as well be the trunk."
                             };

            // WHEN: the Branch was saved. The files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this.writer.SaveBranchDescription(branch);
            this.writer.Flush();            

            // THEN: the Branch can be loaded successfully and correctly
            var branchFromFile = this.reader.LoadBranch(BranchName);
            Assert.AreEqual(BranchName, branchFromFile.Name);
            Assert.AreEqual(branch.Description, branchFromFile.Description);
        }

        [TestMethod]
        public void WriteBuildDescription()
        {
            // GIVEN: a typical Build
            var build = new Build { Name = BuildName, Date = DateTime.Today, Revision = "10123", Status = "success" };

            build.AddDetail(DetailsVersionKey, "1.0.1");

            // WHEN: the Build was saved.
            this.writer.SaveBuildDescription(build);

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this.writer.Flush();

            // THEN: the Build file exists
            Assert.IsTrue(File.Exists(this.docuFiles.GetBuildFile(BranchName, BuildName)));
        }

        [TestMethod]
        public void WriteUseCaseDescription()
        {
            // GIVEN: a typical use case
            var usecase = new UseCase
                              {
                                  Name = UseCaseName,
                                  Description = "this is a typical use case with a decription",
                                  Status = "success",
                              };

            usecase.AddDetail("webtestName", "UseCaseWebTest");
            usecase.Labels.AddLabels(this.CreateLabels());

            // WHEN: the usecase was saved.
            this.writer.SaveUseCase(usecase);

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this.writer.Flush();

            // THEN: the usecase file exists
            Assert.IsTrue(File.Exists(this.docuFiles.GetUseCaseFile(BranchName, BuildName, UseCaseName)));
        }

        [TestMethod]
        public void WriteScenarioDescription()
        {
            // GIVEN: a typical scenario
            var scenario = new Scenario
                               {
                                   Name = ScenarioName,
                                   Description = "this is a typical scenario with a decription",
                                   Status = "success",
                               };

            scenario.AddDetail("userRole", "customer");
            scenario.Labels.AddLabels(this.CreateLabels());

            // WHEN: the scenario was saved.
            this.writer.SaveScenario(UseCaseName, scenario);

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this.writer.Flush();

            // THEN: the scneario file exists
            Assert.IsTrue(File.Exists(this.docuFiles.GetScenarioFile(BranchName, BuildName, UseCaseName, ScenarioName)));
        }

        [TestMethod]
        public void WriteStep()
        {
            // GIVEN: a typical step
            var step = new Step();
            var labels = new Labels();
            labels.AddLabels(this.CreateLabels());

            var stepDescription = new StepDescription
                                      {
                                          Index = StepIndex,
                                          Title = "Test Step",
                                          Status = "success",
                                          Labels = labels,
                                          ScreenshotFileName = this.docuFiles.GetScreenshotFileName(StepIndex)
                                      };

            step.StepDescription = stepDescription;
            step.StepHtml = new StepHtml { HtmlSource = "<html>just some page text</html>" };
            step.Page = new Page { Name = "customer/overview.jsp", Labels = labels };

            step.StepMetadata = new StepMetadata
                                   {
                                       VisibleText = "just some page text",
                                   };

            step.StepMetadata.AddDetail("mockedServicesConfiguration", "dummy_config_xy.properties");

            // WHEN: the step was saved.
            this.writer.SaveStep(UseCaseName, ScenarioName, step);

            // THEN: the files are not created directly but asynchronously. WaitAll will wait until all Tasks are finished.
            this.writer.Flush();

            // THEN: at least one step file exists and one scenario file exists
            Assert.IsTrue(File.Exists(this.docuFiles.GetScenarioStepFile(BranchName, BuildName, UseCaseName, ScenarioName, 1)));
        }
        
        [TestMethod]
        public void WriteGenericCollectionsInDetails()
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
            this.writer.SaveScenario(UseCaseName, scenario);

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this.writer.Flush();

            // THEN: the scneario file exists
            Assert.IsTrue(File.Exists(this.docuFiles.GetScenarioFile(BranchName, BuildName, UseCaseName, ScenarioName)));
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
            this.writer.SaveScenario(UseCaseName, scenario);

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this.writer.Flush();

            // THEN: the scneario file exists
            Assert.IsTrue(File.Exists(this.docuFiles.GetScenarioFile(BranchName, BuildName, UseCaseName, ScenarioName)));
        }

        [TestMethod]
        public void AsyncWriteOfMultipleFilesAndFlush()
        {
            // GIVEN: a lot of large steps to write, that have not yet been written 
            var steps = new Step[10];
            for (var index = 0; index < 10; index++)
            {
                steps[index] = this.CreateBigDataStepForLoadTestAsyncWriting(index + 1);
            }

            var expectedFileForSteps = this.docuFiles.GetScenarioStepFile(
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
                this.writer.SaveStep(UseCaseName, ScenarioName, step);
            }

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            this.writer.Flush();
            Assert.IsTrue(File.Exists(expectedFileForSteps));
        }

        private static StepHtml CreateBigHtml()
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
        private static StepMetadata CreateBigMetadata()
        {
            var stepMetadata = new StepMetadata();
            for (var i = 0; i < 1000; i++)
            {
                stepMetadata.Details.AddDetail("detail" + i, "just a detail to produce a lot of data that needs marshalling and writing.");
            }

            return stepMetadata;
        }

        private Step CreateBigDataStepForLoadTestAsyncWriting(int index)
        {
            var step = new Step();

            // Description
            var stepDescription = new StepDescription
                                      {
                                          Index = index,
                                          ScreenshotFileName =
                                              this.docuFiles.GetScreenshotFile(
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

            // Creates HTML (lot of dummy data, just to generate big data for writing) 
            step.StepHtml = CreateBigHtml();

            return step;
        }

        private IEnumerable<string> CreateLabels()
        {
            var labels = new List<string> { "internetz", "test", "scenarioo-test-case" };

            return labels;
        }
    }
}
