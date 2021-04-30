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
        [Route("/secret")]
        [Authorize]
        public string Index() 
        {
            var claims = User.Claims.ToList();

            return "secret message from API entra";
        }
    }
}
