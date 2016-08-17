using System.ComponentModel.DataAnnotations;

namespace Crdt.Core.API.Models
{
    public class GetDocumentRequest
    {
        [Required]
        public string Id {get;set;}
    }
}