using Newtonsoft.Json.Linq;
using System;

namespace Crdt.Core
{
    public abstract class UpdateDocumentCommand
    {
        public string CommandId { get; set; }

        public DateTime DateTime { get; set; }

        public string DocumentId { get; set; }

        public string FieldName { get; set; }
    }

    public class AddToSetCommand : UpdateDocumentCommand
    {
        public JObject Entry { get; set; }
    }

    public interface ICommandProcessor<T> where T : UpdateDocumentCommand
    {
        void Process(T cmd, DocumentData target);
    }

    public class AddToSetCommandProcessor : ICommandProcessor<AddToSetCommand>
    {
        public void Process(AddToSetCommand cmd, DocumentData target)
        {
            throw new NotImplementedException();
        }
    }
}