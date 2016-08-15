using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crdt.Core.Storage
{
    public class StringDataEntryProcessor : IDataEntryProcessor
    {
        public DataEntry Copy(DataEntry dataEntry)
        {
            var stringDataEntry = (StringDataEntry)dataEntry;
            return new StringDataEntry()
            {
                Name = stringDataEntry.Name,
                Value = stringDataEntry.Value,
            };
        }

        public void Update(DataEntry dataEntry, UpdateDocumentCommand update)
        {
            var stringDataEntry = (StringDataEntry)dataEntry;
            throw new NotImplementedException();
            //stringDataEntry.Value = update.
        }
    }
}
