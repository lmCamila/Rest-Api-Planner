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
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public UserController(IConfiguration configuration)
        {
            _userRepository = new UserRepository(configuration);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Recupera TODOS os Usuários.",
                          Tags = new[] { "User" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 200, Type = typeof(List<User>))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult GetAll()
        {
            var list = _userRepository.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Recupera um usuário identificado por seu {id}.",
                          Tags = new[] { "User" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 200, Type = typeof(User))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult Get([FromRoute][SwaggerParameter("Id do usuário que será obtido.")] int id)
        {
            var model = _userRepository.Get(id);
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Insere um novo usuário..",
                          Tags = new[] { "User" },
                          Produces = new[] { "application/json" })]
        [ProducesResponseType(statusCode: 201, Type = typeof(User))]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 400)]
        public IActionResult Insert([Bind("Name")] User user)
        {
            if (ModelState.IsValid)
            {
                var result = _userRepository.Insert(user);
                var lastUser = result ? _userRepository.GetLastInserted() : null;
                var uri = Url.Action("Get", new { id = lastUser.Id, Version = "1.0" });
                return Created(uri, lastUser);
            }
            return BadRequest();
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Altera um usuário.",
                          Tags = new[] { "User" })]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 400)]
        public IActionResult Update([Bind("Id,Name")] User user)
        {
            if (ModelState.IsValid)
            {
                _userRepository.Update(user);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um usuário.",
                          Tags = new[] { "User" })]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 500, Type = typeof(ErrorResponse))]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult Delete([FromRoute]int id)
        {
            var model = _userRepository.Get(id);
            if(model == null)
            {
                return NotFound();
            }
            _userRepository.Delete(id);
            return NoContent();
        }
    }
}