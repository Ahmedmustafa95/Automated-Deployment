using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Backup : ControllerBase
    {
        [HttpGet]
        public IActionResult MoveTOBackUpFolder(int HubId,int ApplicationID)
        {
            // select  assembly file from connfiguratuion where 

            return Ok();
        }
    }
}
