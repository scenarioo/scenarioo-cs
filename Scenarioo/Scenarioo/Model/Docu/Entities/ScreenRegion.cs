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
            X = x;
            Y = y;
            Height = height;
            Width = width;
            Width = width;
        }
    }
}