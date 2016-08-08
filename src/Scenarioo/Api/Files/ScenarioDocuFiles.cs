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
using Scenarioo.Api.Util.Files;

namespace Scenarioo.Api.Files
{
    /// <summary>
    /// Represents the file structure of the documentation.
    /// </summary>
    public class ScenarioDocuFiles
    {
        private const string FileNameBranch = "branch.json";
        private const string FileNameBuild = "build.json";
        private const string FileNameScenario = "scenario.json";
        private const string FileNameUseCase = "usecase.json";

        private const string DirectoryNameScenarioScreenshot = "screenshots";
        private const string DirectoryNameScenarioSteps = "steps";
        private const string DirectoryNameHtml = "html";

        private readonly string _rootDirectory;

        public ScenarioDocuFiles(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
        }

        public void AssertRootDirectoryExists()
        {
            if (!Directory.Exists(_rootDirectory))
            {
                throw new ArgumentException(string.Format("Directory for docu content generation does not exist: {0}", _rootDirectory));
            }
        }

        public string GetBranchFile(string branchId)
        {
            return string.Format(
                @"{0}{1}{2}",
                GetBranchDirectory(branchId),
                Path.DirectorySeparatorChar,
                FileNameBranch);
        }

        public string GetBuildFile(string branchId, string buildId)
        {
            return string.Format(
                @"{0}{1}{2}",
                GetBuildDirectory(branchId, buildId),
                Path.DirectorySeparatorChar,
                FileNameBuild);
        }

        public string GetUseCaseFile(string branchId, string buildId, string useCaseId)
        {
            return string.Format(
                @"{0}{1}{2}",
                GetUseCaseDirectory(branchId, buildId, useCaseId),
                Path.DirectorySeparatorChar,
                FileNameUseCase);
        }

        public string GetScenarioFile(string branchId, string buildId, string useCaseId, string scenarioId)
        {
            return string.Format(
                @"{0}{1}{2}",
                GetScenarioDirectory(branchId, buildId, useCaseId, scenarioId),
                Path.DirectorySeparatorChar,
                FileNameScenario);
        }

        public string GetScenarioStepFile(string branchId, string buildId, string useCaseId, string scenarioId, int stepIndex)
        {
            return string.Format(
                @"{0}{1}{2:000}.json",
                GetScenarioStepDirectory(branchId, buildId, useCaseId, scenarioId),
                Path.DirectorySeparatorChar,
                stepIndex);
        }

        public string GetHtmlFile(string branchId, string buildId, string useCaseId, string scenarioId, int stepIndex)
        {
            return string.Format(
                @"{0}{1}{2:000}.html",
                GetHtmlDirectory(branchId, buildId, useCaseId, scenarioId),
                Path.DirectorySeparatorChar,
                stepIndex);
        }

        /// <summary>
        /// Get the file name of the file where the screenshot of a step is stored.
        /// </summary>
        /// <returns>Screenshot filename</returns>
        public string GetScreenshotFile(string branchId, string buildId, string useCaseId, string scenarioId, int stepIndex)
        {
            return string.Format(
                @"{0}{1}{2:000}.png",
                GetScreenshotDirectory(branchId, buildId, useCaseId, scenarioId),
                Path.DirectorySeparatorChar,
                stepIndex);
        }

        public string GetScreenshotFileName(int stepIndex)
        {
            return string.Format(@"{0:000}.png", stepIndex);
        }

        /// <summary>
        /// Concatenate directory of documentation files.
        /// </summary>
        /// <returns>Directory of the build</returns>
        public string GetBuildDirectory(string branchId, string buildId)
        {
            return string.Format(
                @"{0}{1}{2}",
                GetBranchDirectory(branchId),
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(buildId));
        }

        public string GetBranchDirectory(string branchId)
        {
            return string.Format(
                @"{0}{1}{2}",
                _rootDirectory,
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(branchId));
        }

        public string GetUseCaseDirectory(string branchId, string buildId, string useCaseId)
        {
            return string.Format(
                @"{0}{1}{2}",
                GetBuildDirectory(branchId, buildId),
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(useCaseId));
        }

        public string GetScenarioDirectory(string branchId, string buildId, string useCaseId, string scenarioId)
        {
            return string.Format(
                @"{0}{1}{2}",
                GetUseCaseDirectory(branchId, buildId, useCaseId),
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(scenarioId));
        }

        public string GetScenarioStepDirectory(string branchId, string buildId, string useCaseId, string scenarioId)
        {
            return string.Format(
                @"{0}{1}{2}",
                GetScenarioDirectory(branchId, buildId, useCaseId, scenarioId),
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(DirectoryNameScenarioSteps));
        }

        public string GetScreenshotDirectory(string branchId, string buildId, string useCaseId, string scenarioId)
        {
            return string.Format(
                @"{0}{1}{2}",
                GetScenarioDirectory(branchId, buildId, useCaseId, scenarioId),
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(DirectoryNameScenarioScreenshot));
        }

        public string GetHtmlDirectory(string branchId, string buildId, string useCaseId, string scenarioId)
        {
            return string.Format(
                @"{0}{1}{2}",
                GetScenarioDirectory(branchId, buildId, useCaseId, scenarioId),
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(DirectoryNameHtml));
        }
    }
}
