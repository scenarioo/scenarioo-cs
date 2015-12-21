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
        private readonly string _branchName;
        private readonly string _buildName;
        public ScenarioDocuFiles DocuFiles { get; set; }

        private string DestinationRootDirectory { [UsedImplicitly] get; set; }

        public ScenarioDocuWriter(
            string destinationRootDirectory,
            string branchName,
            string buildName)
        {
            DocuFiles = new ScenarioDocuFiles(destinationRootDirectory);
            DestinationRootDirectory = destinationRootDirectory;

            _branchName = branchName;
            _buildName = buildName;

            CreateBuildDirectoryIfNotYetExists();
        }
        
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
            CreateDirectoryIfNotYetExists(GetBuildDirectory());
        }

        private void CreateBranchDirectoryIfNotYetExists()
        {
            CreateDirectoryIfNotYetExists(GetBranchDirectory());
        }

        private void CreateUseCaseDirectoryIfNotYetExists(UseCase useCase)
        {
            CreateDirectoryIfNotYetExists(GetUseCaseDirectory(useCase));
        }

        private void CreateScenarioDirectoryIfNotYetExists(string useCaseName, Scenario scenario)
        {
            CreateDirectoryIfNotYetExists(GetScenarioDirectory(useCaseName, scenario));
        }

        private void CreateStepDirectoryIfNotYetExists(string useCaseName, string scenarioName)
        {
            CreateDirectoryIfNotYetExists(GetStepsDirectory(useCaseName, scenarioName));
        }

        private void CreateScreenshotDirectoryIfNotYetExists(string useCaseName, string scenarioName)
        {
            CreateDirectoryIfNotYetExists(GetScreenshotDirectory(useCaseName, scenarioName));
        }

        private void CreateDirectoryIfNotYetExists(string directory)
        {
            DocuFiles.AssertRootDirectoryExists();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private string GetBuildDirectory()
        {
            return DocuFiles.GetBuildDirectory(_branchName, _buildName);
        }

        private string GetBranchDirectory()
        {
            return DocuFiles.GetBranchDirectory(_branchName);
        }

        private string GetUseCaseDirectory(UseCase useCase)
        {
            return DocuFiles.GetUseCaseDirectory(_branchName, _buildName, useCase.Name);
        }

        private string GetStepsDirectory(string useCaseName, string scenarioName)
        {
            return DocuFiles.GetScenarioStepDirectory(
                _branchName,
                _buildName,
                useCaseName,
                scenarioName);
        }

        private string GetScenarioDirectory(string useCaseName, Scenario scenario)
        {
            return DocuFiles.GetScenarioDirectory(
                _branchName,
                _buildName,
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
            return DocuFiles.GetScreenshotDirectory(
                _branchName, 
                _buildName, 
                useCaseName, 
                scenarioName);
        }

        /// <summary>
        /// Save the build description to appropriate directory.
        /// </summary>
        /// <param name="build">The build description to write</param>
        public void SaveBuildDescription(Build build)
        {
            var destBuildFile = DocuFiles.GetBuildFile(_branchName, _buildName);
            CreateBuildDirectoryIfNotYetExists();
            ExecuteAsyncXmlWriter(build, destBuildFile);
        }

        /// <summary>
        /// Save the branch description to appropriate directory.
        /// </summary>
        /// <param name="branch">The branch description to write.</param>
        public void SaveBranchDescription(Branch branch)
        {
            var destBranchFile = DocuFiles.GetBranchFile(_branchName);
            CreateBranchDirectoryIfNotYetExists();
            ExecuteAsyncXmlWriter(branch, destBranchFile);
        }

        /// <summary>
        /// Save the use case description to appropriate directory and file.
        /// </summary>
        /// <param name="useCase">The use case description to write</param>
        public void SaveUseCase(UseCase useCase)
        {
            var desUseCaseFile = DocuFiles.GetUseCaseFile(_branchName, _buildName, useCase.Name);
            CreateUseCaseDirectoryIfNotYetExists(useCase);
            ExecuteAsyncXmlWriter(useCase, desUseCaseFile);
        }

        public void SaveScenario(string useCaseName, Scenario scenario)
        {
            var desScenarioFile = DocuFiles.GetScenarioFile(
                _branchName,
                _buildName,
                useCaseName,
                scenario.Name);
            CreateScenarioDirectoryIfNotYetExists(useCaseName, scenario);
            ExecuteAsyncXmlWriter(scenario, desScenarioFile);
        }

        public void SaveStep(string useCaseName, string scenarioName, Step step)
        {
            var desScenarioStepFile = DocuFiles.GetScenarioStepFile(
                _branchName,
                _buildName,
                useCaseName,
                scenarioName,
                step.StepDescription.Index);
            CreateStepDirectoryIfNotYetExists(useCaseName, scenarioName);
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
            var desScreenshotFile = DocuFiles.GetScreenshotFile(
                _branchName, 
                _buildName, 
                usecaseName, 
                scenarioName, 
                step.StepDescription.Index); // TODO: align to java writer library and use filename!
            
            CreateScreenshotDirectoryIfNotYetExists(usecaseName, scenarioName);
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
