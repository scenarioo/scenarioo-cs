using System;
using System.Collections;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Scenarioo
{
    public static class Json
    {
        public static JsonSerializerSettings Settings = new JsonSerializerSettings
                                                        {
                                                            NullValueHandling =
                                                                NullValueHandling.Ignore,
                                                            ContractResolver =
                                                                new SkipEmptyContractResolver
                                                                {
                                                                    SerializeCompilerGeneratedMembers
                                                                        =
                                                                        false
                                                                }
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

    public class SkipEmptyContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var isDefaultValueIgnored = ((property.DefaultValueHandling ?? DefaultValueHandling.Ignore)
                                         & DefaultValueHandling.Ignore) != 0;

            if (isDefaultValueIgnored && !typeof(string).IsAssignableFrom(property.PropertyType)
                && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
            {
                Predicate<object> newShouldSerialize = obj =>
                {
                    var collection = property.ValueProvider.GetValue(obj) as IList;
                    return collection == null || collection.Count != 0;
                };

                var oldShouldSerialize = property.ShouldSerialize;
                property.ShouldSerialize = oldShouldSerialize != null
                    ? o => oldShouldSerialize(o) && newShouldSerialize(o)
                    : newShouldSerialize;
            }
            return property;
        }
    }
}