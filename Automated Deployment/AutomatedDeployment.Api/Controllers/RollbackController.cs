using AutomatedDeployment.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Controllers
{
     [Route("api/[controller]/{hubid}/{applicationid}")]
    [ApiController]
    public class RollbackController : ControllerBase
    {
        private readonly IRollbackService rollbackService;

        public RollbackController(IRollbackService _rollbackService)
        {
            rollbackService = _rollbackService;
        }

        [HttpGet]
        public IActionResult RollBack(int hubid, int applicationid)
        {
            rollbackService.Rollback(@"C:\Users\ecs\Desktop\Backup", @"C:\Users\ecs\Desktop\assembly");
            return Ok();
        }
    }
}
