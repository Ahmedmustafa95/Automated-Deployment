using AutomatedDeployment.Api.Services;
using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
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
        private readonly IUnitOfWork unitOfWork;

        public ReplaceDeployment(IReplaceServices _replaceService, IPathRepository _pathRepository, 
            IBackupServices _ibackupService, IUnitOfWork _unitOfWork)
        {
            replaceService = _replaceService;
            pathRepository = _pathRepository;
            ibackupService = _ibackupService;
            unitOfWork = _unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()=>  Ok("File Upload API running...");

   
        public bool CheckValidData(int hubid, int applicationid)=>
         unitOfWork.HubsApplicationsRepository.GetHubsApplicationByID(hubid, applicationid) != null;  
      
        [HttpPost]
        public IActionResult Upload(List<IFormFile> files,int hubid,int applicationid)
        {
            if (!CheckValidData(hubid, applicationid)) return BadRequest("Not Valid Data");

            if(unitOfWork.DeploymentRepository.GetDeploymentCounts(hubid,applicationid)>1)
            {
                string AssemblyPath = pathRepository.GetAssemblyPath(hubid, applicationid);
                string BackUpPath = pathRepository.GetBackupPath(hubid, applicationid);
                if (AssemblyPath == null)
                {
                    return NotFound();
                }
                var filesName = files.Select(f => f.FileName).ToList();
                ibackupService.MoveTOBackUpFolder(filesName, AssemblyPath, BackUpPath);
                replaceService.Upload(files, AssemblyPath);
            }
            else
            {
                string AssemblyPath = pathRepository.GetAssemblyPath(hubid, applicationid);
                if (AssemblyPath == null) return NotFound();
                
                replaceService.Upload(files, AssemblyPath);

                Deployment deployment = new Deployment()
                {HubID=hubid,
                 AppID=applicationid,
                 DeploymentDate=DateTime.Now,
                 ApprovedBy="ahmed",
                 RequestedBy="Mustafa",
                };
                if(unitOfWork.DeploymentRepository.AddDeployment(deployment) is null) 
                    return BadRequest(" Failed to Save Deployment in database");

            }
         

            return Ok();
        }
    }
}
