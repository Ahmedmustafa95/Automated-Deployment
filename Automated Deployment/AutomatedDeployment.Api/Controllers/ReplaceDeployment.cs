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
    [Route("api/[controller]")]
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
        //public IActionResult Upload(UploadingModel uploadingModel)
        public IActionResult Upload(List<IFormFile> files)
        {
            //List<IFormFile>files = uploadingModel.files;
            //List<HubsApplications> hubsApplications = uploadingModel.HubsApplications;
            var hubsApplications = new List<HubsApplications>
            {
                new HubsApplications
                {
                    HubID=14,
                    AppID=8
                },
                 new HubsApplications
                {
                    HubID=14,
                    AppID=9
                },
                  new HubsApplications
                {
                    HubID=14,
                    AppID=10
                }
            };
            var deploymentDetails = new List<DeploymentDetails>();
            var deploymentFiles = new List<DeploymentFiles>();
            int currentDeploymentId = unitOfWork.DeploymentRepository.GetCurrentDeploymentId();
            int currentDeploymentDetailsId = unitOfWork.DeploymentDetailsRepository
                                                       .GetCurrentDeploymentDetailsId();
            var currentDate = DateTime.Now;
            Deployment deployment ;
            foreach (var hubsApplication in hubsApplications)
            {
                if (!CheckValidData(hubsApplication.HubID,hubsApplication.AppID)) return BadRequest("Not Valid Data");
                string AssemblyPath = $"{pathRepository.GetAssemblyPath(hubsApplication.HubID, hubsApplication.AppID)}{@"\"}".Trim();
                if (AssemblyPath is null) { return NotFound(); }

                if (unitOfWork.DeploymentRepository.GetDeploymentCounts(hubsApplication.HubID, hubsApplication.AppID) > 0)
                {
                    string BackUpPath = pathRepository.GetBackupPath(hubsApplication.HubID, hubsApplication.AppID);


                    Dictionary<string, List<string>> filesState = replaceService.CompareDeployFilesWithAssemblyFiles(files, AssemblyPath);

                    if (filesState["Modified"].Count > 0)
                    {

                        List<string> backupFiles = new List<string>();
                        backupFiles.AddRange(filesState["Modified"]);
                        string NewBackupPath = $"{BackUpPath}\\BK_{currentDate.ToString("yyyy-MM-dd-hh-mm-ss")}".Trim();
                        Directory.CreateDirectory(NewBackupPath);
                        ibackupService.MoveTOBackUpFolder(backupFiles, AssemblyPath, NewBackupPath);
                        replaceService.Upload(files, AssemblyPath);

                    }

                    foreach (var fileName in filesState["Modified"])
                    {
                        DeploymentFiles deploymentFile = new DeploymentFiles()
                        { DeploymentDetailsId = currentDeploymentDetailsId, FilesName = fileName, Status = status.Modified };
                        deploymentFiles.Add(deploymentFile);
                    }

                    foreach (var fileName in filesState["Added"])
                    {
                        DeploymentFiles deploymentFile = new DeploymentFiles()
                        { DeploymentDetailsId = currentDeploymentDetailsId, FilesName = fileName, Status = status.Added };
                        deploymentFiles.Add(deploymentFile);
                    }

                    var deploymentDetail = new DeploymentDetails
                    {
                        HubId = hubsApplication.HubID,
                        AppId = hubsApplication.AppID,
                        DeploymentId = currentDeploymentId

                    };
                    deploymentDetails.Add(deploymentDetail);

                    //deployment = new Deployment
                    //{
                    //    DeploymentDate = currentDate,
                    //    DeploymentType = DeploymentType.Deployment,
                    //    DeployedBy = "shawky",
                    //    ApprovedBy = "ahmed",
                    //    RequestedBy = "Mustafa"
                    //};
                    //if (unitOfWork.DeploymentRepository.AddDeployment(deployment) is null)
                    //    return StatusCode(StatusCodes.Status500InternalServerError, " Failed to Save Deployment in database");

                    //if (unitOfWork.DeploymentDetailsRepository.AddDeploymentDetails(deploymentDetails) is null)
                    //    return StatusCode(StatusCodes.Status500InternalServerError,
                    //        " Failed to Save Deployment Files in database");
                    



                }
                else
                {
                    replaceService.Upload(files, AssemblyPath);
                   
                    var deploymentDetail = new DeploymentDetails
                    {
                        HubId = hubsApplication.HubID,
                        AppId = hubsApplication.AppID,
                        DeploymentId = currentDeploymentId

                    };
                    deploymentDetails.Add(deploymentDetail);
                    
                }

            }
            deployment = new Deployment()
            {
                DeploymentDate = currentDate,
                DeploymentType = DeploymentType.Deployment,
                DeployedBy = "Islam",
                ApprovedBy = "ahmed",
                RequestedBy = "Mustafa",
            };
            if (unitOfWork.DeploymentRepository.AddDeployment(deployment) is null)
                return StatusCode(StatusCodes.Status500InternalServerError, " Failed to Save Deployment in database");

            if (unitOfWork.DeploymentDetailsRepository.AddDeploymentDetails(deploymentDetails) is null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    " Failed to Save Deployment Files in database");

            if (deploymentFiles.Count > 0)
            {
                if (unitOfWork.DeploymentFilesRepository.AddDeploymentFiles(deploymentFiles) is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, " Failed to Save Deployment Files in database");
            }

            
            return Ok();
        }
    }
}
