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
    public class PlanController : Controller
    {
        private readonly PlanRepository _planRepository;
        public PlanController(IConfiguration configuration)
        {
            _planRepository = new PlanRepository(configuration);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _planRepository.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var model = _planRepository.Get(id);
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        public IActionResult Insert(
                [Bind("Name,Sponsor,Type,Status,StartDate,EndDate,Description,Cost,Interested")]
                PlanUpload plan)
        {
            if (ModelState.IsValid)
            {
                var result = _planRepository.Insert(plan);
                var lastType = result ? _planRepository.GetLastInserted() : null;
                var uri = Url.Action("Get", new { id = lastType.Id });
                return Created(uri, lastType);
            }
            return BadRequest();
        }

        [HttpPut]
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