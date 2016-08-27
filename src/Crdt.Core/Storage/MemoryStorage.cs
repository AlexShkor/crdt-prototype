using Newtonsoft.Json.Linq;
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

        private IReadOnlyDictionary<string, IDataEntryProcessor> _columnModel;

        public MemoryStorage(IReadOnlyDictionary<string, IDataEntryProcessor> columnModel)
        {
            if (columnModel == null)
            {
                throw new ArgumentNullException(nameof(columnModel));
            }

            _columnModel = columnModel;
        }

        public JObject GetDocument(string id)
        {
            DocumentStorageData documentStorage;
            var doc = _storage.TryGetValue(id, out documentStorage) ? documentStorage.CalculateDocument() : null;
            var jo = new JObject();
            if (doc != null)
            {
                foreach (var entry in doc.Entries)
                {

                }
            }
            return jo;
        }

        public JObject SaveDocument(JObject data)
        {
            _storage.AddOrUpdate(data.Id, 
                new DocumentStorageData(_columnModel) { Document = data }, 
                (key, storData) =>
                {
                    storData.Document = data;
                    return storData;
                });
        }

        public void UpdateDocument(UpdateDocumentCommand cmd)
        {
            _storage.AddOrUpdate(cmd.DocumentId, 
                new DocumentStorageData(_columnModel),
                (key, storData) =>
                {
                    storData.Update(cmd);
                    return storData;
                });
        }
    }
}
