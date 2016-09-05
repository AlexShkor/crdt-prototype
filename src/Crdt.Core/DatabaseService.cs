using Newtonsoft.Json.Linq;
using System;

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
                 Entry = JObject.Parse(item),
                 CommandId = Guid.NewGuid().ToString()
            });
        }

        public string CreateDocument(string json)
        {
            var jo = JObject.Parse(json);
            var doc = _storage.SaveDocument(jo);
            return doc.ToString();      
        }

        public string GetDocument(string id)
        {
            return _storage.GetDocument(id).ToString();
        }
    }
}
