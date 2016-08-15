using System.ComponentModel.DataAnnotations;

namespace Crdt.Server.Models{
    public class  AddToEmbededDocumentModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Field { get; set; }
        [Required]
        public string Body { get; set; }
    }
}