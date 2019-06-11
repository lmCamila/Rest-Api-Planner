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
    public class TypePlanController : ControllerBase
    {
        private readonly TypePlanRepository _typePlanRepository;
        public TypePlanController(IConfiguration configuration)
        {
            _typePlanRepository = new TypePlanRepository(configuration);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Recupera TODOS os Tipos de Planos.",
                          Tags = new[] { "TypePlan" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 200, Type = typeof(List<TypePlan>))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult GetAll()
        {
            var list = _typePlanRepository.GetAll();
            return Ok(list);
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Recupera um Tipo de Plano identificado por seu {id}.",
                          Tags = new[] { "TypePlan" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 200, Type = typeof(TypePlan))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult Get([FromRoute][SwaggerParameter("Id do tipo de plano que será obtido.")] int id)
        {
            var model = _typePlanRepository.Get(id);
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Insere um novo tipo de plano..",
                          Tags = new[] { "TypePlan" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 201, Type = typeof(TypePlan))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 400)]
        public IActionResult Insert([Bind("Name")]TypePlan type)
        {
            if (ModelState.IsValid)
            {
                var result = _typePlanRepository.Insert(type);
                var lastType = result ? _typePlanRepository.GetLastInserted() : null;
                var uri = Url.Action("Get", new { id = lastType.Id, Version = "1.0" });
                return Created(uri, lastType);
            }
            return BadRequest();
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Altera um tipo de plano.",
                          Tags = new[] { "TypePlan" })]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 400)]
        public IActionResult Update([Bind("Id,Name")]TypePlan type)
        {
            if (ModelState.IsValid)
            {
                _typePlanRepository.Update(type);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um tipo de plano.",
                          Tags = new[] { "TypePlan" })]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult Delete(int id)
        {
            var model = _typePlanRepository.Get(id);
            if(model == null)
            {
                return NotFound();
            }
            _typePlanRepository.Delete(id);
            return NoContent();
        }
    }
}