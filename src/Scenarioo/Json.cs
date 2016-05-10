using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Scenarioo
{
    public static class Json
    {
        public static JsonSerializerSettings Settings = new JsonSerializerSettings()
                                                        {
                                                            NullValueHandling = NullValueHandling.Ignore,
                                                            ContractResolver = new CamelCasePropertyNamesContractResolver() {  SerializeCompilerGeneratedMembers = false }
                                                        };
    }

    //public class SkipEmptyListContractResolver : CamelCasePropertyNamesContractResolver
    //{
    //    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    //    {
    //        JsonProperty property = base.CreateProperty(member, memberSerialization);

    //        if (property.DeclaringType == typeof(IList<>))
    //                   {
    //                        property.ShouldSerialize =
    //                           instance =>
    //                           { return instance != null && (instance as IList<object>).Count > 0; };
    //                    }
            
    //    return property;
    //    }
    //}
}