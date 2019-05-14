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
    public class PlanStatusController : ControllerBase
    {
        private readonly PlanStatusRepository _planStatusRepository;
        public PlanStatusController(IConfiguration configuration)
        {
            _planStatusRepository = new PlanStatusRepository(configuration);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _planStatusRepository.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var model = _planStatusRepository.Get(id);
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        public IActionResult Insert([Bind("Name")] PlanStatus status)
        {
            if (ModelState.IsValid)
            {
                var result = _planStatusRepository.Insert(status);
                var lastPlan = result ? _planStatusRepository.GetLastInserted() : null;
                var uri = Url.Action("Get", new { Id = lastPlan.Id });
                return Created(uri, lastPlan);
            }
            return BadRequest();
        }

        [HttpPut]
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