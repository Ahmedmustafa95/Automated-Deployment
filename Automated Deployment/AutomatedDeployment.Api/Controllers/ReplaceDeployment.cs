using AutomatedDeployment.Api.Services;
using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
        public IActionResult Get() => Ok("File Upload API running...");


        private bool CheckValidData(int hubid, int applicationid) =>
         unitOfWork.HubsApplicationsRepository
                   .GetHubsApplicationByID(hubid, applicationid) != null;

        [HttpPost]

        public IActionResult Upload(List<IFormFile> files, int hubid, int applicationid)

        {
            if (!CheckValidData(hubid, applicationid)) return BadRequest("Not Valid Data");
            string AssemblyPath = pathRepository.GetAssemblyPath(hubid, applicationid);
            if (AssemblyPath is null) { return NotFound(); }

            if (unitOfWork.DeploymentRepository.GetDeploymentCounts(hubid, applicationid) > 0)
            {
                string BackUpPath = pathRepository.GetBackupPath(hubid, applicationid);

                //if (!Directory.Exists(BackUpPath))
                //    Directory.CreateDirectory(BackUpPath);
                string NewBackupPath = $"{BackUpPath} \\ BK_{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}";
                Directory.CreateDirectory(NewBackupPath);

                // Dictionary has Files Name as key and Files state as value
                Dictionary<string, List<string>> filesState = replaceService.CompareDeployFilesWithAssemblyFiles(files, AssemblyPath);

                //var filesName = files.Select(f => f.FileName).ToList();
                //ibackupService.MoveTOBackUpFolder(filesName, AssemblyPath, BackUpPath);

                ibackupService.MoveTOBackUpFolder(filesState["Modified"], AssemblyPath, NewBackupPath);
                replaceService.Upload(files, AssemblyPath);
                Deployment deployment = new Deployment()
                {
                    HubID = hubid,
                    AppID = applicationid,
                    DeploymentDate = DateTime.Now,
                    ApprovedBy = "ahmed",
                    RequestedBy = "Mustafa",
                };

                if (unitOfWork.DeploymentRepository.AddDeployment(deployment) is null)
                    return BadRequest(" Failed to Save Deployment in database");

                int currentDeploymentId=unitOfWork.DeploymentRepository.GetCurrentDeploymentId();

                List<DeploymentFiles> deploymentFiles = new List<DeploymentFiles>();

                //refactor code
                foreach (var fileName in filesState["Modified"])
                {
                    DeploymentFiles deploymentFile = new DeploymentFiles() {DeploymentID= currentDeploymentId, FilesName= fileName, Status=status.Modified };
                    deploymentFiles.Add(deploymentFile);
                }

                foreach (var fileName in filesState["Added"])
                {
                    DeploymentFiles deploymentFile = new DeploymentFiles() { DeploymentID = currentDeploymentId, FilesName = fileName, Status = status.Added };
                    deploymentFiles.Add(deploymentFile);
                }

                if (unitOfWork.DeploymentFilesRepository.AddDeploymentFiles(deploymentFiles) is null)
                    return BadRequest(" Failed to Save Deployment Files in database");

            }
            else
            {

                replaceService.Upload(files, AssemblyPath);

                Deployment deployment = new Deployment()
                {
                    HubID = hubid,
                    AppID = applicationid,
                    DeploymentDate = DateTime.Now,
                    ApprovedBy = "ahmed",
                    RequestedBy = "Mustafa",
                };
                if (unitOfWork.DeploymentRepository.AddDeployment(deployment) is null)
                    return BadRequest(" Failed to Save Deployment in database");

            }


            return Ok();
        }
    }
}
