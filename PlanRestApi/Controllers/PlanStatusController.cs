using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlanRestApi.Models;
using PlanRestApi.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace PlanRestApi.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PlanStatusController : ControllerBase
    {
        private readonly PlanStatusRepository _planStatusRepository;
        public PlanStatusController(IConfiguration configuration)
        {
            _planStatusRepository = new PlanStatusRepository(configuration);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Recupera TODOS os Status de Planos.",
                          Tags = new[] { "PlanStatus" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 200, Type = typeof(List<PlanStatus>))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult GetAll()
        {
            var list = _planStatusRepository.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Recupera um Status de Plano identificado por seu {id}.",
                          Tags = new[] { "PlanStatus" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 200, Type = typeof(PlanStatus))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult Get([FromRoute][SwaggerParameter("Id do status de plano que será obtido.")] int id)
        {
            var model = _planStatusRepository.Get(id);
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Insere um novo Status de plano..",
                          Tags = new[] { "PlanStatus" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 201, Type = typeof(PlanStatus))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 400)]
        public IActionResult Insert([Bind("Name")] PlanStatus status)
        {
            if (ModelState.IsValid)
            {
                var result = _planStatusRepository.Insert(status);
                var lastPlan = result ? _planStatusRepository.GetLastInserted() : null;
                var uri = Url.Action("Get", new { Id = lastPlan.Id , Version = "1.0"});
                return Created(uri, lastPlan);
            }
            return BadRequest();
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Altera um status de plano.",
                          Tags = new[] { "PlanStatus" })]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 400)]
        public IActionResult Update([Bind("Id,Name")] PlanStatus status)
        {
            if (ModelState.IsValid)
            {
                _planStatusRepository.Update(status);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um status de plano.",
                          Tags = new[] { "PlanStatus" })]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult Delete(int id)
        {
            var model = _planStatusRepository.Get(id);
            if(model == null)
            {
                return NotFound();
            }
            _planStatusRepository.Delete(id);
            return NoContent();
        }
    }
}