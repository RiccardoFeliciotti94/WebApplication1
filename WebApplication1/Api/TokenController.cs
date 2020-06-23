using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplication1.Api.Services;

namespace WebApplication1.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
      
        private readonly IConfiguration _config;
        private readonly IJwtService _jwtService;

        public TokenController(IConfiguration config, IJwtService jwtService)
        {
            _jwtService = jwtService;
            _config = config;
        }

        [HttpGet]
        public string GetRandomToken()
        {  /*
            var jwt = new JwtService(_config);
            var token = jwt.GenerateSecurityToken();
            return token;*/
            return _jwtService.GenerateSecurityToken();
        }
    }
}
