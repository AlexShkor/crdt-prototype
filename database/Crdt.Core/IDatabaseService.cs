using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crdt.Core
{
    public interface IDatabaseService
    {
        void CreateDocument(CreateDocumentRequest request);
        string GetDocument();
        void AddToEmbededCollection();
    }
}
