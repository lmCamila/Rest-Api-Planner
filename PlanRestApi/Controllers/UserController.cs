using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlanRestApi.Models;
using PlanRestApi.Repositories;

namespace PlanRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public UserController(IConfiguration configuration)
        {
            _userRepository = new UserRepository(configuration);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _userRepository.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            var model = _userRepository.Get(id);
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        public IActionResult Insert([Bind("Name")] User user)
        {
            if (ModelState.IsValid)
            {
                var result = _userRepository.Insert(user);
                var lastUser = result ? _userRepository.GetLastInserted() : null;
                var uri = Url.Action("Get", new { id = lastUser.Id });
                return Created(uri, lastUser);
            }
            return BadRequest();
        }

        [HttpPut]
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