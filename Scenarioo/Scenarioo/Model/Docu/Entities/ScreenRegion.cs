using System;
using System.Xml.Serialization;

namespace Scenarioo.Model.Docu.Entities
{
    [Serializable]
    public class ScreenRegion
    {
        [XmlElement("x")]
        public int X { get; set; }

        [XmlElement("y")]
        public int Y { get; set; }

        [XmlElement("height")]
        public int Height { get; set; }

        [XmlElement("width")]
        public int Width { get; set; }

        public ScreenRegion()
        {
        }

        public ScreenRegion(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Height = height;
            this.Width = width;
        }
    }
}