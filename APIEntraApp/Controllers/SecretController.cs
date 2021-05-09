using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIEntraApp.Controllers
{
    [ApiController]
    public class SecretController : ControllerBase
    {
        [Authorize]
        [Route("/secret")]
        public IActionResult Secret() 
        {
            return Ok(new ResponseResult { Result = "Secret message from API" });
        }

        [AllowAnonymous]
        [Route("/opened")]
        public IActionResult Opened()
        {
            return Ok(new ResponseResult { Result = "Open message from API" });
        }
    }

    public class ResponseResult 
    {
        public string Result { get; set; }
    }
}
