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
    /// <summary>
    /// The minimalistic sample tries to show you which fields are mandatory. That's the absolute
    /// minimum that is required for the Scenarioo UI to properly show your stuff.
    /// </summary>
    [TestFixture]
    public class SampleMinimalistic
    {
        private const string BranchId = "example-branch-minimal";
        private const string BuildId = "example-build-minimal";
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
            var branch = new Branch("example branch minimal");

            // act
            _writer.SaveBranchDescription(branch);

            // assert
            var expected = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/example/example-branch-minimal/branch.json".GetStringFromUrl());
            var actual = JToken.Parse(File.ReadAllText(_docuFiles.GetBranchFile(branch.Id)));

            Console.WriteLine("Actual: \n{0}\n\n", actual);
            Console.WriteLine("Expected: \n{0}", expected);

            Assert.IsTrue(JToken.DeepEquals(expected, actual));
        }

        [Test]
        public void Write_Build()
        {
            // arrange
            var build = new Build("example build minimal");

            // act
            _writer.SaveBuildDescription(build);

            // assert
            var expected = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/example/example-branch-minimal/example-build-minimal/build.json".GetStringFromUrl());
            var actual = JToken.Parse(File.ReadAllText(_docuFiles.GetBuildFile(BranchId, build.Id)));

            Console.WriteLine("Actual: \n{0}\n\n", actual);
            Console.WriteLine("Expected: \n{0}", expected);

            Assert.IsTrue(JToken.DeepEquals(expected, actual));
        }

        [Test]
        public void Write_UseCase()
        {
            // arrange
            var usecase = new UseCase("example use case minimal");

            // act
            _writer.SaveUseCase(usecase);

            // assert
            var expected = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/example/example-branch-minimal/example-build-minimal/example-use-case-minimal/usecase.json".GetStringFromUrl());
            var actual = JToken.Parse(File.ReadAllText(_docuFiles.GetUseCaseFile(BranchId, BuildId, usecase.Id)));

            Console.WriteLine("Actual: \n{0}\n\n", actual);
            Console.WriteLine("Expected: \n{0}", expected);

            Assert.IsTrue(JToken.DeepEquals(expected, actual));
        }

        ////[Test]
        ////public void Write_Example_Scenario()
        ////{
        ////    var scenario = new Scenario("example scenario", string.Empty);

        ////    scenario.Description = "an optional description";
        ////    scenario.Labels.Add("example-label-1");
        ////    scenario.Labels.Add("example-label-2");
        ////    scenario.Status = "success";
        ////    scenario.Properties.Add(CreateSimpleProperty());
        ////    scenario.Properties.Add(CreateComplexProperty());

        ////    _writer.SaveScenario(UseCaseId, scenario);

        ////    var step = new Step();
        ////    step.Index = 0;
        ////    step.Title = "My Step Title";
        ////    step.Status = "success";
        ////    step.Page = new Page("example/page.html");
        ////    step.Page.Properties.Add(CreateSimpleProperty());
        ////    step.Page.Properties.Add(CreateComplexProperty());
        ////    step.Page.Labels.Add("example-label-1");
        ////    step.Page.VisibleText = "just some dummy html code";
        ////    step.Page.ScreenAnnotations.AddRange(CreateScreenAnnotations());
        ////    step.Labels.Add("example-label-1");
        ////    step.Labels.Add("example-label-2");
        ////    step.Properties.Add(CreateSimpleProperty());

        ////    step.Sections.Add("Property Group 1", CreateSimpleProperty(), CreateComplexProperty());

        ////    step.StepHtml = "<html><head></head><body><p>just some dummy html code</p></body></html>";

        ////    _writer.SaveStep(UseCaseId, scenario.Id, step);
        ////    _writer.SaveScreenshot(UseCaseId, scenario.Id, step, File.ReadAllBytes("data/screenshot.png"));

        ////    var expected = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/example/example-branch/example-build/example-use-case/example-scenario/scenario.json".GetStringFromUrl());
        ////    var actual = JToken.Parse(File.ReadAllText(_docuFiles.GetScenarioFile(BranchId, BuildId, UseCaseId, scenario.Id)));

        ////    Console.WriteLine("Actual: \n{0}\n\n", actual);
        ////    Console.WriteLine("Expected: \n{0}", expected);

        ////    Assert.IsTrue(JToken.DeepEquals(expected, actual));
        ////}

        ////[Test]
        ////public void Write_Example_Scenario_With_ManualId()
        ////{
        ////    var scenario = new Scenario("example scenario with a manually set ID not generated from name");
        ////    scenario.Status = "success";
        ////    scenario.Id = "example-scenario-with-manual-id";

        ////    _writer.SaveScenario(UseCaseId, scenario);

        ////    var expected = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/example/example-branch/example-build/example-use-case/example-scenario-with-manual-id/scenario.json".GetStringFromUrl());
        ////    var actual = JToken.Parse(File.ReadAllText(_docuFiles.GetScenarioFile(BranchId, BuildId, UseCaseId, scenario.Id)));

        ////    Console.WriteLine("Actual: \n{0}\n\n", actual);
        ////    Console.WriteLine("Expected: \n{0}", expected);

        ////    Assert.IsTrue(JToken.DeepEquals(expected, actual));
        ////}
    }
}