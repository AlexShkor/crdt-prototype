using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crdt.Core
{
    public interface IDatabaseService
    {
        string CreateDocument(string json);
        string GetDocument(string id);
        void AddToEmbededCollection(string id, string field, string itemJson);
    }
}
