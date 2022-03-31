using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;


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


    }
}

