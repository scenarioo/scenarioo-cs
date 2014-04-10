using System;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;

namespace Scenarioo.Api.Util.Image
{

    public class ScenarioDocuImageUtil
    {
        public static async Task MarshalImage(string file, MemoryStream st)
        {
            if (st == null)
            {
                throw new NullReferenceException("MemoryStream cannot be null");
            }

            try
            {
                var image = System.Drawing.Image.FromStream(st);
                await Task.Run(() => image.Save(file, ImageFormat.Png));
            }
            catch (SerializationException ex)
            {
                throw new System.Exception(string.Format("Could not marshall image {0}", file), ex);
            }
        }
    }
}
