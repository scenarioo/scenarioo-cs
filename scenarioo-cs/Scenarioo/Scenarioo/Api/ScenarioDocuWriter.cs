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

using System.Threading;

namespace Scenarioo.Api
{
    using System.IO;

    using Scenarioo.Api.Files;
    using Scenarioo.Api.Util.Files;
    using Scenarioo.Api.Util.Xml;
    using Scenarioo.Model.Docu.Entities;

    public class ScenarioDocuWriter
    {
        public ScenarioDocuFiles DocuFiles { get; set; }

        private readonly string _branchName;

        private readonly string _buildName;

        private string DestinationRootDirectory { get; set; }

        public ScenarioDocuWriter(
            string destinationRootDirectory,
            string branchName,
            string buildName)
        {
            this.DocuFiles = new ScenarioDocuFiles(destinationRootDirectory);
            this._branchName = branchName;
            this._buildName = buildName;
            this.DestinationRootDirectory = destinationRootDirectory;

            CreateBuildDirectoryIfNotYetExists();
        }

        private void CreateBuildDirectoryIfNotYetExists()
        {
            CreateDirectoryIfNotYetExists(this.GetBuildDirectory());
        }

        private void CreateBranchDirectoryIfNotYetExists()
        {
            CreateDirectoryIfNotYetExists(this.GetBranchDirectory());
        }

        private void CreateUseCaseDirectoryIfNotYetExists(UseCase useCase)
        {
            CreateDirectoryIfNotYetExists(this.GetUseCaseDirectory(useCase));
        }

        private void CreateScenarioDirectoryIfNotYetExists(string useCaseName, Scenario scenario)
        {
            CreateDirectoryIfNotYetExists(this.GetScenarioDirectory(useCaseName, scenario));
        }

        private void CreateStepDirectoryIfNotYetExists(string useCaseName, string scenarioName)
        {
            CreateDirectoryIfNotYetExists(this.GetStepsDirectory(useCaseName, scenarioName));
        }

        private void CreateScreenshotDirectoryIfNotYetExists(string useCaseName, string scenarioName)
        {
            CreateDirectoryIfNotYetExists(this.GetScreenshotDirectory(useCaseName, scenarioName));
        }

        private void CreateDirectoryIfNotYetExists(string directory)
        {
            this.DocuFiles.AssertRootDirectoryExists();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static void ExecuteAsyncXmlWriter<T>(T entity, string destFile) where T : class
        {
            ScenarioDocuXMLFileUtil.MarshalXml(entity, destFile);
        }

        private static void ExecuteAsyncImageWriter(string fileName, byte[] file)
        {
            ScenarioDocuImageFileUtil.MarshalImage(fileName, file);
        }

        private string GetBuildDirectory()
        {
            return this.DocuFiles.GetBuildDirectory(this._branchName, this._buildName);
        }

        private string GetBranchDirectory()
        {
            return this.DocuFiles.GetBranchDirectory(this._branchName);
        }

        private string GetUseCaseDirectory(UseCase useCase)
        {
            return this.DocuFiles.GetUseCaseDirectory(this._branchName, this._buildName, useCase.Name);
        }

        private string GetStepsDirectory(string useCaseName, string scenarioName)
        {
            return this.DocuFiles.GetScenarioStepDirectory(
                this._branchName,
                this._buildName,
                useCaseName,
                scenarioName);
        }

        private string GetScenarioDirectory(string useCaseName, Scenario scenario)
        {
            return this.DocuFiles.GetScenarioDirectory(
                this._branchName,
                this._buildName,
                useCaseName,
                scenario.Name);
        }

        private string GetScreenshotDirectory(string useCaseName, string scenarioName)
        {
            return this.DocuFiles.GetScreenshotDirectory(
                this._branchName, 
                this._buildName, 
                useCaseName, 
                scenarioName);
        }

        public void SaveBuildDescription(Build build)
        {
            var destBuildFile = this.DocuFiles.GetBuildFile(this._branchName, this._buildName);
            this.CreateBuildDirectoryIfNotYetExists();
            ExecuteAsyncXmlWriter(build, destBuildFile);
        }

        public void SaveBranchDescription(Branch branch)
        {
            var destBranchFile = this.DocuFiles.GetBranchFile(this._branchName);
            this.CreateBranchDirectoryIfNotYetExists();
            ExecuteAsyncXmlWriter(branch, destBranchFile);
        }

        public void SaveUseCase(UseCase useCase)
        {
            var desUseCaseFile = this.DocuFiles.GetUseCaseFile(this._branchName, this._buildName, useCase.Name);
            this.CreateUseCaseDirectoryIfNotYetExists(useCase);
            ExecuteAsyncXmlWriter(useCase, desUseCaseFile);
        }

        public void SaveScenario(string useCaseName, Scenario scenario)
        {
            var desScenarioFile = this.DocuFiles.GetScenarioFile(
                this._branchName,
                this._buildName,
                useCaseName,
                scenario.Name);
            this.CreateScenarioDirectoryIfNotYetExists(useCaseName, scenario);
            ExecuteAsyncXmlWriter(scenario, desScenarioFile);
        }

        public void SaveStep(string useCaseName, string scenarioName, Step step)
        {
            var desScenarioStepFile = this.DocuFiles.GetScenarioStepFile(
                this._branchName,
                this._buildName,
                useCaseName,
                scenarioName,
                step.StepDescription.Index);
            this.CreateStepDirectoryIfNotYetExists(useCaseName, scenarioName);
            ExecuteAsyncXmlWriter(step, desScenarioStepFile);
        }

        public void SaveScreenshot(string usecaseName, string scenarioName, Step step, byte[] file)
        {
            var desScreenshotFile = this.DocuFiles.GetScreenshotFile(
                this._branchName, 
                this._buildName, 
                usecaseName, 
                scenarioName, 
                step.StepDescription.Index);
            this.CreateScreenshotDirectoryIfNotYetExists(usecaseName, scenarioName);
            ExecuteAsyncImageWriter(desScreenshotFile, file);
        }

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
