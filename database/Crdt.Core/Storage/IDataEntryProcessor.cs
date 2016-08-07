using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crdt.Core.Storage
{
    public interface IDataEntryProcessor
    {
        DataEntry Copy(DataEntry dataEntry);

        void Update(DataEntry dataEntry, UpdateDocumentCommand update);
    }
}
