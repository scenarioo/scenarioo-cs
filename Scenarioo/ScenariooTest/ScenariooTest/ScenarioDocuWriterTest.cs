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
using System.IO;
using System.Text;

using NUnit.Framework;

using Scenarioo.Api;
using Scenarioo.Api.Files;
using Scenarioo.Model.Docu.Entities;
using Scenarioo.Model.Docu.Entities.Generic;
using Scenarioo.Model.Docu.Entities.Generic.Interfaces;

namespace ScenariooTest
{
    [TestFixture]
    public class ScenarioDocuWriterTest
    {
        private const string BranchName = "csharp-writer-unit-tests";
        private const string BuildName = "testBuild";
        private const string SerializationUseCase = "Serialization";
        private const string ScenarioName = "testScenario";
        private const string DetailsVersionKey = "version";
        private const int StepIndex = 1;
        
        private string rootDirectory;

        private ScenarioDocuWriter writer;
        private ScenarioDocuReader reader;
        private ScenarioDocuFiles docuFiles;

        [TestFixtureSetUp]
        public void TestInit()
        {
            // Sets outcome directory
            rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "testoutcome");
            TestCleanUp();
            
            if (!Directory.Exists(rootDirectory))
            {
                Directory.CreateDirectory(rootDirectory);
            }

            writer = new ScenarioDocuWriter(
                rootDirectory,
                BranchName,
                BuildName);

            reader = new ScenarioDocuReader(rootDirectory);

            docuFiles = new ScenarioDocuFiles(rootDirectory);
        }

        //[TestFixtureTearDown]
        public void TestCleanUp()
        {
            if (Directory.Exists(rootDirectory))
            {
                Directory.Delete(rootDirectory, true);
            }
        }

        [Test]
        public void Serializez_Branch_Name_And_Description_Can_Be_Read()
        {
            // arrange
            var branch = new Branch
                             {
                                 Name = BranchName,
                                 Description = "just a simple development Branch, might as well be the trunk."
                             };

            // act
            writer.SaveBranchDescription(branch);
            writer.Flush();            

            // assert
            var branchFromFile = reader.LoadBranch(BranchName);
            Assert.AreEqual(BranchName, branchFromFile.Name);
            Assert.AreEqual(branch.Description, branchFromFile.Description);
        }

        [Test]
        public void Serialized_Build_Can_Be_Read()
        {
            // arrange
            var build = new Build { Name = BuildName, Date = DateTime.Today, Revision = "1337", Status = "success" };

            build.AddDetail(DetailsVersionKey, "1.0.1");

            // act
            writer.SaveBuildDescription(build);
            writer.Flush();

            // arrange
            Assert.IsTrue(File.Exists(docuFiles.GetBuildFile(BranchName, BuildName)));

            // a build can't be read, because there is no implementation to read the details.
            // i'd propose to change xml to json since it's more tolerant. just look
            // at Details.cs:#WriteXml if you don't understand this comment.
            ////var result = reader.LoadBuild(BranchName, BuildName);

            ////Assert.That(BuildName, Is.EqualTo(result.Name));
            ////Assert.That("1337", Is.EqualTo(result.Revision));
            ////Assert.That("success", Is.EqualTo("success"));
        }

        [Test]
        public void Serialized_Usecase_Can_Be_Read()
        {
            // arrange
            var usecase = new UseCase
                              {
                                  Name = TestContext.CurrentContext.Test.Name,
                                  Description = "this is a typical use case with a decription",
                                  Status = "success",
                              };

            usecase.AddDetail("webtestName", "UseCaseWebTest");

            // act
            writer.SaveUseCase(usecase);
            writer.Flush();

            // assert
            Assert.IsTrue(File.Exists(docuFiles.GetUseCaseFile(BranchName, BuildName, SerializationUseCase)));

            var usecaseXml = File.ReadAllText(docuFiles.GetUseCaseFile(BranchName, BuildName, SerializationUseCase));
            StringAssert.Contains("UseCaseWebTest", usecaseXml);
            StringAssert.Contains(usecase.Name, usecaseXml);
            StringAssert.Contains(usecase.Description, usecaseXml);
            StringAssert.Contains(usecase.Status, usecaseXml);
        }

        [Test]
        public void Serialize_Scenario_Name_Description_And_Status()
        {
            // arrange
            var scenario = new Scenario
                               {
                                   Name = TestContext.CurrentContext.Test.Name,
                                   Description = "this is a typical scenario with a decription",
                                   Status = "success",
                               };

            // act
            scenario.AddDetail("userRole", "customer");
            writer.SaveScenario(SerializationUseCase, scenario);
            writer.Flush();

            // assert
            var scenarioXml = File.ReadAllText(docuFiles.GetScenarioFile(BranchName, BuildName, SerializationUseCase, TestContext.CurrentContext.Test.Name));
            
            StringAssert.Contains(string.Format("<name>{0}</name>"), scenario.Name, scenarioXml);
            StringAssert.Contains(string.Format("<name>{0}</name>"), scenario.Description, scenarioXml);
            StringAssert.Contains("<status>success</status>", scenarioXml);
        }

        [Test]
        public void Serialize_A_Step()
        {
            // arrange
            var step = new Step();
            var stepDescription = new StepDescription { Index = StepIndex, Title = "Test Step", Status = "success" };
            step.StepDescription = stepDescription;

            step.StepHtml = new StepHtml { HtmlSource = "<html>just some page text</html>" };
            step.Page = new Page { Name = "customer/overview.jsp" };

            step.StepMetadata = new StepMetadata
                                   {
                                       VisibleText = "just some page text",
                                   };

            // act
            writer.SaveStep(SerializationUseCase, TestContext.CurrentContext.Test.Name, step);
            writer.Flush();

            // assert
            Assert.IsTrue(File.Exists(docuFiles.GetScenarioStepFile(BranchName, BuildName, SerializationUseCase, TestContext.CurrentContext.Test.Name, 1)));
        }

        [Test]
        public void Write_Steps_With_Screen_Annotations()
        {
            // arrange
            var usecase = new UseCase
            {
                Name = "Screen Annotations",
                Description = "test screen annotations",
                Status = "failed",
            };

            writer.SaveUseCase(usecase);

            var scenario = new Scenario
            {
                Name = "All screen annotations on one page",
                Description = "this is a typical scenario with a decription",
                Status = "failed",
            };

            writer.SaveScenario(usecase.Name, scenario);
            writer.Flush();
            
            // act
            var step = new Step();
            var stepDescription = new StepDescription { Index = StepIndex, Title = "Test Step", Status = "success" };
            step.StepDescription = stepDescription;

            step.StepHtml = new StepHtml { HtmlSource = "<html>just some page text</html>" };
            step.Page = new Page { Name = "Kawasaki Ninja" };
            step.StepDescription.ScreenshotFileName = "000.png";
            step.StepDescription.Index = 0;

            step.ScreenAnnotations.Add(DataGenerator.CreateScreenAnnotation(10, 50, ScreenAnnotationStyle.Highlight));
            step.ScreenAnnotations.Add(DataGenerator.CreateScreenAnnotation(10, 150, ScreenAnnotationStyle.Click, ScreenAnnotationClickAction.ToUrl, "next-url"));
            step.ScreenAnnotations.Add(DataGenerator.CreateScreenAnnotation(10, 250, ScreenAnnotationStyle.Error));
            step.ScreenAnnotations.Add(DataGenerator.CreateScreenAnnotation(10, 350, ScreenAnnotationStyle.Expected));
            step.ScreenAnnotations.Add(DataGenerator.CreateScreenAnnotation(10, 450, ScreenAnnotationStyle.Info));
            step.ScreenAnnotations.Add(DataGenerator.CreateScreenAnnotation(400, 50, ScreenAnnotationStyle.Keyboard));
            step.ScreenAnnotations.Add(DataGenerator.CreateScreenAnnotation(400, 150, ScreenAnnotationStyle.Warn));
            step.ScreenAnnotations.Add(DataGenerator.CreateScreenAnnotation(400, 250, ScreenAnnotationStyle.Default));
            step.ScreenAnnotations.Add(DataGenerator.CreateScreenAnnotation(400, 350, ScreenAnnotationStyle.NavigateToUrl, ScreenAnnotationClickAction.ToUrl, "blabla"));

            writer.SaveStep(usecase.Name, scenario.Name, step);
            writer.Flush();
            
            writer.SaveScreenshot(usecase.Name, scenario.Name, step, File.ReadAllBytes("data/screenshot.png"));
            writer.Flush();

            usecase.Status = "success";
            scenario.Status = "success";

            writer.SaveUseCase(usecase);
            writer.SaveScenario(usecase.Name, scenario);

            var stepAsString = File.ReadAllText(docuFiles.GetScenarioStepFile(BranchName, BuildName, usecase.Name, scenario.Name, 0));

            // well this is a really sloppy assertion. BUT: we don't have to test the whole serialization because this is done by .NET framework.
            // we have to do some little things that the java deserializer likes our xml. So we do just some checks on "problem zones". the whole
            // import should still be checked with an api call on the scenarioo backend.
            StringAssert.Contains("<style>highlight</style>", stepAsString);
            StringAssert.Contains("<style>click</style>", stepAsString);
            StringAssert.Contains("<style>error</style>", stepAsString);
            StringAssert.Contains("<style>info</style>", stepAsString);
            StringAssert.Contains("<style>expected</style>", stepAsString);
        }

        [Test]
        public void Serialized_Details_Object_With_ObjectList_Exists()
        {
            // arrange
            var scenario = new Scenario
                               {
                                   Name = TestContext.CurrentContext.Test.Name,
                                   Description = "a scenario for testing collections in details"
                               };

            // act
            var detailsMap = new Details();
            detailsMap.AddDetail("anyGenericObjectReference", new ObjectReference("serviceCall", "MainDB.getUsers"));
            detailsMap.AddDetail(
                "anyGenericObject",
                new ObjectDescription("configuration", "my_dummy_mocks_configuration.properties"));
            detailsMap.AddDetail("key1", "value1");
            detailsMap.AddDetail("key2", "value2");

            scenario.AddDetail("map", detailsMap);

            var objList = new ObjectList<string> { "item1", "item2", "item3" };
            scenario.Details.AddDetail("list", objList);
            writer.SaveScenario(SerializationUseCase, scenario);
            writer.Flush();

            // assert (there is no way to deserialize the details atm :()
            Assert.IsTrue(File.Exists(docuFiles.GetScenarioFile(BranchName, BuildName, SerializationUseCase, TestContext.CurrentContext.Test.Name)));
        }

        [Test]
        public void Serilazed_Details_Object_With_ObjectTreeNode_Exists()
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
            writer.SaveScenario(SerializationUseCase, scenario);

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            writer.Flush();

            // THEN: the scneario file exists
            Assert.IsTrue(File.Exists(docuFiles.GetScenarioFile(BranchName, BuildName, SerializationUseCase, ScenarioName)));
        }

        [Test] // TODO: review this async stuff, i'm not sure about that!
        public void AsyncWriteOfMultipleFilesAndFlush()
        {
            // GIVEN: a lot of large steps to write, that have not yet been written 
            var steps = new Step[10];
            for (var index = 0; index < 10; index++)
            {
                steps[index] = CreateBigDataStepForLoadTestAsyncWriting(index + 1);
            }

            var expectedFileForSteps = docuFiles.GetScenarioStepFile(
                BranchName,
                BuildName,
                SerializationUseCase,
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
                writer.SaveStep(SerializationUseCase, ScenarioName, step);
            }

            // THEN: the files are not created directly but asynchronously. Flush will wait until all Tasks are finished.
            writer.Flush();
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
                                              docuFiles.GetScreenshotFile(
                                                  BranchName,
                                                  BuildName,
                                                  SerializationUseCase,
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
    }
}
