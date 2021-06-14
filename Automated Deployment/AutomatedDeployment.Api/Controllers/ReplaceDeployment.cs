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
        private readonly IBackupServices ibackupService;

        public ReplaceDeployment(IReplaceServices _replaceService, IPathRepository _pathRepository, IBackupServices _ibackupService)
        {
            replaceService = _replaceService;
            pathRepository = _pathRepository;
            ibackupService = _ibackupService;
        }

        [HttpGet]
        public IActionResult Get()
        {

            return Ok("File Upload API running...");

        }

        [HttpPost]
     
        public IActionResult Upload(List<IFormFile> files,int hubid,int applicationid)
        {
            //string assembpath = pathRepository.GetAssemblyPath(hubid,applicationid);
            var paths = pathRepository.GetPaths(hubid,applicationid);
            
           if(paths == null)
            {
                return NotFound();
            }
           var filesName= files.Select(f => f.FileName).ToList();
            ibackupService.MoveTOBackUpFolder(filesName, paths.AssemblyPath, paths.BackupPath);
            replaceService.Upload(files, paths.AssemblyPath);

            return Ok();
        }
    }
}
