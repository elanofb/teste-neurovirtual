using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BLL.Services{

    public class IgnoreObjectsResolver : DefaultContractResolver{
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization){
            JsonProperty prop = base.CreateProperty(member, memberSerialization);

            if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string)){
                prop.ShouldSerialize = obj => false;
            }

            return prop;
        }
    }
}