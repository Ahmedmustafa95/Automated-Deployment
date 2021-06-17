using AutomatedDeployment.Api.Services;
using AutomatedDeployment.Core.Interfaces;
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
       
        [HttpGet]
        //[Route("BackUp")]
        public IActionResult MoveTOBackUpFolder(List<string> filesName, string assemblyPath, string backupPath)
        {
            //List<string> filesName = new List<string>();
            // //string  assemblyPath = @"C:\Users\Ziad\Desktop\folder1";
            // //string   backupPath = @"C:\Users\Ziad\Desktop\folder2";
            //filesName.Add("text1.txt");
            //filesName.Add("text2.txt");
            ibackupService.MoveTOBackUpFolder(filesName, assemblyPath, backupPath);
            return Ok();
        }
    }
}