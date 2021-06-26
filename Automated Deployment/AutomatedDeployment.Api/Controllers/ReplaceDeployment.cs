using AutomatedDeployment.Api.Services;
using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


namespace AutomatedDeployment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplaceDeployment : ControllerBase
    {
        private readonly IReplaceServices _replaceService;
        private readonly IUnitOfWork _unitOfWork;

        public ReplaceDeployment(IReplaceServices replaceService, IUnitOfWork unitOfWork)
        {
            _replaceService = replaceService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetallFiles(int hubid, int applicationid)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var result = _unitOfWork.DeploymentRepository.GetAllfiles(hubid, applicationid);
                if(result is null) return StatusCode(StatusCodes.Status500InternalServerError);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost]
        public IActionResult UploadFiles(List<IFormFile> files, [System.Web.Http.FromUri] string hubIds, [System.Web.Http.FromUri] string appIds ,
                        [System.Web.Http.FromUri] string ApprovedBy, [System.Web.Http.FromUri] string DeployedBy, [System.Web.Http.FromUri] string RequestedBy)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (files.Count == 0 || hubIds == string.Empty || appIds == string.Empty ||
                ApprovedBy == string.Empty || DeployedBy == string.Empty || RequestedBy == string.Empty) return BadRequest("not valid data");
            
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
                var uploadAndBackupFiles= _replaceService.UploadAndBackupFiles(fileViewModel);
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
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
