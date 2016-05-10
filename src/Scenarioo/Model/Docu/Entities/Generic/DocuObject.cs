using System.Collections.Generic;

namespace Scenarioo.Model.Docu.Entities.Generic
{
    public class DocuObject
    {
        public string Id { get; set; }

        public string LabelKey { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public List<DocuObject> Properties { get; set; }
        public List<DocuObject> Items { get; set; }

        public DocuObject(string value = null, string id = null, string labelKey = null, string type = null)
        {
            Id = id;
            LabelKey = labelKey;
            Value = value;
            Type = type;

            Properties = new List<DocuObject>();
            Items = new List<DocuObject>();
        }
    }
}