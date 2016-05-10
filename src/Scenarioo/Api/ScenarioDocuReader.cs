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

using System;

using Newtonsoft.Json;

using Scenarioo.Api.Files;
using Scenarioo.Api.Util.Xml;
using Scenarioo.Model.Docu.Entities;

namespace Scenarioo.Api
{
    /// <summary>
    /// Gives access to the generated scenario documentation files in the file system.
    /// </summary>
    public class ScenarioDocuReader
    {
        private readonly ScenarioDocuFiles _docuFiles;
        private readonly JsonSerializer _serializer;

        public ScenarioDocuReader(string rootDirectory)
        {
            _serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                ContractResolver = new SkipEmptyContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
            };

            _docuFiles = new ScenarioDocuFiles(rootDirectory);
        }

        public Branch LoadBranch(string branchName)
        {
            return JsonConvert.DeserializeObject<Branch>(_docuFiles.GetBranchFile(branchName));
        }

        public Build LoadBuild(string branchName, string buildName)
        {
            return JsonConvert.DeserializeObject<Build>(_docuFiles.GetBuildFile(branchName, buildName));
        }

        public UseCase LoadUseCase(string branchName, string buildName, string useCaseName)
        {
            return JsonConvert.DeserializeObject<UseCase>(_docuFiles.GetUseCaseFile(branchName, buildName, useCaseName));
        }

        public Scenario LoadScenario(string branchName, string buildName, string useCaseName, string scenarioName)
        {
            return JsonConvert.DeserializeObject<Scenario>(
                    _docuFiles.GetScenarioFile(branchName, buildName, useCaseName, scenarioName));
        }

        public Step LoadStep(
            string branchName,
            string buildName,
            string useCaseName,
            string scenarioName,
            string scenarioStepName,
            int stepIndex)
        {
            return JsonConvert.DeserializeObject<Step>(
                    _docuFiles.GetScenarioStepFile(
                        branchName,
                        buildName,
                        useCaseName,
                        scenarioName,
                        stepIndex));
        }

        public Step[] LoadSteps(
            string branchName, string buildName, string useCaseName, string scenarioName, string scenarioStepName)
        {
            throw new NotImplementedException();
        }
    }
}
