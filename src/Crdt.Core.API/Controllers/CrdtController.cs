using Crdt.Core;
using Crdt.Core.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Crdt.Core.API.Controllers
{
    [Route("documents")]
    public class DocumentsController : Controller 
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger _logger;

        public DocumentsController(IDatabaseService databaseService, ILoggerFactory loggerFactory)
        {
            // TODO: Fix DI for IDatabaseService databaseService
            _databaseService = databaseService;
            _logger = loggerFactory.CreateLogger<DocumentsController>();
        }

        [HttpPost("create")]
        public IActionResult CreateDocument([FromBody] Crdt.Core.API.Models.CreateDocumentRequest request)
        {
            _logger.LogInformation("JsonDocument is " + request.JsonDocument);
            _databaseService.CreateDocument(request.JsonDocument);
            return Ok(/* return created document id here */);
        }

        [HttpPost("get")]
        public IActionResult GetDocument([FromBody] Crdt.Core.API.Models.GetDocumentRequest request)
        {
            _logger.LogInformation("Id is " + request.Id);
            return Ok(_databaseService.GetDocument(request.Id));
        }

        [HttpPost("add")]
        public IActionResult AddToEmbededCollection([FromBody] Crdt.Core.API.Models.AddToEmbededCollectionRequest request)
        {
            _logger.LogInformation("Id is " + request.Id + ", Field is " + request.Field + ", JsonItem is " + request.JsonItem);
            _databaseService.AddToEmbededCollection(request.Id, request.Field, request.JsonItem);
            return Ok();
        }
    }
}