using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger _loggger;

        public UserController(ILogger logger)
        {
            _loggger = logger;
        }

        // GET: api/values
        [HttpGet]
        public string Get()
        {

            try
            {
                return SHA256Encryption.SHA256Hash("123456");
            }
            catch (Exception ex)
            {
                _loggger.Debug("This is index -- debug.");
                _loggger.Information($"‘{typeof(UserController)}’ This is index -- information.");
                _loggger.Warning("This is index -- warning.");
                _loggger.Error("This is index -- error." + ex.ToString());
                return SHA256Encryption.SHA256Hash("123456");
            }
        }



        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

    }
}

