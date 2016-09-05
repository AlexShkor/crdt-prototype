using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crdt.Core.Data
{
    public class ArrayDataEntry : DataEntry
    {
        public override JTokenType Type
        {
            get
            {
                return JTokenType.Array;
            }
        }

        public IList<DocumentData> Items { get; private set; }

        public ArrayDataEntry()
        {
            Items = new List<DocumentData>();
        }

        public void AddItem(DocumentData item)
        {
            item.Id = Items.Count.ToString();
            Items.Add(item);
        }

        public override JToken ToJToken()
        {
            JArray jArray = new JArray();
            foreach (var item in Items)
            {
                var jObject = item.ToJson();
                jObject.Remove("id");
                jArray.Add(jObject);
            }
            return jArray;
        }
    }
}
