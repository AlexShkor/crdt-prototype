using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Crdt.Core.Messaging.Utils
{
    public class ObjectHolder
    {
        public string SerializedObject { get; set; }
        public string SerializeHints { get; set; }
    }

    public static class Serializer
    {
        public static object DeserializeFromString(string objectData)
        {
            ObjectHolder holder = JsonConvert.DeserializeObject<ObjectHolder>(objectData);
            Type objType = Type.GetType(holder.SerializeHints);
            var obj = JsonConvert.DeserializeObject(holder.SerializedObject, objType);
            return obj;
        }

        public static string SerializeToString(object obj)
        {
            string sobj = JsonConvert.SerializeObject(obj);

            string holder = JsonConvert.SerializeObject(new ObjectHolder
            {
                SerializedObject = sobj,
                SerializeHints = obj.GetType().ToString()
            });

            return holder;
        }

    }
}
