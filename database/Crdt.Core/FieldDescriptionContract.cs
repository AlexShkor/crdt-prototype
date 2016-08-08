using System.Collections.Generic;

namespace Crdt.Core
{
    public class FieldDescriptionContract
    {
        public string Name { get; set; }

        public List<FieldDescriptionContract> Fields { get; set; }
    }
}