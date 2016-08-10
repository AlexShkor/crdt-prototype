using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Crdt.Core
{
    public class DatabaseService : IDatabaseService
    {
        private IStorage _storage;
        
        public DatabaseService(IStorage storage)
        {
            _storage = storage;
        }

        public void AddToEmbededCollection(string id, string prop, string item)
        {
            _storage.UpdateDocument(new AddToSetCommand
            {
                 DocumentId = id,
                 FieldName = prop,
                 Entry = JsonValue.Parse(item).ToJsonObject()
            });
        }

        public string CreateDocument(string json)
        {
            var jo = JsonValue.Parse(json).ToJsonObject();
            var result = _storage.SaveDocument(jo);
            return jo.ToString();      
        }

        public string GetDocument(string id)
        {
            return _storage.GetDocument(id).ToString();
        }
    }
}
