using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crdt.Core.Storage
{
    internal class DocumentStorageData
    {
        public DocumentStorageData(IReadOnlyDictionary<string, IDataEntryProcessor> columnModel)
        {
            if (columnModel == null)
            {
                throw new ArgumentNullException(nameof(columnModel));
            }

            _columnModel = columnModel;
        }

        public DocumentData Document
        {
            get
            {
                return _document;
            }
            set
            {
                CheckColumnModel(value);
                _document = value;
            }
        }

        private readonly ConcurrentDictionary<string, UpdateDocumentCommand> _updates  = 
            new ConcurrentDictionary<string, UpdateDocumentCommand>();

        private readonly IReadOnlyDictionary<string, IDataEntryProcessor> _columnModel;

        private DocumentData _document;

        public void Update(UpdateDocumentCommand update)
        {
            _updates.TryAdd(update.CommandId, update);
        }

        public DocumentData CalculateDocument()
        {
            if(Document == null)
            {
                return null;
            }

            var entries = CopyDocumentEntries();
            var orderedUpdates = _updates.Select(u => u.Value).OrderBy(u => u.DateTime).ToList();
            foreach (var update in orderedUpdates)
            {
                if (!entries.ContainsKey(update.FieldName))
                {
                    entries.Add(update.FieldName, new StringDataEntry());
                }

                var entry = entries[update.FieldName];
                _columnModel[update.FieldName].Update(entry, update);
            }

            return new DocumentData()
            {
                Id = Document.Id,
                Entries = entries
            };
        }

        private void CheckColumnModel(DocumentData document)
        {
            var missedColumns = _columnModel.Where(c => !document.Entries.ContainsKey(c.Key));
            if (missedColumns.Any())
            {
                throw new FormatException();
            }
        }

        private Dictionary<string, DataEntry> CopyDocumentEntries()
        {
            return Document.Entries.ToDictionary(p => p.Key, p => _columnModel[p.Key].Copy(p.Value));
        }
    }
}
