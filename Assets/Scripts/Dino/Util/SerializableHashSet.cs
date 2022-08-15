using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dino.Util
{
    [Serializable]
    public class SerializableHashSet<T> : HashSet<T>
    {
        public SerializableHashSet() : base() {}
        [JsonConstructor]
        public SerializableHashSet(IEnumerable<T> collection) : base(collection) {}
    }
}