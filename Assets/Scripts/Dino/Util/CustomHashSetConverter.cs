using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dino.Util
{
    public class CustomHashSetConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(HashSet<T>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            return new HashSet<T>(jObject.Properties().Select(p => (T) p.Value.ToObject(typeof(T))));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var hashSet = (HashSet<T>) value;
            var jo = new JObject(hashSet.Select(s => new JProperty(s.GetType().Name, JToken.FromObject(s))));
            jo.WriteTo(writer);
        }
    }
}