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
        private readonly IReplaceServices replaceService;
        private readonly IPathRepository pathRepository;
        public ReplaceDeployment(IReplaceServices _replaceService, IPathRepository _pathRepository)
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
     
        public IActionResult Upload(List<IFormFile> files,int hubid,int applicationid)
        {
            string assembpath = pathRepository.GetAssemblyPath(hubid,applicationid);
           if(assembpath==null)
            {
                return NotFound();
            }
            replaceService.Upload(files,assembpath);

            return Ok();
        }
    }
}
