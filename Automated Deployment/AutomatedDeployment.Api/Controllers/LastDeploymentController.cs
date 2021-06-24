using AutomatedDeployment.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutomatedDeployment.Api.Services;
using AutomatedDeployment.Domain.Entities;

namespace AutomatedDeployment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LastDeploymentController : ControllerBase
    {
       
        private readonly IUnitOfWork unitOfWork;

        public LastDeploymentController(
       
            IUnitOfWork _unitOfWork
            )
        {
         
            unitOfWork = _unitOfWork;
        }

        [HttpGet]
        public ActionResult<List<DeploymentDetails>> LastDeployment()
        {

            try
            {
                var deployment = unitOfWork.DeploymentFilesRepository.GetLastDepolyment();

                return deployment;
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }
    }
}
