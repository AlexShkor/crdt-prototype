using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crdt.Core
{
    public interface IStorage
    {
        JObject SaveDocument(JObject data);
        void UpdateDocument(UpdateDocumentCommand cmd);
        JObject GetDocument(string id);
    }
}
