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

namespace Scenarioo.Api
{
    using System.IO;
    using System.Threading.Tasks;

    using Scenarioo.Api.Files;
    using Scenarioo.Api.Util.Xml;
    using Scenarioo.Model.Docu.Entities;

    public class ScenarioDocuWriter
    {
        public ScenarioDocuFiles DocuFiles { get; set; }
        
        private string BranchName { get; set; }

        private string BuildName { get; set; }

        private string UseCaseName { get; set; }

        private string DestinationRootDirectory { get; set; }

        public ScenarioDocuWriter(string destinationRootDirectory, string branchName, string buildName)
        {
            this.DocuFiles = new ScenarioDocuFiles(destinationRootDirectory);
            this.BranchName = branchName;
            this.BuildName = buildName;
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

        private void CreateUseCaseDirectoryIfNotYetExists()
        {
            CreateDirectoryIfNotYetExists(this.GetUseCaseDirectory());
        }

        private void CreateDirectoryIfNotYetExists(string directory)
        {
            this.DocuFiles.AssertRootDirectoryExists();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }



        private void ExecuteAsyncWrite<T>(T entity, string destBranchFile) where T : class
        {
            ScenarioDocuXMLFileUtil.Marshal(entity, destBranchFile);
        }

        private string GetBuildDirectory()
        {
            return this.DocuFiles.GetBuildDirectory(this.BuildName);
        }

        private string GetBranchDirectory()
        {
            return this.DocuFiles.GetBranchDirectory(this.BuildName, this.BranchName);
        }

        private string GetUseCaseDirectory()
        {
            return this.DocuFiles.GetUseCaseDirectory(this.BuildName, this.BranchName, this.UseCaseName);
        }

        public void SaveBuildDescription(build build)
        {
            var destBuildFile = this.DocuFiles.GetBuildFile(this.BuildName);
            this.CreateBuildDirectoryIfNotYetExists();
            ExecuteAsyncWrite(build, destBuildFile);
        }

        public void SaveBranchDescription(branch branch)
        {
            var destBranchFile = this.DocuFiles.GetBranchFile(this.BuildName, branch.name);
            this.CreateBranchDirectoryIfNotYetExists();
            ExecuteAsyncWrite(branch, destBranchFile);
        }

        public void SaveUseCase(useCase useCase)
        {
            var desUseCaseFile = this.DocuFiles.GetUseCaseFile(this.BuildName, this.BranchName, useCase.name);
            this.CreateUseCaseDirectoryIfNotYetExists();
            ExecuteAsyncWrite(useCase, desUseCaseFile);
        }

    }
}
