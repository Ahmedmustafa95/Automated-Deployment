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
    public class ReplaceDeployment : ControllerBase
    {
        private readonly IReplaceService replaceService;
        private readonly IPathRepository pathRepository;
        public ReplaceDeployment(IReplaceService _replaceService, IPathRepository _pathRepository)
        {
            replaceService = _replaceService;
            pathRepository = _pathRepository;
        }
        [HttpGet]
        public IActionResult Get()
        {

            return Ok("File Upload API running...");

        }

        [HttpPost]
     
        [Route("upload")]
        public IActionResult Upload(List<IFormFile> files,int hubid,int applicationid)
        {
            string assembpath = pathRepository.GetConficPath(hubid,applicationid);
           if(assembpath==null)
            {
                return NotFound();
            }
            replaceService.Upload(files,assembpath);

            return Ok();
        }
    }
}
