﻿/* scenarioo-api
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
using System.Collections.Generic;
using System.IO;

namespace Scenarioo.Api.Files
{
    using Scenarioo.Api.Util.Files;

    /// <summary>
    /// Represents the file structure of the documentation.
    /// </summary>
    public class ScenarioDocuFiles
    {
        private const string FileNameBranch = "branch.xml";

        private const string FileNameBuild = "build.xml";

        private const string DirectoryNameScenarioScreenshot = "screenshots";

        private const string DirectoryNameScenarioSteps = "steps";

        private const string FileNameScenario = "scenario.xml";

        private const string FileNameUseCase = "usecase.xml";

        private readonly string rootDirectory;

        private readonly string digitFormat = CreateNumberFormatWithMinimumIntegerDigits(3, "0");

        public ScenarioDocuFiles(string rootDirectory)
        {
            this.rootDirectory = rootDirectory;
        }

        public void AssertRootDirectoryExists()
        {
            if (!Directory.Exists(this.rootDirectory))
            {
                throw new ArgumentException(
                    string.Format("Directory for docu content generation does not exist: {0}", this.rootDirectory));
            }
        }

        public string GetBranchFile(string branchName)
        {
            return string.Format(
                @"{0}{1}{2}",
                this.GetBranchDirectory(branchName),
                Path.DirectorySeparatorChar,
                FileNameBranch);
        }

        public string GetBuildFile(string branchName, string buildName)
        {
            return string.Format(
                @"{0}{1}{2}",
                this.GetBuildDirectory(branchName, buildName),
                Path.DirectorySeparatorChar,
                FileNameBuild);
        }

        public string GetUseCaseFile(string branchName, string buildName, string useCaseName)
        {
            return string.Format(
                @"{0}{1}{2}",
                this.GetUseCaseDirectory(branchName, buildName, useCaseName),
                Path.DirectorySeparatorChar,
                FileNameUseCase);
        }

        public string GetScenarioFile(string branchName, string buildName, string useCaseName, string scenarioName)
        {
            return string.Format(
                @"{0}{1}{2}",
                this.GetScenarioDirectory(branchName, buildName, useCaseName, scenarioName),
                Path.DirectorySeparatorChar,
                FileNameScenario);
        }

        public string GetScenarioStepFile(string branchName, string buildName, string useCaseName, string scenarioName, int stepIndex)
        {
            return string.Format(
                @"{0}{1}{2}",
                this.GetScenarioStepDirectory(branchName, buildName, useCaseName, scenarioName),
                Path.DirectorySeparatorChar,
                string.Format(@"{0}{1}", stepIndex.ToString(this.digitFormat), ".xml"));
        }

        /// <summary>
        /// Get the file name of the file where the screenshot of a step is stored.
        /// </summary>
        /// <param name="branchName"></param>
        /// <param name="buildName"></param>
        /// <param name="useCaseName"></param>
        /// <param name="scenarioName"></param>
        /// <param name="stepIndex"></param>
        /// <returns>Screenshot filename</returns>
        public string GetScreenshotFile(string branchName, string buildName, string useCaseName, string scenarioName, int stepIndex)
        {
            return string.Format(
                @"{0}{1}{2}",
                this.GetScreenshotDirectory(branchName, buildName, useCaseName, scenarioName),
                Path.DirectorySeparatorChar,
                string.Format(@"{0}{1}", stepIndex.ToString(this.digitFormat), ".png"));
        }

        public string GetScreenshotFileName(int stepIndex)
        {
            return string.Format(@"{0}{1}", stepIndex.ToString(this.digitFormat), ".png");
        }

        /// <summary>
        /// Concatenate directory of documentation files.
        /// </summary>
        /// <param name="buildName"></param>
        /// <param name="branchName"></param>
        /// <returns>Directory of the build</returns>
        public string GetBuildDirectory(string branchName, string buildName)
        {
            return string.Format(
                @"{0}{1}{2}",
                this.GetBranchDirectory(branchName),
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(buildName));
        }

        public string GetBranchDirectory(string branchName)
        {
            return string.Format(
                @"{0}{1}{2}",
                this.rootDirectory,
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(branchName));
        }

        public string GetUseCaseDirectory(string branchName, string buildName, string useCaseName)
        {
            return string.Format(
                @"{0}{1}{2}",
                this.GetBuildDirectory(branchName, buildName),
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(useCaseName));
        }

        public string GetScenarioDirectory(string branchName, string buildName, string useCaseName, string scenarioName)
        {
            return string.Format(
                @"{0}{1}{2}",
                this.GetUseCaseDirectory(branchName, buildName, useCaseName),
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(scenarioName));
        }

        public string GetScenarioStepDirectory(string branchName, string buildName, string useCaseName, string scenarioName)
        {
            return string.Format(
                @"{0}{1}{2}",
                this.GetScenarioDirectory(branchName, buildName, useCaseName, scenarioName),
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(DirectoryNameScenarioSteps));
        }

        public string GetScreenshotDirectory(string branchName, string buildName, string useCaseName, string scenarioName)
        {
            return string.Format(
                @"{0}{1}{2}",
                this.GetScenarioDirectory(branchName, buildName, useCaseName, scenarioName),
                Path.DirectorySeparatorChar,
                FilesUtil.EncodeName(DirectoryNameScenarioScreenshot));
        }

        private static string CreateNumberFormatWithMinimumIntegerDigits(int minimumIntegerDigits, string digitPatern)
        {
            var format = new List<string>();

            for (var i = 0; i < minimumIntegerDigits; i++)
            {
                format.Add(digitPatern);
            }

            return string.Concat(format.ToArray());
        }
    }
}
