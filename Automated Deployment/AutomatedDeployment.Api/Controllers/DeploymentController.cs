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
    public class DeploymentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeploymentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var AllDeployments = _unitOfWork.DeploymentRepository.GetAll();
                if (AllDeployments is null) return NotFound();
                return Ok(AllDeployments);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("{deployid}")]
        public ActionResult<List<deploywithfilesviewmodel>> Getfiles(int deployid)
        {
            try
            {
                var allfileswithdeploy = _unitOfWork.DeploymentRepository.Getallwithfiles(deployid);
                if(allfileswithdeploy!=null)
                {
                    return allfileswithdeploy;
                }
                return NotFound();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
