using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

using Scenarioo.Api;
using Scenarioo.Api.Files;
using Scenarioo.Model.Docu.Entities;
using Scenarioo.Model.Docu.Entities.Generic;

namespace ScenariooTest
{
    [TestFixture]
    public class Sample
    {
        private const string BranchId = "example-branch";
        private const string BuildId = "example-build";
        private const string UseCaseId = "example-use-case";

        private string _rootDirectory;

        private ScenarioDocuWriter _writer;
        private ScenarioDocuFiles _docuFiles;

        [TestFixtureSetUp]
        public void TestInit()
        {
            // Sets outcome directory
            _rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "sample");
            TestCleanUp();

            if (!Directory.Exists(_rootDirectory))
            {
                Directory.CreateDirectory(_rootDirectory);
            }

            _writer = new ScenarioDocuWriter(
                _rootDirectory,
                BranchId,
                BuildId);

            _docuFiles = new ScenarioDocuFiles(_rootDirectory);
        }

        public void TestCleanUp()
        {
            if (Directory.Exists(_rootDirectory))
            {
                Directory.Delete(_rootDirectory, true);
            }
        }

        [Test]
        public void Write_Branch()
        {
            // arrange
            var branch = new Branch("example branch");
            
            branch.Description = "an optional description text";
            
            branch.Properties.Add(CreateSimpleProperty());
            branch.Properties.Add(CreateComplexProperty());

            // act
            _writer.SaveBranchDescription(branch);

            // assert
            var expected = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/reference-example/example-branch/branch.json".GetStringFromUrl());
            var actual = JToken.Parse(File.ReadAllText(_docuFiles.GetBranchFile(branch.Id)));

            Console.WriteLine("Actual: \n{0}\n\n", actual);
            Console.WriteLine("Expected: \n{0}", expected);

            Assert.IsTrue(JToken.DeepEquals(expected, actual));
        }

        [Test]
        public void Write_Build()
        {
            // arrange
            var build = new Build("example build");
            build.Description = "an optional description text";
            build.Revision = "1.0.1#5ba891b";
            build.Date = new DateTime(2016, 05, 01, 20, 04, 13);
            build.Status = "success";
            build.Properties.Add(CreateSimpleProperty());
            build.Properties.Add(CreateComplexProperty());

            // act
            _writer.SaveBuildDescription(build);

            // assert
            var expected = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/reference-example/example-branch/example-build/build.json".GetStringFromUrl());
            var actual = JToken.Parse(File.ReadAllText(_docuFiles.GetBuildFile(BranchId, build.Id)));

            Console.WriteLine("Actual: \n{0}\n\n", actual);
            Console.WriteLine("Expected: \n{0}", expected);

            Assert.IsTrue(JToken.DeepEquals(expected, actual));
        }

        [Test]
        public void Write_UseCase()
        {
            // arrange
            var usecase = new UseCase("example use case");
            usecase.Description = "an optional description text";
            usecase.Labels.Add("example-label-1");
            usecase.Labels.Add("example-label-2");
            usecase.Status = "success";
            usecase.Properties.Add(CreateSimpleProperty());
            usecase.Properties.Add(CreateComplexProperty());

            usecase.Sections.Add(new DocuObject(labelKey: "More Information", value: "#Simple Text Section\nAllowed to have multiline content of course.\nBut we need to take care to visualize this nicely."));

            var property2 = new DocuObject(labelKey: "property2-with-list-value");
            property2.Items.Add(new DocuObject(value: "item1"));
            property2.Items.Add(new DocuObject(value: "item2"));

            usecase.Sections.Add("Another Section with more Properties", new DocuObject(labelKey: "property1", value: " a simple value"), property2);

            // act
            _writer.SaveUseCase(usecase);

            // assert
            var expected = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/reference-example/example-branch/example-build/example-use-case/usecase.json".GetStringFromUrl());
            var actual = JToken.Parse(File.ReadAllText(_docuFiles.GetUseCaseFile(BranchId, BuildId, usecase.Id)));

            Console.WriteLine("Actual: \n{0}\n\n", actual);
            Console.WriteLine("Expected: \n{0}", expected);

            Assert.IsTrue(JToken.DeepEquals(expected, actual));
        }

        [Test]
        public void Write_Example_Scenario_And_Step()
        {
            // arrange
            var scenario = new Scenario("example scenario", string.Empty);
            
            scenario.Description = "an optional description";
            scenario.Labels.Add("example-label-1");
            scenario.Labels.Add("example-label-2");
            scenario.Status = "success";
            scenario.Properties.Add(CreateSimpleProperty());
            scenario.Properties.Add(CreateComplexProperty());

            scenario.Sections.Add(new DocuObject(labelKey: "More Information", value: "#Simple Text Section with Markdown\n\nAllowed to have multiline content of course.\n\nBut we need to take care to visualize this nicely."));

            var property2 = new DocuObject(labelKey: "property2-with-list-value");
            property2.Items.Add(new DocuObject(value: "item1"));
            property2.Items.Add(new DocuObject(value: "item2"));

            scenario.Sections.Add("Another Section with more Properties", new DocuObject(labelKey: "property1", value: " a simple value"), property2);

            var step = new Step();
            step.Index = 0;
            step.Title = "My Step Title";
            step.Status = "success";
            step.Page = new Page("example/page.html");
            step.Page.Properties.Add(CreateSimpleProperty());
            step.Page.Properties.Add(CreateComplexProperty());
            step.Page.Labels.Add("example-label-1");
            step.Page.VisibleText = "just some dummy html code";
            step.Page.ScreenAnnotations.AddRange(CreateScreenAnnotations());
            step.Labels.Add("example-label-1");
            step.Labels.Add("example-label-2");
            step.Properties.Add(CreateSimpleProperty());
            step.Sections.Add("Property Group 1", CreateSimpleProperty(), CreateComplexProperty());
            step.StepHtml = "<html><head></head><body><p>just some dummy html code</p></body></html>";

            // act
            _writer.SaveScenario(UseCaseId, scenario);
            _writer.SaveStep(UseCaseId, scenario.Id, step);
            _writer.SaveScreenshot(UseCaseId, scenario.Id, step, File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "data/screenshot.png")));

            // assert
            var expected = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/reference-example/example-branch/example-build/example-use-case/example-scenario/scenario.json".GetStringFromUrl());
            var actual = JToken.Parse(File.ReadAllText(_docuFiles.GetScenarioFile(BranchId, BuildId, UseCaseId, scenario.Id)));

            Console.WriteLine("Actual: \n{0}\n\n", actual);
            Console.WriteLine("Expected: \n{0}", expected);

            Assert.IsTrue(JToken.DeepEquals(expected, actual));
        }

        [Test]
        public void Write_Example_Scenario_With_ManualId()
        {
            // arrange
            var scenario = new Scenario("example scenario with a manually set ID not generated from name");
            scenario.Status = "success";
            scenario.Id = "example-scenario-with-manual-id";

            // act
            _writer.SaveScenario(UseCaseId, scenario);

            // assert
            var expected = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/reference-example/example-branch/example-build/example-use-case/example-scenario-with-manual-id/scenario.json".GetStringFromUrl());
            var actual = JToken.Parse(File.ReadAllText(_docuFiles.GetScenarioFile(BranchId, BuildId, UseCaseId, scenario.Id)));

            Console.WriteLine("Actual: \n{0}\n\n", actual);
            Console.WriteLine("Expected: \n{0}", expected);

            Assert.IsTrue(JToken.DeepEquals(expected, actual));
        }

        private static DocuObject CreateSimpleProperty()
        {
            return new DocuObject("some value", labelKey: "simpleProperty");
        }

        private static DocuObject CreateComplexProperty()
        {
            var complexObject = new DocuObject("Complex Object 1", "Complex-Object-1", "complexObjectProperty", "ComplexObject");
            complexObject.Properties.Add(new DocuObject("any value", labelKey: "objectProperty"));
            complexObject.Items.Add(new DocuObject("Sub Object A", type: "ComplexObject"));
            complexObject.Items.Add(new DocuObject("Sub Object B", type: "ComplexObject"));

            return complexObject;
        }

        private static IEnumerable<ScreenAnnotation> CreateScreenAnnotations()
        {
            return new List<ScreenAnnotation>()
                   {
                       new ScreenAnnotation(new ScreenRegion(0, 0, 200, 100))
                       {
                           ClickAction = ScreenAnnotationClickAction.ToNextStep,
                           Style = ScreenAnnotationStyle.Click,
                           Properties = new DocuObjectMap()
                                        {
                                            CreateSimpleProperty(),
                                            CreateComplexProperty()
                                        }
                       },
                       new ScreenAnnotation(new ScreenRegion(0, 0, 200, 100))
                       {
                           ClickAction = ScreenAnnotationClickAction.ToUrl,
                           Style = ScreenAnnotationStyle.Click,
                           ClickActionUrl = "http://blabla.com/blubbi",
                       }
                   };
        }
    }
}