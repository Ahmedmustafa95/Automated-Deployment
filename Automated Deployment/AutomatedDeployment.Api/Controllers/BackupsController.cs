using AutomatedDeployment.Api.Services;
using AutomatedDeployment.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AutomatedDeployment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupsController : ControllerBase
    {
        private readonly IPathRepository _pathRepository;
        private readonly IBackupServices _ibackupService;
        public BackupsController(IPathRepository pathRepository, IBackupServices ibackupService)
        {
            _pathRepository = pathRepository;
            _ibackupService = ibackupService;
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
            _ibackupService.MoveTOBackUpFolder(filesName, assemblyPath, backupPath);
            return Ok();
        }
    }
}