using AutomatedDeployment.Api.Services;
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
    public class ReplaceDeployment : ControllerBase
    {
        private readonly IReplaceService replaceService;
        public ReplaceDeployment(IReplaceService _replaceService)
        {
            replaceService = _replaceService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("File Upload API running...");
        }

        [HttpPost]
        [Route("upload")]
        public IActionResult Upload(List<IFormFile> files)
        {
            replaceService.Upload(files);

            return Ok();
        }
    }
}
