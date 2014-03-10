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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scenarioo.Api.Util.Xml
{
    using System.IO;

    using Scenarioo.Model.Docu.Entities;

    using Exception = System.Exception;

    public class ScenarioDocuXMLFileUtil
    {
        public static T Unmarshal<T>(string srcFile) where T : class
        {
            if (!File.Exists(srcFile))
            {
                throw new FileNotFoundException(srcFile);
            }

            try
            {
                using (var fs = new FileStream(srcFile, FileMode.Open))
                {
                    return ScenarioDocuXMLUtil.Unmarshal<T>(fs);
                }

            }
            catch (Exception e)
            {

                throw new Exception(string.Format("Could not unmarshall Object from file:{0}", srcFile), e);
            }

        }

        public static void Marshal<T>(T entity, string destFile) where T : class
        {
            try
            {
                using (var fs = new FileStream(destFile, FileMode.CreateNew))
                {
                    ScenarioDocuXMLUtil.Marshal(entity, fs);
                    fs.Flush();
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Could not marshall Object of type:{0} into file:{1}", entity.GetType().Name, destFile), e);
            }
        }
    }
}
