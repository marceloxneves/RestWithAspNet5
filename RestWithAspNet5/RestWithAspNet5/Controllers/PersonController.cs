using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithAspNet5.Model;
using RestWithAspNet5.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestWithAspNet5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _personService;

        public PersonController(ILogger<PersonController> logger, IPersonService personService)
        {
            _logger = logger;
            _personService = personService;
        }

        // GET: api/<PersonController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_personService.FindAll());
        }

        // GET api/<PersonController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var person = _personService.FindById(id);

            if (person != null)
            {
                return Ok(person);
            }

            return NotFound();
        }

        // POST api/<PersonController>
        [HttpPost]
        public IActionResult Post([FromBody] Person person)
        {
            if (person != null)
            {
                return Ok(_personService.Create(person));
            }

            return BadRequest();
        }

        // PUT api/<PersonController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Person person)
        {
            if (person != null)
            {
                return Ok(_personService.Update(person));
            }

            return BadRequest();
        }

        // DELETE api/<PersonController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _personService.Delete(id);

            return NoContent();
        }
    }
}
