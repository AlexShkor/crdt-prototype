using Crdt.Core;
using Crdt.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crdt.Server.Controllers
{
    [Route("documents")]
    public class DocumentsController : Controller 
    {
        private readonly IDatabaseService _databaseService;

        public DocumentsController(IDatabaseService databaseService){
            _databaseService = databaseService;
        }

        [HttpPost("create")]
        public IActionResult CreateDocument([FromBody]string document){
            _databaseService.CreateDocument(document);
            return Ok();
        }

        [HttpPost("{id}")]
        public IActionResult GetDocument(string id){
            return Ok(_databaseService.GetDocument(id));
        }

        [HttpPost("addToDocument")]
        public IActionResult AddToEmbededCollection([FromBody]AddToEmbededDocumentModel model){
            _databaseService.AddToEmbededCollection(model.Id, model.Field, model.Body);
            return Ok();
        }
    }
}