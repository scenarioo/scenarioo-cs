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

namespace Scenarioo.Api
{
    using System.IO;

    using Scenarioo.Api.Files;
    using Scenarioo.Api.Util.Xml;
    using Scenarioo.Model.Docu.Entities;

    public class ScenarioDocuWriter
    {
        public ScenarioDocuFiles DocuFiles { get; set; }

        private readonly string branchName;

        private readonly string buildName;

        private readonly string useCaseName;

        private readonly string scenarioName;

        private readonly string scenarioStepName;

        private readonly int stepIndex;

        private string DestinationRootDirectory { get; set; }

        public ScenarioDocuWriter(
            string destinationRootDirectory,
            string branchName,
            string buildName,
            string useCaseName,
            string scenarioName,
            string scenarioStepName,
            int stepIndex)
        {
            this.DocuFiles = new ScenarioDocuFiles(destinationRootDirectory);
            this.branchName = branchName;
            this.buildName = buildName;
            this.useCaseName = useCaseName;
            this.scenarioName = scenarioName;
            this.scenarioStepName = scenarioStepName;
            this.DestinationRootDirectory = destinationRootDirectory;
            this.stepIndex = stepIndex;
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

        private void CreateUseCaseDirectoryIfNotYetExists()
        {
            CreateDirectoryIfNotYetExists(this.GetUseCaseDirectory());
        }

        private void CreateScenariooDirectoryIfNotYetExists()
        {
            CreateDirectoryIfNotYetExists(this.GetScenarioDirectory());
        }

        private void CreateScenariooStepDirectoryIfNotYetExists()
        {
            CreateDirectoryIfNotYetExists(this.GetScenarioStepDirectory());
        }

        private void CreateDirectoryIfNotYetExists(string directory)
        {
            this.DocuFiles.AssertRootDirectoryExists();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static void ExecuteAsyncWrite<T>(T entity, string destFile) where T : class
        {
            ScenarioDocuXMLFileUtil.Marshal(entity, destFile);
        }

        private string GetBuildDirectory()
        {
            return this.DocuFiles.GetBuildDirectory(this.buildName);
        }

        private string GetBranchDirectory()
        {
            return this.DocuFiles.GetBranchDirectory(this.buildName, this.branchName);
        }

        private string GetUseCaseDirectory()
        {
            return this.DocuFiles.GetUseCaseDirectory(this.buildName, this.branchName, this.useCaseName);
        }

        private string GetScenarioStepDirectory()
        {
            return this.DocuFiles.GetScenarioStepDirectory(
                this.buildName,
                this.branchName,
                this.useCaseName,
                this.scenarioName,
                this.scenarioStepName);
        }

        private string GetScenarioDirectory()
        {
            return this.DocuFiles.GetScenarioDirectory(
                this.buildName,
                this.branchName,
                this.useCaseName,
                this.scenarioName);
        }

        public void SaveBuildDescription(Build build)
        {
            var destBuildFile = this.DocuFiles.GetBuildFile(this.buildName);
            this.CreateBuildDirectoryIfNotYetExists();
            ExecuteAsyncWrite(build, destBuildFile);
        }

        public void SaveBranchDescription(Branch branch)
        {
            var destBranchFile = this.DocuFiles.GetBranchFile(this.buildName, branch.Name);
            this.CreateBranchDirectoryIfNotYetExists();
            ExecuteAsyncWrite(branch, destBranchFile);
        }

        public void SaveUseCase(UseCase useCase)
        {
            var desUseCaseFile = this.DocuFiles.GetUseCaseFile(this.buildName, this.branchName, useCase.Name);
            this.CreateUseCaseDirectoryIfNotYetExists();
            ExecuteAsyncWrite(useCase, desUseCaseFile);
        }

        public void SaveScenario(Scenario scenario)
        {
            var desScenarioFile = this.DocuFiles.GetScenarioFile(
                this.buildName,
                this.branchName,
                this.useCaseName,
                scenario.Name);
            this.CreateScenariooDirectoryIfNotYetExists();
            ExecuteAsyncWrite(scenario, desScenarioFile);
        }

        public void SaveStep(Step step)
        {
            var desScenarioStepFile = this.DocuFiles.GetScenarioStepFile(
                this.buildName,
                this.branchName,
                this.useCaseName,
                this.scenarioName,
                this.scenarioStepName,
                stepIndex);
            this.CreateScenariooStepDirectoryIfNotYetExists();
            ExecuteAsyncWrite(step, desScenarioStepFile);
        }
    }
}
