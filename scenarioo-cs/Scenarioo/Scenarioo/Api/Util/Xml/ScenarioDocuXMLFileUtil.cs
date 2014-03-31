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

namespace Scenarioo.Api.Util.Xml
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Exception = System.Exception;

    public class ScenarioDocuXMLFileUtil
    {
        private const int Buffer = 8;

        public static string XmlKeyIdentifier = "key";

        public static string XmlValueIdentifier = "value";

        public static string SchemaInstanceNamespace = "http://www.w3.org/2001/XMLSchema-instance";

        public static string ScenarioNameSpace = "http://www.scenarioo.org/scenarioo";

        public static string XmlSchema = "http://www.w3.org/2001/XMLSchema";

        public static T Unmarshal<T>(string srcFile) where T : class
        {
            if (!File.Exists(srcFile))
            {
                throw new FileNotFoundException(srcFile);
            }

            Lock(
                srcFile,
                (f) =>
                    {
                        try
                        {
                            var cbuffer = new byte[] { };
                            f.Write(cbuffer, 0, cbuffer.Length);
                        }
                        catch (IOException e)
                        {
                            throw new Exception(string.Format("Could not unmarshall Object from file:{0}", srcFile), e);
                        }
                    });

            using (var fs = new FileStream(srcFile, FileMode.Open, FileAccess.Read, FileShare.None, Buffer, true))
            {
                var desirializedObject = ScenarioDocuXMLUtil.Unmarshal<T>(fs);
                fs.Flush();
                fs.Close();
                fs.Dispose();

                return desirializedObject;
            }
        }

        public static async Task Marshal<T>(T entity, string destFile) where T : class
        {
            try
            {
                using (
                    var fs = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None, Buffer, true))
                {
                    await ScenarioDocuXMLUtil.Marshal(entity, fs);
                    fs.Flush();
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (Exception e)
            {
                throw new Exception(
                    string.Format(
                        "Could not marshall Object of type:{0} into file:{1}",
                        entity.GetType().Name,
                        destFile),
                    e);
            }
        }

        public static void Lock(string srcPath, Action<FileStream> action)
        {
            var autoResetEvent = new AutoResetEvent(false);

            while (true)
            {
                try
                {
                    using (var file = File.Open(srcPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Write))
                    {
                        action(file);
                        break;
                    }
                }
                catch (IOException)
                {
                    var fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(srcPath))
                                                {
                                                    EnableRaisingEvents
                                                        = true
                                                };

                    fileSystemWatcher.Changed += (o, e) =>
                        {
                            if (Path.GetFullPath(e.FullPath) == Path.GetFullPath(srcPath))
                            {
                                autoResetEvent.Set();
                            }
                        };

                    autoResetEvent.WaitOne();
                }
            }
        }

    }
}
