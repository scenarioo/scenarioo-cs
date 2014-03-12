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

namespace Scenarioo.Api.Files
{
    using System;
    using System.IO;

    using Scenarioo.Api.Util.Files;

    public class ScenarioDocuFiles
    {
        private const string FileNameBranch = "branch.xml";

        private const string FileNameBuild = "build.xml";

        private const string FileNameScenariooScreenshot = "screenshots";

        private const string FileNameScenariooStep = "steps";

        private const string FileNameScenarioo = "scenario.xml";

        private const string FileNameUseCase = "usecase.xml";

        private readonly string rootDirectory;
    
        public ScenarioDocuFiles(string rootDirectory)
        {
            this.rootDirectory = rootDirectory;
        }

        public void AssertRootDirectoryExists()
        {
            if (!Directory.Exists(this.rootDirectory))
            {
                throw new ArgumentException(string.Format("Directory for docu content generation does not exist: {0}", rootDirectory));
            }
        }

        public string GetBranchFile(string buildName, string branchName)
        {
            return string.Format(@"{0}{1}{2}", GetBranchDirectory(buildName, branchName), Path.DirectorySeparatorChar, FileNameBranch);
        }

        public string GetBuildFile(string buildName)
        {
            return string.Format(@"{0}{1}{2}", GetBuildDirectory(buildName), Path.DirectorySeparatorChar, FileNameBuild);
        }

        public string GetUseCaseFile(string buildName, string branchName, string useCaseName)
        {
            return string.Format(@"{0}{1}{2}", GetBranchDirectory(buildName, branchName), Path.DirectorySeparatorChar, FileNameUseCase);
        }


        /// <summary>
        /// concatenate directory
        /// </summary>
        /// <param name="buildName"></param>
        /// <param name="branchName"></param>
        /// <returns></returns>
        public string GetBranchDirectory(string buildName, string branchName)
        {
            return string.Format(@"{0}{1}{2}", this.GetBuildDirectory(buildName), Path.DirectorySeparatorChar, FilesUtil.EncodeName(branchName));
        }


        public string GetBuildDirectory(string buildName)
        {
            return string.Format(@"{0}{1}{2}", this.rootDirectory, Path.DirectorySeparatorChar, buildName);
        }

        public string GetUseCaseDirectory(string buildName, string branchName, string useCaseName)
        {
            return string.Format(@"{0}{1}{2}", GetBranchDirectory(buildName, branchName), Path.DirectorySeparatorChar, useCaseName);
        }
    }
}
