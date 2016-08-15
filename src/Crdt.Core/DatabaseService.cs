using Newtonsoft.Json.Linq;

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
                 //TODO need attention
                 Entry = new DocumentData()
            });
        }

        //TODO Need attention
        public string CreateDocument(string json)
        {
            var jo = JObject.Parse(json);
            _storage.SaveDocument(new DocumentData());
            return jo.ToString();      
        }

        public string GetDocument(string id)
        {
            return _storage.GetDocument(id).ToString();
        }
    }
}
