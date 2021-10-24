using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithAspNet5.Authentication.Services;
using RestWithAspNet5.Authentication.VO;

namespace RestWithAspNet5.Authentication.Controllers
{
    [ApiVersion("1")]    
    [Route("api/[controller]/v{Version:apiVersion}")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [Route("signin")]
        [HttpPost]
        public IActionResult Signin([FromBody] UserVO user)
        {
            if (user == null) return BadRequest("Invalid Client Request");

            var token = _loginService.ValidateCredentials(user);

            if (token == null) return Unauthorized();

            return Ok(token);
        }

        [Route("refresh")]
        [HttpPost]
        public IActionResult Refresh([FromBody] TokenVO tokenVo)
        {
            if (tokenVo == null) return BadRequest("Invalid Client Request");

            var token = _loginService.ValidateCredentials(tokenVo);

            if (token == null) return BadRequest("Invalid Client Request");

            return Ok(token);
        }

        //Authentication
        [Authorize("Bearer")]
        [Route("revoke")]
        [HttpGet]
        //Loggof
        public IActionResult Revoke()
        {
            //Pelo Bearer o .net já sabe quem é o usuário
            var username = User.Identity.Name;

            var result = _loginService.RevokeToken(username);

            if (!result) return BadRequest("Invalid Client Request");

            return NoContent();
        }
    }
}
