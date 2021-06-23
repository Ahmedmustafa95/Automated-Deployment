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
     [Route("api/[controller]/{hubid}/{applicationid}")]
    [ApiController]
    public class RollbackController : ControllerBase
    {
        private readonly IRollbackService rollbackService;
        private readonly IPathRepository pathRepository;
        private readonly IUnitOfWork unitOfWork;

        public RollbackController(
            IRollbackService _rollbackService, 
            IPathRepository _pathRepository,
            IUnitOfWork _unitOfWork
            )
        {
            rollbackService = _rollbackService;
            pathRepository = _pathRepository;
            unitOfWork = _unitOfWork;
        }

        [HttpGet]
        public IActionResult RollBack(int hubid, int applicationid)
        {
            try
            {
                //string AssemblyPath = pathRepository.GetAssemblyPath(hubid, applicationid);
                //string BackUpPath = pathRepository.GetBackupPath(hubid, applicationid);
                string AssemblyPath = @"C:\Users\ecs\Desktop\Test";
                string BackUpPath = @"C:\Users\ecs\Desktop\Test\backups";
                var currentDate = DateTime.Now;
                var lastdeployment = unitOfWork.DeploymentFilesRepository.GetLastDepolyment(hubid, applicationid).DeploymentDate;
                var y = unitOfWork.DeploymentFilesRepository.GetById(hubid, applicationid);
                int x = rollbackService.SingleRollback(hubid, applicationid, "shawky", "belal", "eslam",currentDate); 
                rollbackService.Rollback(hubid, applicationid, BackUpPath, AssemblyPath,x,currentDate, y, lastdeployment);
                return Ok();
            }catch (Exception e)
            {
                return NotFound(e);
            }
        }
    }
}
