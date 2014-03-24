using System;

namespace Scenarioo.Api.Util.Xml
{
    using System.Runtime.Serialization;
    using System.Xml;

    public class ContractDataCustomResolver : DataContractResolver
    {
        private const string ResolverNamespace = "http://www.w3.org/2001/XMLSchema-instance";

        // Used at deserialization
        // Allows users to map xsi:type name to any Type 
        public override Type ResolveName(
            string typeName,
            string typeNamespace,
            Type declaredType,
            DataContractResolver knownTypeResolver)
        {
            throw new NotImplementedException();
        }

        public override bool TryResolveType(
            Type type,
            Type declaredType,
            DataContractResolver knownTypeResolver,
            out XmlDictionaryString typeName,
            out XmlDictionaryString typeNamespace)
        {
//            string name = type.Name;
//            string namesp = type.Namespace;
//            typeName = new XmlDictionaryString(XmlDictionary.Empty, name, 0);
//            typeNamespace = new XmlDictionaryString(XmlDictionary.Empty, namesp, 0);

            var dictionary = new XmlDictionary();
            typeName = dictionary.Add(type.Name);
            typeNamespace = dictionary.Add(ResolverNamespace);

            return true;
        }
    }
}
