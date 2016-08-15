using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crdt.Core
{
    public interface IStorage
    {
        void SaveDocument(DocumentData data);
        void UpdateDocument(UpdateDocumentCommand cmd);
        DocumentData GetDocument(string id);
    }
}
