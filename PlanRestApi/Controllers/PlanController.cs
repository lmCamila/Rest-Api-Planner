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
    public class PlanController : Controller
    {
        private readonly PlanRepository _planRepository;
        public PlanController(IConfiguration configuration)
        {
            _planRepository = new PlanRepository(configuration);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Recupera TODOS os Planos.",
                          Tags = new[] { "Plan" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 200, Type = typeof(List<Plan>))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult GetAll()
        {
            var list = _planRepository.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Recupera Plano identificado por seu {id}.",
                          Tags = new[] { "Plan" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 200, Type = typeof(Plan))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult Get([FromRoute][SwaggerParameter("Id do plano que será obtido.")] int id)
        {
            var model = _planRepository.Get(id);
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Insere um novo plano..",
                          Tags = new[] { "Plan" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 201, Type = typeof(Plan))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 400)]
        public IActionResult Insert(
                [Bind("Name,Sponsor,Type,Status,StartDate,EndDate,Description,Cost,Interested")]
                PlanUpload plan)
        {
            if (ModelState.IsValid)
            {
                var result = _planRepository.Insert(plan);
                var lastType = result ? _planRepository.GetLastInserted() : null;
                var uri = Url.Action("Get", new { id = lastType.Id, Version = "1.0" });
                return Created(uri, lastType);
            }
            return BadRequest();
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Altera um plano.",
                          Tags = new[] { "Plan" })]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 400)]
        public IActionResult Update([Bind("Id,Name,Sponsor,Type,Status,StartDate,EndDate,Description,Cost,Interested")]
                PlanUpload plan)
        {
            if (ModelState.IsValid)
            {
                _planRepository.Update(plan);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um plano.",
                          Tags = new[] { "Plan" })]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult Delete([FromRoute]int id)
        {
            var model = _planRepository.Get(id);
            if(model == null)
            {
                return NotFound();
            }
            _planRepository.Delete(id);
            return NoContent();
        }
    }
}