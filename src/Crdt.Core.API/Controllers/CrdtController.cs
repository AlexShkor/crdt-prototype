using Crdt.Core;
using Crdt.Core.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Crdt.Core.API.Controllers
{
    [Route("documents")]
    public class DocumentsController : Controller 
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger _logger;

        public DocumentsController(IDatabaseService databaseService, ILoggerFactory loggerFactory)
        {
            _databaseService = databaseService;
            _logger = loggerFactory.CreateLogger<DocumentsController>();
        }

        [HttpPost("create")]
        public IActionResult CreateDocument([FromBody] CreateDocumentRequest request)
        {
            _logger.LogInformation("JsonDocument is " + request.JsonDocument);
            return Ok(_databaseService.CreateDocument(request.JsonDocument));
        }

        [HttpPost("get")]
        public IActionResult GetDocument([FromBody] GetDocumentRequest request)
        {
            _logger.LogInformation("Id is " + request.Id);
            return Ok(_databaseService.GetDocument(request.Id));
        }

        [HttpPost("add")]
        public IActionResult AddToEmbededCollection([FromBody] AddToEmbededCollectionRequest request)
        {
            _logger.LogInformation("Id is " + request.Id + ", Field is " + request.Field + ", JsonItem is " + request.JsonItem);
            _databaseService.AddToEmbededCollection(request.Id, request.Field, request.JsonItem);
            return Ok();
        }
    }
}