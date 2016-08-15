using System.Collections.Generic;

namespace Crdt.Core
{
    public class CreateDocumentRequest
    {
        public string Id { get; set; }

        public List<FieldDescriptionContract> Fields { get; set; }
    }
}