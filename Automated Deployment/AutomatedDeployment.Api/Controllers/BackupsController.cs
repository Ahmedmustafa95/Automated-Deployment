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
    public class BackupsController : ControllerBase
    {
        private readonly IPathRepository pathRepository;
        private readonly IBackupServices ibackupService;
        public BackupsController(IPathRepository _pathRepository, IBackupServices _ibackupService)
        {
            pathRepository = _pathRepository;
            ibackupService = _ibackupService;
        }
        // Call from UI & send files & hubid & application id
        // is valid backup 
        //update database
        [HttpGet]
        //[Route("BackUp")]
        public IActionResult MoveTOBackUpFolder(List<IFormFile> files, int hubid = 1, int applicationid = 1)
        {
            string assembpath = pathRepository.GetConficPath(hubid, applicationid);
            if (assembpath == null)
            {
                return NotFound();
            }
            ibackupService.MoveTOBackUpFolder(files, assembpath);
            return Ok();
        }
    }
}