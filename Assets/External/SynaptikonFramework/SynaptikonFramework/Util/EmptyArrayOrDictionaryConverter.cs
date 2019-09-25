using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SynaptikonFramework.Util
{
    public class EmptyArrayOrDictionaryConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
#if UNITY_WEBGL
            return false;
#else
            return objectType.GetTypeInfo().IsAssignableFrom(typeof(Dictionary<string, object>).GetTypeInfo());
#endif
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Object)
            {
                return token.ToObject(objectType);
            }
            else if (token.Type == JTokenType.Array && !token.HasValues)
            {
                return Activator.CreateInstance(objectType);
            }

            throw new JsonSerializationException("Object or empty array expected");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
