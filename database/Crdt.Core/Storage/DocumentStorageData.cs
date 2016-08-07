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
        public DocumentData Document { get; set; }

        private ConcurrentDictionary<string, UpdateDocumentCommand> _updates  = 
            new ConcurrentDictionary<string, UpdateDocumentCommand>();

        private static Dictionary<string, IDataEntryProcessor> _dataEntryProcessors =
             new Dictionary<string, IDataEntryProcessor>
             {
                { "string", new StringDataEntryProcessor() }
             };

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

            var entries = Document.Entries.ToDictionary(p => p.Key, p => _dataEntryProcessors[p.Value.Type].Copy(p.Value));

            foreach(var update in _updates.Select(u => u.Value).OrderBy(u => u.DateTime).ToList())
            {
                if (!entries.ContainsKey(update.FieldName))
                {
                    entries.Add(update.FieldName, new StringDataEntry());
                }

                var entry = entries[update.FieldName];
                _dataEntryProcessors[entry.Type].Update(entry, update);
            }

            return new DocumentData()
            {
                Id = Document.Id,
                Entries = entries
            };
        }
    }
}
