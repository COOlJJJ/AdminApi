using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace AdminApi.Controllers
{
    [Route("api/User")]
    [ApiController]
    [Authorize(Policy = "userOradmin")]
    public class UserController : ControllerBase
    {
        private readonly ILogger _loggger;

        public UserController(ILogger logger)
        {
            _loggger = logger;
        }

        [Route("Get")]
        [Authorize(Policy = "admin")]
        [HttpGet]
        public string Get()
        {
            return SHA256Encryption.SHA256Hash("123456");
        }

        [Route("HelloWorld")]
        [Authorize(Policy = "user")]
        [HttpGet]
        public string HelloWorld()
        {
            return "HelloWorld";
        }


    }
}

