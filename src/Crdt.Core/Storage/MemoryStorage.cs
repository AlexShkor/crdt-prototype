using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Crdt.Core.Data;
using Newtonsoft.Json.Linq;

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
            _updater.ListenForUpdate(ApplyUpdate);
        }

        public JObject GetDocument(string id)
        {
            var doc = GetFromStorage(id);
            return doc?.ToJson();
        }

        public JObject SaveDocument(JObject data)
        {
            var id = (data["id"] ?? Guid.NewGuid()).ToString();
            var documentData = new DocumentData() { Id = id, Entries = new Dictionary<string, DataEntry>()};
            documentData.ParseEntries(data);
            _storage.TryAdd(id, documentData);

            Send(new AddToSetCommand
            {
                CommandId = Guid.NewGuid().ToString(),
                DocumentId = id,
                Entry = data,
            });

            return documentData.ToJson();
        }

        public void UpdateDocument(AddToSetCommand cmd)
        {
            var doc = GetFromStorage(cmd.DocumentId);
            Update(doc, cmd);
            Send(cmd);
        }

        private void ApplyUpdate(AddToSetCommand cmd)
        {
            var doc = GetFromStorage(cmd.DocumentId);
            if (doc == null)
            {
                doc = new DocumentData() { Id = cmd.DocumentId, Entries = new Dictionary<string, DataEntry>() };
                doc.ParseEntries(cmd.Entry);
                _storage.TryAdd(cmd.DocumentId, doc);
            }
            else
            {
                Update(doc, cmd);
            }
        }

        private void Update(DocumentData doc, AddToSetCommand cmd)
        {
            var entry = doc.Entries[cmd.FieldName];
            var entryDoc = new DocumentData();
            entryDoc.ParseEntries(cmd.Entry);

            var array = entry as ArrayDataEntry;
            if (array != null)
            {
                array.AddItem(entryDoc);
            }
            else
            {
                var stringDataEntry = entry as StringDataEntry;
                stringDataEntry.Value = cmd.Entry.ToString();
            }
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
