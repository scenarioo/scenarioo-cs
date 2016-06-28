using System;
using System.Collections;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Scenarioo
{
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
                    var list = property.ValueProvider.GetValue(obj) as IList;
                    
                    return list == null || list.Count != 0;
                };

                var oldShouldSerialize = property.ShouldSerialize;

                if (oldShouldSerialize != null)
                {
                    property.ShouldSerialize = o => oldShouldSerialize(o) && newShouldSerialize(o);
                }
                else
                {
                    property.ShouldSerialize = newShouldSerialize;
                }
            }

            return property;
        }
    }
}