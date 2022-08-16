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
            return new HashSet<T>(JArray.Load(reader).Values<T>());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var hashSet = (HashSet<T>) value;
            var jArray = new JArray(hashSet.Select(s => JToken.FromObject(s)));
            jArray.WriteTo(writer);
        }
    }
}