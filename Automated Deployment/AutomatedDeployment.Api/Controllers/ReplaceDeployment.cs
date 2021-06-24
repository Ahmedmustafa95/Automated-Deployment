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


        [HttpPost]
        public IActionResult UploadFiles(List<IFormFile> files, [System.Web.Http.FromUri] string hubIds="", [System.Web.Http.FromUri] string appIds = "",
                        [System.Web.Http.FromUri] string ApprovedBy = "", [System.Web.Http.FromUri] string DeployedBy = "", [System.Web.Http.FromUri] string RequestedBy = "")
        {
            var ArrhubIds = hubIds.Split("_");
            var ArrappIds = appIds.Split("_");
            List<HubsApplications> hubsApplications = new List<HubsApplications>();
            foreach(var hubId in ArrhubIds)
            {
                foreach (var appId in ArrappIds)
                {
                    hubsApplications.Add(new HubsApplications() { AppID = int.Parse(appId), HubID = int.Parse(hubId) });
                }
            }

            UploadingFileViewModel fileViewModel = new UploadingFileViewModel() 
            {
                files = files,
                ApprovedBy = ApprovedBy,
                DeployedBy = DeployedBy,
                RequestedBy = RequestedBy,
                HubsApplications = hubsApplications
            };
            
            try
            {
                var uploadAndBackupFiles= replaceService.UploadAndBackupFiles(fileViewModel);
                switch (uploadAndBackupFiles)
                {
                    case UploadStatus.Success:
                        return Ok();
                    case UploadStatus.DatabaseFailure:
                        return StatusCode(StatusCodes.Status500InternalServerError, "Failed To Save In Database");
                    case UploadStatus.AssembyNotExist:
                        return NotFound("Assembly Not Exist");
                    case UploadStatus.NotValidData:
                        return BadRequest("HubsApplication Is not Valid");
                    case UploadStatus.DeletedFailed:
                        return StatusCode(StatusCodes.Status500InternalServerError, "Failed To Upload File And Failed To Delete " +
                            "Current Deployment From Database");
                    case UploadStatus.Uploadfailed:
                        return StatusCode(StatusCodes.Status500InternalServerError, "Failed To Upload File ");

                }
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();   
            }
        }
    }
}
