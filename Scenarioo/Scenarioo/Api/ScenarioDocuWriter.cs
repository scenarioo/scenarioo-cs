/* Scenarioo-api
 * Copyright (C) 2014, Scenarioo.org Development Team
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

using System.IO;
using System.Threading;

using Scenarioo.Annotations;
using Scenarioo.Api.Files;
using Scenarioo.Api.Util.Image;
using Scenarioo.Api.Util.Xml;
using Scenarioo.Model.Docu.Entities;

namespace Scenarioo.Api
{
    /// <summary>
    ///  Generator to produce documentation files for a specific build.
    /// The writer performs all save operations as asynchronous writes, to not block the web tests that are typically calling
    /// the save operations to save documentation content.
    /// </summary>
    public class ScenarioDocuWriter
    {
        private readonly string branchName;

        private readonly string buildName;

        public ScenarioDocuWriter(
            string destinationRootDirectory,
            string branchName,
            string buildName)
        {
            this.DocuFiles = new ScenarioDocuFiles(destinationRootDirectory);
            this.branchName = branchName;
            this.buildName = buildName;
            this.DestinationRootDirectory = destinationRootDirectory;

            this.CreateBuildDirectoryIfNotYetExists();
        }

        public ScenarioDocuFiles DocuFiles { get; set; }

        private string DestinationRootDirectory { [UsedImplicitly] get; set; }

        private static void ExecuteAsyncXmlWriter<T>(T entity, string destFile) where T : class
        {
            ScenarioDocuXMLFileUtil.MarshalXml(entity, destFile);
        }

        private static void ExecuteAsyncImageWriter(string fileName, byte[] file)
        {
            ScenarioDocuImageFileUtil.MarshalImage(fileName, file);
        }

        private void CreateBuildDirectoryIfNotYetExists()
        {
            this.CreateDirectoryIfNotYetExists(this.GetBuildDirectory());
        }

        private void CreateBranchDirectoryIfNotYetExists()
        {
            this.CreateDirectoryIfNotYetExists(this.GetBranchDirectory());
        }

        private void CreateUseCaseDirectoryIfNotYetExists(UseCase useCase)
        {
            this.CreateDirectoryIfNotYetExists(this.GetUseCaseDirectory(useCase));
        }

        private void CreateScenarioDirectoryIfNotYetExists(string useCaseName, Scenario scenario)
        {
            this.CreateDirectoryIfNotYetExists(this.GetScenarioDirectory(useCaseName, scenario));
        }

        private void CreateStepDirectoryIfNotYetExists(string useCaseName, string scenarioName)
        {
            this.CreateDirectoryIfNotYetExists(this.GetStepsDirectory(useCaseName, scenarioName));
        }

        private void CreateScreenshotDirectoryIfNotYetExists(string useCaseName, string scenarioName)
        {
            this.CreateDirectoryIfNotYetExists(this.GetScreenshotDirectory(useCaseName, scenarioName));
        }

        private void CreateDirectoryIfNotYetExists(string directory)
        {
            this.DocuFiles.AssertRootDirectoryExists();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private string GetBuildDirectory()
        {
            return this.DocuFiles.GetBuildDirectory(this.branchName, this.buildName);
        }

        private string GetBranchDirectory()
        {
            return this.DocuFiles.GetBranchDirectory(this.branchName);
        }

        private string GetUseCaseDirectory(UseCase useCase)
        {
            return this.DocuFiles.GetUseCaseDirectory(this.branchName, this.buildName, useCase.Name);
        }

        private string GetStepsDirectory(string useCaseName, string scenarioName)
        {
            return this.DocuFiles.GetScenarioStepDirectory(
                this.branchName,
                this.buildName,
                useCaseName,
                scenarioName);
        }

        private string GetScenarioDirectory(string useCaseName, Scenario scenario)
        {
            return this.DocuFiles.GetScenarioDirectory(
                this.branchName,
                this.buildName,
                useCaseName,
                scenario.Name);
        }

        /// <summary>
        /// In case you want to define your screenshot names differently than by step name, you can save it on your own, into
        /// the following directory for a scenario.
        /// </summary>
        /// <param name="useCaseName">
        /// The use case name from which screenshot should be retrieved
        /// </param>
        /// <param name="scenarioName">
        /// The scenario name from which screenshot should be retrieved
        /// </param>
        /// <returns>
        /// Screenshot directory
        /// </returns>
        private string GetScreenshotDirectory(string useCaseName, string scenarioName)
        {
            return this.DocuFiles.GetScreenshotDirectory(
                this.branchName, 
                this.buildName, 
                useCaseName, 
                scenarioName);
        }

        /// <summary>
        /// Save the build description to appropriate directory.
        /// </summary>
        /// <param name="build">The build description to write</param>
        public void SaveBuildDescription(Build build)
        {
            var destBuildFile = this.DocuFiles.GetBuildFile(this.branchName, this.buildName);
            this.CreateBuildDirectoryIfNotYetExists();
            ExecuteAsyncXmlWriter(build, destBuildFile);
        }

        /// <summary>
        /// Save the branch description to appropriate directory.
        /// </summary>
        /// <param name="branch">The branch description to write.</param>
        public void SaveBranchDescription(Branch branch)
        {
            var destBranchFile = this.DocuFiles.GetBranchFile(this.branchName);
            this.CreateBranchDirectoryIfNotYetExists();
            ExecuteAsyncXmlWriter(branch, destBranchFile);
        }

        /// <summary>
        /// Save the use case description to appropriate directory and file.
        /// </summary>
        /// <param name="useCase">The use case description to write</param>
        public void SaveUseCase(UseCase useCase)
        {
            var desUseCaseFile = this.DocuFiles.GetUseCaseFile(this.branchName, this.buildName, useCase.Name);
            this.CreateUseCaseDirectoryIfNotYetExists(useCase);
            ExecuteAsyncXmlWriter(useCase, desUseCaseFile);
        }

        public void SaveScenario(string useCaseName, Scenario scenario)
        {
            var desScenarioFile = this.DocuFiles.GetScenarioFile(
                this.branchName,
                this.buildName,
                useCaseName,
                scenario.Name);
            this.CreateScenarioDirectoryIfNotYetExists(useCaseName, scenario);
            ExecuteAsyncXmlWriter(scenario, desScenarioFile);
        }

        public void SaveStep(string useCaseName, string scenarioName, Step step)
        {
            var desScenarioStepFile = this.DocuFiles.GetScenarioStepFile(
                this.branchName,
                this.buildName,
                useCaseName,
                scenarioName,
                step.StepDescription.Index);
            this.CreateStepDirectoryIfNotYetExists(useCaseName, scenarioName);
            ExecuteAsyncXmlWriter(step, desScenarioStepFile);
        }

        /// <summary>
        /// Save Screenshot as a PNG file in usual file for step.
        /// </summary>
        /// <param name="usecaseName">The use case name</param>
        /// <param name="scenarioName">The scenario name</param>
        /// <param name="step">The step</param>
        /// <param name="file">Bytes of screenshot</param>
        public void SaveScreenshot(string usecaseName, string scenarioName, Step step, byte[] file)
        {
            var desScreenshotFile = this.DocuFiles.GetScreenshotFile(
                this.branchName, 
                this.buildName, 
                usecaseName, 
                scenarioName, 
                step.StepDescription.Index); // TODO: filename!
            
            this.CreateScreenshotDirectoryIfNotYetExists(usecaseName, scenarioName);
            ExecuteAsyncImageWriter(desScreenshotFile, file);
        }

        /// <summary>
        /// Finish asynchronous writing of all saved files. This has to be called in the end, to ensure all data saved in
        /// this generator is written to the file system.
        /// Will block until writing has finished.
        /// </summary>
        public void Flush()
        {
            while (ScenarioDocuXMLFileUtil.RunningTasks.Count > 0)
            {
                ScenarioDocuXMLFileUtil.RemoveFinishedTasks();

                Thread.Sleep(100);
            }
        }
    }
}
