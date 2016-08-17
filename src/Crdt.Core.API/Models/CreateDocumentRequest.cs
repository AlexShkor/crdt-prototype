using System.ComponentModel.DataAnnotations;

namespace Crdt.Core.API.Models
{
    public class CreateDocumentRequest
    {
        public string JsonDocument {get;set;}
    }
}