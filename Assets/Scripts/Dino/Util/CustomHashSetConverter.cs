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
            var list = jObject.Property(typeof(T).Name)?.Value.Children().Select(it => it.ToObject(typeof(T))).Cast<T>();
            return new HashSet<T>(list);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var hashSet = (HashSet<T>) value;
            var jo = new JObject(new JProperty(typeof(T).Name, hashSet.Select(s => JToken.FromObject(s))));
            jo.WriteTo(writer);
        }
    }
}