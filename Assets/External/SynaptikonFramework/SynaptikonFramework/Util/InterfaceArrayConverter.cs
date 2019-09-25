using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace SynaptikonFramework.Util
{
    public class InterfaceArrayConverter<TInterface, TImplementation> : JsonConverter where TImplementation : TInterface
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            TImplementation[] res;
            try{
                res = serializer.Deserialize<TImplementation[]>(reader);
            }catch(Exception e){
                res = new TImplementation[0];
            }

            var ret = new TInterface[res.Length];
            for (var i = 0; i < res.Length; i++){
                ret[i] = res[i];
            }
            return ret;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

}
