using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestWithAspNet5.Business;
using RestWithAspNet5.Data.VO;
using RestWithAspNet5.Hypermedia.Filters;
using RestWithAspNet5.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestWithAspNet5.Controllers
{
    [ApiVersion("1")]
    [Route("api/[controller]/v{version:apiVersion}")]
    [ApiController]
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
        [HttpGet]
        //Hateoas
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return Ok(_personBusiness.FindAll());
        }

        // GET api/<PersonController>/5
        //Hateoas
        [TypeFilter(typeof(HyperMediaFilter))]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var person = _personBusiness.FindById(id);

            if (person != null)
            {
                return Ok(person);
            }

            return NotFound();
        }

        // POST api/<PersonController>
        //Hateoas
        [TypeFilter(typeof(HyperMediaFilter))]
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
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]PersonVO person)
        {
            if (person != null)
            {
                return Ok(_personBusiness.Update(person));
            }

            return BadRequest();
        }

        // DELETE api/<PersonController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _personBusiness.Delete(id);

            return NoContent();
        }
    }
}
