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
    public class LastDeploymentController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public LastDeploymentController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        [HttpGet]
        public ActionResult<List<LastDeploymentviewmodel>> LastDeployment()
        {
            try
            {
                List<LastDeploymentviewmodel> hubsinlastdeployment = unitOfWork.DeploymentFilesRepository.GetLastDepolyment();
                if(hubsinlastdeployment is null) return StatusCode(StatusCodes.Status500InternalServerError);
                return Ok(hubsinlastdeployment);
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

       
    }
}
