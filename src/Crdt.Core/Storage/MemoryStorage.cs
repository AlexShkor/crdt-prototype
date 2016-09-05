using Crdt.Core.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crdt.Core.Storage
{
    public class MemoryStorage : IStorage
    {
        private readonly ConcurrentDictionary<string, DocumentData> _storage =
            new ConcurrentDictionary<string, DocumentData>();
        
        private readonly IReplicasUpdater _updater;

        public MemoryStorage(IReplicasUpdater updater)
        {
            _updater = updater;
            _updater.ListenForUpdate(this.UpdateDocument);
        }

        public JObject GetDocument(string id)
        {
            var doc = GetFromStorage(id);
            return doc.ToJson();
        }

        public JObject SaveDocument(JObject data)
        {
            var id = data["id"].ToString() ?? Guid.NewGuid().ToString();
            var documentData = new DocumentData() { Id = id, Entries = new Dictionary<string, DataEntry>()};
            documentData.ParseEntries(data);
            _storage.TryAdd(id, documentData);
            return documentData.ToJson();
        }

        public void UpdateDocument(AddToSetCommand cmd)
        {
            var doc = GetFromStorage(cmd.DocumentId);
            var entry = doc.Entries[cmd.FieldName];
            var array = entry as ArrayDataEntry;
            var entryDoc = new DocumentData();
            entryDoc.ParseEntries(cmd.Entry);
            array.AddItem(entryDoc);
        }

        private DocumentData GetFromStorage(string id)
        {
            DocumentData doc = null;
            _storage.TryGetValue(id, out doc);
            return doc;
        }

        private void Send(UpdateDocumentCommand cmd)
        {
            _updater.SendUpdate(cmd);
        }
    }
}
