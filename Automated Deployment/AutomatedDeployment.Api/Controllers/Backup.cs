using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using AutomatedDeployment.Api.Services;

namespace AutomatedDeployment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Backup : ControllerBase
    {
        private readonly IPathRepository pathRepository;
        public Backup(IPathRepository _pathRepository)
        {
            pathRepository = _pathRepository;
        }
        [HttpGet]
        public IActionResult GetAssemblyFolder(int HubId = 1, int ApplicationID = 1)
        {
            //var _context = new EfgconfigurationdbContext();

            //var assemblyPath = _context.Configurations.Where(c => c.HubID == HubId && c.AppID == ApplicationID).Select(c => c.AssemblyPath).SingleOrDefault();
            //OpenAssemblyFolder(assemblyPath);
            //return Ok();
            try
            {
                var assemblyPath = pathRepository.GetAssemblyPath(ApplicationID, HubId);
                OpenAssemblyFolder(assemblyPath);
                return Ok();

            }catch(Exception)
            {
                return NotFound();
            }

        }


       
        public IActionResult OpenAssemblyFolder(string assemblyPath)
        {    
            if (Directory.Exists(assemblyPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = assemblyPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
     

        //public FileContentResult  MoveTOBackUpFolder(List<string> filePaths)
        //{
        //    //string[] filePaths = Directory.GetFiles("Your Path");
        //    //foreach (var filename in filePaths)
        //    //{
        //    //    string file = filename.ToString();

        //    //    string str = "Your Destination" + file.ToString(),Replace("Your Path");
        //    //    if (!File.Exists(str))
        //    //    {
        //    //        File.Copy(file, str);
        //    //    }
        //    //}

        //    //return Ok();
        //}
    }
}
