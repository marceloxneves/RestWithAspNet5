using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithAspNet5.Business;
using RestWithAspNet5.Data.VO;
using RestWithAspNet5.Hypermedia.Filters;
using RestWithAspNet5.Model;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestWithAspNet5.Controllers
{
    [ApiVersion("1")]    
    [ApiController]
    //Authentication
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonBusiness _personBusiness;

        public PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness)
        {
            _logger = logger;
            _personBusiness = personBusiness;
        }

        // GET: api/<PersonController>
        [HttpGet("{sortDirection}/{pageSize}/{page}")]
        //Hateoas
        [TypeFilter(typeof(HyperMediaFilter))]
        //swagger - customização - ini
        [ProducesResponseType((200), Type = typeof(List<PersonVO>))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        //swagger - customização - fim
        public IActionResult Get([FromQuery] string name, string sortDirection, int pageSize, int page)
        {
            return Ok(_personBusiness.FindWithPagedSearch(name, sortDirection, pageSize, page));
        }

        // GET api/<PersonController>/5
        //Hateoas
        [TypeFilter(typeof(HyperMediaFilter))]
        //swagger - customização - ini
        [ProducesResponseType((200), Type = typeof(PersonVO))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        //swagger - customização - fim
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var person = _personBusiness.FindById(id);

            if (person != null)
            {
                return Ok(person);
            }

            return NotFound();
        }

        // GET api/<PersonController>/5
        [Route("FindByName")]
        //Hateoas
        [TypeFilter(typeof(HyperMediaFilter))]
        //swagger - customização - ini
        [ProducesResponseType((200), Type = typeof(PersonVO))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        [HttpGet]
        //swagger - customização - fim
        public IActionResult Get([FromQuery] string firstName, [FromQuery] string lastName)
        {
            var person = _personBusiness.FindByName(firstName, lastName);

            if (person != null)
            {
                return Ok(person);
            }

            return NotFound();
        }

        // POST api/<PersonController>
        //Hateoas
        [TypeFilter(typeof(HyperMediaFilter))]
        //swagger - customização - ini
        [ProducesResponseType((200), Type = typeof(PersonVO))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        //swagger - customização - fim
        [HttpPost]
        public IActionResult Post([FromBody] PersonVO person)
        {
            if (person != null)
            {
                return Ok(_personBusiness.Create(person));
            }

            return BadRequest();
        }

        // PUT api/<PersonController>/5
        //Hateoas
        [TypeFilter(typeof(HyperMediaFilter))]
        //swagger - customização - ini
        [ProducesResponseType((200), Type = typeof(PersonVO))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        //swagger - customização - fim
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]PersonVO person)
        {
            if (person != null)
            {
                return Ok(_personBusiness.Update(person));
            }

            return BadRequest();
        }

        // PATCH api/<PersonController>/5
        //Hateoas
        [TypeFilter(typeof(HyperMediaFilter))]
        //swagger - customização - ini
        [ProducesResponseType((200), Type = typeof(PersonVO))]
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        //swagger - customização - fim
        [HttpPatch("{id}")]
        public IActionResult Patch(long id)
        {
            var person = _personBusiness.Disable(id);

            if (person != null)
            {
                return Ok(person);
            }

            return NotFound();
        }

        // DELETE api/<PersonController>/5
        //swagger - customização - ini
        [ProducesResponseType((204))]
        [ProducesResponseType((400))]
        [ProducesResponseType((401))]
        //swagger - customização - fim
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _personBusiness.Delete(id);

            return NoContent();
        }
    }
}
