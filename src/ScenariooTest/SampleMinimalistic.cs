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

        [Test]
        public void Write_Example_Scenario_And_Step()
        {
            // arrange
            var scenario = new Scenario("example scenario minimal");
            scenario.Status = "success";

            var step = new Step { Index = 0 };

            // act
            _writer.SaveScenario(UseCaseId, scenario);
            _writer.SaveStep(UseCaseId, scenario.Id, step);
            _writer.SaveScreenshot(UseCaseId, scenario.Id, step, File.ReadAllBytes("data/screenshot.png"));

            // assert
            var expected = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/example/example-branch-minimal/example-build-minimal/example-use-case-minimal/example-scenario-minimal/scenario.json".GetStringFromUrl());
            var actual = JToken.Parse(File.ReadAllText(_docuFiles.GetScenarioFile(BranchId, BuildId, UseCaseId, scenario.Id)));

            var expectedStep = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/example/example-branch-minimal/example-build-minimal/example-use-case-minimal/example-scenario-minimal/steps/000.json".GetStringFromUrl());
            var actualStep = JToken.Parse(File.ReadAllText(_docuFiles.GetScenarioStepFile(BranchId, BuildId, UseCaseId, scenario.Id, step.Index)));

            Console.WriteLine("Actual: \n{0}\n\n", actual);
            Console.WriteLine("Expected: \n{0}", expected);

            Console.WriteLine("Actual: \n{0}\n\n", actualStep);
            Console.WriteLine("Expected: \n{0}", expectedStep);

            Assert.IsTrue(JToken.DeepEquals(expected, actual), "Scenario");
            Assert.IsTrue(JToken.DeepEquals(expectedStep, actualStep), "Step");
        }

        [Test]
        public void Write_Example_Scenario_Failed()
        {
            // arrange
            var scenario = new Scenario("example scenario failed");
            scenario.Status = "failed";

            // act
            _writer.SaveScenario(UseCaseId, scenario);

            var expected = JToken.Parse("https://raw.githubusercontent.com/scenarioo/scenarioo-format/master/example/example-branch-minimal/example-build-minimal/example-use-case-minimal/example-scenario-failed/scenario.json".GetStringFromUrl());
            var actual = JToken.Parse(File.ReadAllText(_docuFiles.GetScenarioFile(BranchId, BuildId, UseCaseId, scenario.Id)));

            Console.WriteLine("Actual: \n{0}\n\n", actual);
            Console.WriteLine("Expected: \n{0}", expected);

            Assert.IsTrue(JToken.DeepEquals(expected, actual));
        }
    }
}