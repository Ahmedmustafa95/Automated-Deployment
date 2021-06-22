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
        public IActionResult GetallFiles(int hubid, int applicationid)
        {
            try
            {
                var result = unitOfWork.DeploymentRepository.GetAllfiles(hubid, applicationid);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }


        private bool CheckValidData(int hubid, int applicationid) =>
         unitOfWork.HubsApplicationsRepository
                   .GetHubsApplicationByID(hubid, applicationid) != null;

        [HttpPost]
        public IActionResult Upload([FromBody] UploadingModel uploadingModel,int hubid,int applicationid)

        {
            //public IActionResult Upload(List<IFormFile> files, int hubid, int applicationid,List<string> Deleted)

            //{

            //List<string> Deletedfiles = new List<string>();
            //foreach (var file in Deleted)
            //{
            //    Deletedfiles.Add(file.FileName);
            //}
            var files = uploadingModel.files;
            var Deletedfiles = uploadingModel.Deleted;

            if (!CheckValidData(hubid, applicationid)) return BadRequest("Not Valid Data");
            string AssemblyPath = $"{pathRepository.GetAssemblyPath(hubid, applicationid)}{@"\"}".Trim();
            if (AssemblyPath is null) { return NotFound(); }

            if (unitOfWork.DeploymentRepository.GetDeploymentCounts(hubid, applicationid) > 0)
            {
                string BackUpPath = pathRepository.GetBackupPath(hubid, applicationid);


                // Dictionary has Files Name as key and Files state as value
                Dictionary<string,List<string>> filesState = replaceService.CompareDeployFilesWithAssemblyFiles(files, AssemblyPath);

                //var filesName = files.Select(f => f.FileName).ToList();
                //ibackupService.MoveTOBackUpFolder(filesName, AssemblyPat/h, BackUpPath);
                var currentDate = DateTime.Now;

                if (filesState["Modified"].Count > 0 || Deletedfiles.Count > 0)
                {
                    //if (!Directory.Exists(BackUpPath))
                    //    Directory.CreateDirectory(BackUpPath);
                    List<string> backupFiles = new List<string>();
                    backupFiles.AddRange(filesState["Modified"]);
                    backupFiles.AddRange(Deletedfiles);
                  
                    string NewBackupPath = $"{BackUpPath}\\BK_{currentDate.ToString("yyyy-MM-dd-hh-mm-ss")}".Trim();
                    Directory.CreateDirectory(NewBackupPath);
                    ibackupService.MoveTOBackUpFolder(backupFiles, AssemblyPath, NewBackupPath);

                }
                replaceService.Upload(files, AssemblyPath);

                Deployment deployment = new Deployment()
                {
                    HubID = hubid,
                    AppID = applicationid,
                    DeploymentDate = currentDate,
                    DeployedBy="shawky",
                    ApprovedBy = "ahmed",
                    RequestedBy = "Mustafa",
                };

                if (unitOfWork.DeploymentRepository.AddDeployment(deployment) is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, " Failed to Save Deployment in database");  
                 
                
                int currentDeploymentId=unitOfWork.DeploymentRepository.GetCurrentDeploymentId();

                List<DeploymentFiles> deploymentFiles = new List<DeploymentFiles>();

                //refactor code
                foreach (var fileName in filesState["Modified"])
                {
                    DeploymentFiles deploymentFile = new DeploymentFiles() 
                    {DeploymentID= currentDeploymentId, FilesName= fileName, Status=status.Modified };
                    deploymentFiles.Add(deploymentFile);
                }


                foreach (var fileName in filesState["Added"])
                {
                    DeploymentFiles deploymentFile = new DeploymentFiles() 
                    { DeploymentID = currentDeploymentId, FilesName = fileName, Status = status.Added };
                    deploymentFiles.Add(deploymentFile);
                }

                foreach (var fileName in Deletedfiles)
                {
                    DeploymentFiles deploymentFile = new DeploymentFiles()
                    { DeploymentID = currentDeploymentId, FilesName = fileName, Status = status.Deleted };
                    deploymentFiles.Add(deploymentFile);
                }

                if (unitOfWork.DeploymentFilesRepository.AddDeploymentFiles(deploymentFiles) is null)
                return StatusCode(StatusCodes.Status500InternalServerError, " Failed to Save Deployment Files in database");


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
                    DeployedBy = "belal"
                };
                if (unitOfWork.DeploymentRepository.AddDeployment(deployment) is null)
                return StatusCode(StatusCodes.Status500InternalServerError, " Failed to Save Deployment in database");


            }


            return Ok();
        }
    }
}
