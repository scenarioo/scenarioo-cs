using System;
using System.Xml.Serialization;

using Scenarioo.Model.Docu.Entities.Generic;

namespace Scenarioo.Model.Docu.Entities
{
    [Serializable]
    [XmlRoot("screenAnnotation")]
    public class ScreenAnnotation
    {
        [XmlElement("region")]
        public ScreenRegion Region { get; set; }

        [XmlElement("style")]
        public ScreenAnnotationStyle Style { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("screenText")]
        public string ScreenText { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("clickAction")]
        public ScreenAnnotationClickAction? ClickAction { get; set; }

        [XmlElement("clickActionUrl")]
        public string ClickActionUrl { get; set; }

        [XmlElement("details")]
        public Details Details { get; set; }

        public ScreenAnnotation()
        {
        }

        public bool ShouldSerializeScreenAnnotationClickAction()
        {
            return ClickAction.HasValue;
        }

        public ScreenAnnotation(ScreenRegion region)
            : this()
        {
            Region = region;
        }
    }
}