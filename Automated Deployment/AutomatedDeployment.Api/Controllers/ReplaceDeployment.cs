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
        public IActionResult UploadFiles(UploadingFileViewModel fileViewModel)
        {
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
