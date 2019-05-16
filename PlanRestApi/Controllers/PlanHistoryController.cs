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

    public class PlanHistoryController : Controller
    {
        private readonly PlanHistoryRepository _phRepository;
        private readonly PlanRepository _planRepository;
        public PlanHistoryController(IConfiguration configuration)
        {
            _phRepository = new PlanHistoryRepository(configuration);
            _planRepository = new PlanRepository(configuration);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Recupera TODOS os históricos de TODOS os planos",
                          Tags = new[] { "PlanHistory" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 200, Type = typeof(List<PlanHistory>))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult GetAll()
        {
            var model = _phRepository.GetAll();
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Recupera o histórico de um plano identificado por seu {id}",
                          Tags = new[] { "PlanHistory" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 200, Type = typeof(List<PlanHistoryConfig>))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult Get([FromRoute][SwaggerParameter("Id do plano do qual será obtido o histórico")] int id)
        {
            var model = _phRepository.GetAll(id).Select(l => l).ToPlanHistoryConfig(_planRepository.Get(id));
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }
    }
}