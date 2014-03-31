namespace Scenarioo.Api.Util.Xml
{
    public class XmlElement
    {
        public string Name { get; set; }

        public string ElementString { get; set; }

        public object BindingObject { get; set; }

        public string ObjectName { get; set; }

        public XmlElement()
        {

        }
        
        public XmlElement(object bindingObject, string name, string elementString)
        {
            this.Name = name;
            this.ElementString = elementString;
            this.BindingObject = bindingObject;
        }

        public XmlElement(object bindingObject, string name)
        {
            this.Name = name;
            this.BindingObject = bindingObject;
        }
    }
}
