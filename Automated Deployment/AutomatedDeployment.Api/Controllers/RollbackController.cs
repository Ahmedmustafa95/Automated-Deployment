using AutomatedDeployment.Api.Services;
using AutomatedDeployment.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AutomatedDeployment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RollbackController : ControllerBase
    {
        private readonly IRollbackService _rollbackService;
        public RollbackController(IRollbackService rollbackService)
        {
            _rollbackService =rollbackService;
        }

        [HttpPost]
        public IActionResult Rollback(List<RollBackViewModel> rollBackViewModels)
        {
            if (!ModelState.IsValid)return BadRequest();
            try
            {
               bool hasSucceded= _rollbackService.GenralRollback(rollBackViewModels);
               if(hasSucceded ==false) return StatusCode(StatusCodes.Status500InternalServerError);
               return Ok();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        //[HttpGet("{hubid}")]
        //public IActionResult RollbackHub(int hubid)
        //{
        //    try
        //    {
        //        rollbackService.HubRollback(hubid,"shawky","new","shawky");
        //        return Ok();
        //    }catch
        //    {
        //        return NotFound();
        //    }
        //}

        //[HttpGet("{hubid}/{applicationid}")]
        //public IActionResult RollBack(int hubid, int applicationid)
        //{
        //    try
        //    {
        //        //string AssemblyPath = pathRepository.GetAssemblyPath(hubid, applicationid);
        //        //string BackUpPath = pathRepository.GetBackupPath(hubid, applicationid);
        //        ////string AssemblyPath = @"C:\Users\ecs\Desktop\Test";
        //        ////string BackUpPath = @"C:\Users\ecs\Desktop\Test\backups";
        //        //var currentDate = DateTime.Now;
        //        //var lastdeployment = unitOfWork.DeploymentRepository.GetLastDeployment().DeploymentDate;
        //        //var y = unitOfWork.DeploymentFilesRepository.GetById(hubid, applicationid);
        //        //int x = rollbackService.RollbackHelp(hubid, applicationid, "shawky", "belal", "eslam",currentDate); 
        //        //rollbackService.Rollback(hubid, applicationid, BackUpPath, AssemblyPath,x,currentDate, y, lastdeployment);
        //        rollbackService.SingleRollback(hubid, applicationid, "shawky", "new", "shawky");
        //        return Ok();
        //    }catch (Exception e)
        //    {
        //        return NotFound(e);
        //    }
        //}
    }
}
