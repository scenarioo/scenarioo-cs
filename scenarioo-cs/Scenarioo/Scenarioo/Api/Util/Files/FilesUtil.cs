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
using System.Text;
using System.Web;

using Scenarioo.Api.Exception;

namespace Scenarioo.Api.Util.Files
{
    public class FilesUtil
    {
        public static string EncodeName(string name)
        {
            try
            {
                return HttpUtility.UrlEncode(name, Encoding.UTF8);
            }
            catch (System.Exception e)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format(
                        "Unsupported UTF-8 charset. Scenarioo needs to run on a server environment that supports 'UTF-8'."),
                    e.Message);
            }
        }

        /// <summary>
        /// Read all files from 'directory'.
        /// </summary>
        /// <param name="directory">
        /// </param>
        /// <returns>
        /// All files in given directory
        /// </returns>
        public static string[] GetListOfFiles(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new ResourceNotFoundException(directory);
            }

            return Directory.GetFiles(directory);
        }

        /// <summary>
        /// Read all files with given name from all subdirectories of 'directory'.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filename"></param>
        /// <returns>All all files from subdirectories</returns>
        public static string[] GetListOfFilesFromSubdirs(string directory, string filename)
        {
            if (!Directory.Exists(directory))
            {
                throw new ResourceNotFoundException(directory);
            }

            return Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
        }

        /// <summary>
        /// List all files in the given directory sorted alphanumerically using a collator.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns>All files in directory ordered by alphanumeric</returns>
        public static string[] ListFiles(string directory)
        {
            var files = GetListOfFiles(directory);
            Array.Sort(files);
            return files;
        }
    }
}
