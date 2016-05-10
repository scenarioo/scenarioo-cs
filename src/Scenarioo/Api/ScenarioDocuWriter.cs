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

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
        private readonly string _branchId;
        private readonly string _buildId;
        public ScenarioDocuFiles DocuFiles { get; set; }

        private readonly JsonSerializer _serializer;

        private string DestinationRootDirectory { [UsedImplicitly] get; set; }

        public ScenarioDocuWriter(
            string destinationRootDirectory,
            string branchId,
            string buildId)
        {
            DocuFiles = new ScenarioDocuFiles(destinationRootDirectory);
            DestinationRootDirectory = destinationRootDirectory;

            _branchId = branchId;
            _buildId = buildId;

            CreateBuildDirectoryIfNotYetExists();

            _serializer = new JsonSerializer
                          {
                              Formatting = Formatting.Indented,
                              ContractResolver = new SkipEmptyContractResolver(),
                              NullValueHandling = NullValueHandling.Ignore,
                          };
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

        private void CreateHtmlDirectoryIfNotYetExists(string useCaseId, string scenarioId)
        {
            CreateDirectoryIfNotYetExists(DocuFiles.GetHtmlDirectory(_branchId, _buildId, useCaseId, scenarioId));
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
            return DocuFiles.GetBuildDirectory(_branchId, _buildId);
        }

        private string GetBranchDirectory()
        {
            return DocuFiles.GetBranchDirectory(_branchId);
        }

        private string GetUseCaseDirectory(UseCase useCase)
        {
            return DocuFiles.GetUseCaseDirectory(_branchId, _buildId, useCase.Id);
        }

        private string GetStepsDirectory(string useCaseName, string scenarioName)
        {
            return DocuFiles.GetScenarioStepDirectory(
                _branchId,
                _buildId,
                useCaseName,
                scenarioName);
        }
        
        private string GetScenarioDirectory(string useCaseName, Scenario scenario)
        {
            return DocuFiles.GetScenarioDirectory(
                _branchId,
                _buildId,
                useCaseName,
                scenario.Id);
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
                _branchId, 
                _buildId, 
                useCaseName, 
                scenarioName);
        }

        /// <summary>
        /// Save the build description to appropriate directory.
        /// </summary>
        /// <param name="build">The build description to write</param>
        public void SaveBuildDescription(Build build)
        {
            var destBuildFile = DocuFiles.GetBuildFile(_branchId, _buildId);
            CreateBuildDirectoryIfNotYetExists();

            using (StreamWriter file = File.CreateText(destBuildFile))
            {
                _serializer.Serialize(file, build);
            }

            //ExecuteAsyncXmlWriter(build, destBuildFile); // TODO async
        }

        /// <summary>
        /// Save the branch description to appropriate directory.
        /// </summary>
        /// <param name="branch">The branch description to write.</param>
        public void SaveBranchDescription(Branch branch)
        {
            var destBranchFile = DocuFiles.GetBranchFile(_branchId);
            CreateBranchDirectoryIfNotYetExists();

            using (StreamWriter file = File.CreateText(destBranchFile))
            {   
                _serializer.Serialize(file, branch);
            }

            //ExecuteAsyncXmlWriter(branch, destBranchFile); // TODO async
        }

        /// <summary>
        /// Save the use case description to appropriate directory and file.
        /// </summary>
        /// <param name="useCase">The use case description to write</param>
        public void SaveUseCase(UseCase useCase)
        {
            var desUseCaseFile = DocuFiles.GetUseCaseFile(_branchId, _buildId, useCase.Id);
            CreateUseCaseDirectoryIfNotYetExists(useCase);

            using (StreamWriter file = File.CreateText(desUseCaseFile))
            {
                _serializer.Serialize(file, useCase);
            }

            //ExecuteAsyncXmlWriter(useCase, desUseCaseFile); // TODO async
        }

        public void SaveScenario(string useCaseId, Scenario scenario)
        {
            var desScenarioFile = DocuFiles.GetScenarioFile(
                _branchId,
                _buildId,
                useCaseId,
                scenario.Id);

            CreateScenarioDirectoryIfNotYetExists(useCaseId, scenario);

            using (StreamWriter file = File.CreateText(desScenarioFile))
            {
                _serializer.Serialize(file, scenario);
            }

            //ExecuteAsyncXmlWriter(scenario, desScenarioFile);
        }

        public void SaveStep(string useCaseId, string scenarioId, Step step)
        {
            var desScenarioStepFile = DocuFiles.GetScenarioStepFile(
                _branchId,
                _buildId,
                useCaseId,
                scenarioId,
                step.Index);

            CreateStepDirectoryIfNotYetExists(useCaseId, scenarioId);

            using (StreamWriter file = File.CreateText(desScenarioStepFile))
            {
                _serializer.Serialize(file, step);    
            }

            if (!string.IsNullOrEmpty(step.StepHtml))
            {
                CreateHtmlDirectoryIfNotYetExists(useCaseId, scenarioId);

                var htmlFile = DocuFiles.GetHtmlFile(_branchId, _buildId, useCaseId, scenarioId, step.Index);

                File.WriteAllText(htmlFile, step.StepHtml);
            }

            //ExecuteAsyncXmlWriter(step, desScenarioStepFile);
        }

        /// <summary>
        /// Save Screenshot as a PNG file in usual file for step.
        /// </summary>
        /// <param name="usecaseId">The use case name</param>
        /// <param name="scenarioName">The scenario name</param>
        /// <param name="step">The step</param>
        /// <param name="file">Bytes of screenshot</param>
        public void SaveScreenshot(string usecaseId, string scenarioName, Step step, byte[] file)
        {
            var desScreenshotFile = DocuFiles.GetScreenshotFile(
                _branchId, 
                _buildId, 
                usecaseId, 
                scenarioName, 
                step.Index); // TODO: align to java writer library and use filename!
            
            CreateScreenshotDirectoryIfNotYetExists(usecaseId, scenarioName);
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
