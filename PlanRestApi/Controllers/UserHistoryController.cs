﻿using System;
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
    public class UserHistoryController : Controller
    {
        private readonly UserHistoryRepository _uhRepository;
        private readonly UserRepository _userRepository;
        public UserHistoryController(IConfiguration configuration)
        {
            _uhRepository = new UserHistoryRepository(configuration);
            _userRepository = new UserRepository(configuration);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Recupera TODOS os históricos de TODOS os usuários",
                          Tags =new[] {"UserHistory"},
                          Produces = new[] {"application/json"})]
        [ProducesResponseType(statusCode: 200, Type = typeof(List<UserHistory>))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult GetAll()
        {
            var model = _uhRepository.GetAll();
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Recupera o histórico de um usuário identificado por seu {id}",
                          Tags = new[] { "UserHistory" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 200, Type = typeof(List<UserHistoryConfig>))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult Get([FromRoute][SwaggerParameter("Id do usuário do qual será obtido o histórico")]int id)
        {
            var model = _uhRepository.GetAll(id).Select(l => l).ToUserHistoryConfig(_userRepository.Get(id));
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }
    }
}