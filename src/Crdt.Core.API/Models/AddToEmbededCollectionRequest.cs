using System.ComponentModel.DataAnnotations;

namespace Crdt.Core.API.Models
{
    public class AddToEmbededCollectionRequest
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Field { get; set; }
        [Required]
        public string JsonItem { get; set; }
    }
}