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
        private readonly ConcurrentDictionary<string, DocumentStorageData> _storage =
            new ConcurrentDictionary<string, DocumentStorageData>();

        private readonly IReadOnlyDictionary<string, IDataEntryProcessor> _columnModel;
        private readonly IReplicasUpdater _updater;

        public MemoryStorage(IReadOnlyDictionary<string, IDataEntryProcessor> columnModel, IReplicasUpdater updater)
        {
            if (columnModel == null) throw new ArgumentNullException(nameof(columnModel));

            _columnModel = columnModel;
            _updater = updater;
            _updater.ListenForUpdate(this.Update);
        }

        public JObject GetDocument(string id)
        {
            DocumentStorageData documentStorage;
            var documentData = _storage.TryGetValue(id, out documentStorage) ? documentStorage.CalculateDocument() : null;
            return ConvertDocumentDataToJson(documentData);
        }

        public JObject SaveDocument(JObject data)
        {
            var _id = Guid.NewGuid().ToString();

            var documentData = new DocumentData() { Id = _id };
            foreach (var field in data)
            {
                documentData.Entries.Add(field.Key, new StringDataEntry() { Name = field.Key, Value = field.Value.ToString() });
            }

            _storage.AddOrUpdate(_id,
               new DocumentStorageData(_columnModel) { Document = documentData },
               (key, storedData) => storedData);

            return ConvertDocumentDataToJson(documentData);
        }

        public void UpdateDocument(UpdateDocumentCommand cmd)
        {
            Update(cmd);
            Send(cmd);
        }

        private void Update(UpdateDocumentCommand cmd)
        {
            _storage.AddOrUpdate(cmd.DocumentId,
                new DocumentStorageData(_columnModel),
                (key, storedData) =>
                {
                    storedData.Update(cmd);
                    return storedData;
                });
        }

        private void Send(UpdateDocumentCommand cmd)
        {
            _updater.SendUpdate(cmd);
        }


        private JObject ConvertDocumentDataToJson(DocumentData data)
        {
            var jo = new JObject();
            if (data != null)
            {
                foreach (var entry in data.Entries)
                {
                    var stringDataEntry = entry.Value as StringDataEntry;

                    if (stringDataEntry != null)
                        jo[entry.Value.Name] = stringDataEntry.Value;
                }
            }
            return jo;
        }
    }
}
