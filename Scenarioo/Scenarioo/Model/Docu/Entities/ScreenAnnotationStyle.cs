using System.Xml.Serialization;

namespace Scenarioo.Model.Docu.Entities
{
    public enum ScreenAnnotationStyle
    {
        [XmlEnum("default")]
        Default,

        [XmlEnum("click")]
        Click,

        [XmlEnum("keyboard")]
        Keyboard,

        [XmlEnum("expected")]
        Expected,

        [XmlEnum("navigateToUrl")]
        NavigateToUrl,

        [XmlEnum("error")]
        Error,

        [XmlEnum("warn")]
        Warn,

        [XmlEnum("info")]
        Info,

        [XmlEnum("highlight")]
        Highlight,
    }
}