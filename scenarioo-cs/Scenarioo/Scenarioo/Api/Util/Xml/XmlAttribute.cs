namespace Scenarioo.Api.Util.Xml
{
    public class XmlAttribute
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public object BindingObject { get; set; }

        public XmlAttribute(object bindingObject, string name, string value)
        {
            this.Name = name;
            this.Value = value;
            this.BindingObject = bindingObject;
        }


    }
}
