using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
        private const string BranchId = "exmaple-branch";
        private const string BranchName = "example branch";

        private const string UseCaseId = "example use case";

        private const string BuildId = "example-build";

        private string _rootDirectory;

        private ScenarioDocuWriter _writer;

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
        }

        public void TestCleanUp()
        {
            //if (Directory.Exists(_rootDirectory))
            //{
            //    Directory.Delete(_rootDirectory, true);
            //}
        }

        [Test]
        public void Write_Branch()
        {
            var branch = new Branch(BranchName);
            branch.Id = BranchId; // TODO implement automatic conversion of special chars
            
            branch.Description = "an optional description text";
            
            branch.Properties.Add(CreateSimpleProperty());
            branch.Properties.Add(CreateComplexProperty());

            _writer.SaveBranchDescription(branch);
        }

        [Test]
        public void Write_Build()
        {
            var build = new Build("example build");
            build.Id = BuildId; // TODO implement auto id generation if not set
            build.Description = "an optional description text";
            build.Revision = "10.1#5ba891b";
            build.Date = new DateTime(2016, 05, 01, 20, 04, 13);
            build.Status = "success";
            build.Properties.Add(CreateSimpleProperty());
            build.Properties.Add(CreateComplexProperty());

            _writer.SaveBuildDescription(build);
        }

        [Test]
        public void Write_UseCase()
        {
            var usecase = new UseCase("example use case");
            usecase.Id = UseCaseId; // TODO: id generation
            usecase.Labels.Add("example-label-1");
            usecase.Labels.Add("example-label-2");
            usecase.Status = "success";
            usecase.Properties.Add(CreateSimpleProperty());
            usecase.Properties.Add(CreateComplexProperty());

            _writer.SaveUseCase(usecase);
        }

        [Test]
        public void Write_Example_Scenario()
        {
            var scenario = new Scenario("example scenario", string.Empty);
            scenario.Id = "example-scenario";
            scenario.Description = "an optional description";
            scenario.Labels.Add("example-label-1");
            scenario.Labels.Add("example-label-2");
            scenario.Status = "success";
            scenario.Properties.Add(CreateSimpleProperty());
            scenario.Properties.Add(CreateComplexProperty());

            _writer.SaveScenario(UseCaseId, scenario);

            var step = new Step();
            step.Index = 0;
            step.Title = "My Step Title";
            step.Status = "success";
            step.Page = new Page("example/page.htlm");
            step.Page.Id = "example-page-html"; // TODO id generation
            step.Page.Properties = new List<DocuObject>();
            step.Page.Properties.Add(CreateSimpleProperty());
            step.Page.Properties.Add(CreateComplexProperty());
            step.Page.Labels = new Labels("example-label-1");
            step.Page.VisibleText = "just some dummy html code";
            step.Page.ScreenAnnotations = new List<ScreenAnnotation>(CreateScreenAnnotations());
            step.Labels = new Labels("example-label-1", "example-label-2");
            step.Properties = new List<DocuObject>() { CreateSimpleProperty() };
            step.PropertyGroups = new PropertyGroups();

            var docuObject = new DocuObject();
            docuObject.Properties = new List<DocuObject>() { CreateSimpleProperty(), CreateComplexProperty() };

            step.PropertyGroups.Add("Property Group 1", CreateSimpleProperty(), CreateComplexProperty());

            step.PropertyGroups["Property Group 1"].Type = "gaggi";

            step.StepHtml = "<html><head></head><body><p>just some dummy html code</p></body></html>";

            _writer.SaveStep(UseCaseId, scenario.Id, step);

            _writer.SaveScreenshot(UseCaseId, scenario.Id, step, File.ReadAllBytes("data/screenshot.png"));
        }

        [Test]
        public void Ridiculous_Performance_Test()
        {
            var group = new PropertyGroups();

            for (int i = 0; i < 1000; ++i)
            {
                group.Add("shit + " + i, new DocuObject());
            }

            var watch = Stopwatch.StartNew();
            var bla = group["shit + 900"];
            watch.Stop();
            bla.Type = "1337";
            
            Console.WriteLine("Took {0} ms", watch.ElapsedMilliseconds);
            Console.WriteLine("Took {0} ticks ({1})", watch.ElapsedTicks, TimeSpan.TicksPerMillisecond);
        }

        private static DocuObject CreateSimpleProperty()
        {
            return new DocuObject("some value", labelKey: "simpleProperty");
        }

        private static DocuObject CreateComplexProperty()
        {
            var complexObject = new DocuObject("Complex Object", "complex-object-1", "complexObjectProperty", "ComplexObject");
            complexObject.Properties.Add(new DocuObject("any value", "objectProperty"));
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
                           Properties = new List<DocuObject>()
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