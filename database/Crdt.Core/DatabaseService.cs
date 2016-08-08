using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Json;
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
            
        }

        public string CreateDocument(string json)
        {
            var jo = JsonValue.Parse(json);
            _storage.SaveDocument(new DocumentData
            {
                 Id = jo["id"].ToString() ?? Guid.NewGuid().ToString(),
                  Entries = request.Fields.Select(x=> new DataEntry()
            })
        }

        public string GetDocument(string id)
        {

        }
    }
}
