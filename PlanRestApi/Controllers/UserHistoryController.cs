using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlanRestApi.Repositories;

namespace PlanRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserHistoryController : Controller
    {
        private readonly UserHistoryRepository _uhRepository;
        public UserHistoryController(IConfiguration configuration)
        {
            _uhRepository = new UserHistoryRepository(configuration);
        }
        [HttpGet]
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
        public IActionResult Get([FromRoute]int id)
        {
            var model = _uhRepository.GetAll(id);
            if(model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }
    }
}