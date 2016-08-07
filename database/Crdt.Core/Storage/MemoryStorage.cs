using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crdt.Core.Storage
{
    public class MemoryStorage : IStorage
    {
        private ConcurrentDictionary<string, DocumentStorageData> _storage = 
            new ConcurrentDictionary<string, DocumentStorageData>();        

        public DocumentData GetDocument(string id)
        {
            DocumentStorageData documentStorage = null;
            return _storage.TryGetValue(id, out documentStorage) ? documentStorage.CalculateDocument() : null;
        }

        public void SaveDocument(DocumentData data)
        {
            _storage.AddOrUpdate(data.Id, 
                new DocumentStorageData() { Document = data }, 
                (key, storData) =>
                {
                    storData.Document = data;
                    return storData;
                });
        }

        public void UpdateDocument(UpdateDocumentCommand cmd)
        {
            _storage.AddOrUpdate(cmd.DocumentId, 
                new DocumentStorageData(),
                (key, storData) =>
                {
                    storData.Update(cmd);
                    return storData;
                });
        }
    }
}
