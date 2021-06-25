using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                return Ok(AllDeployments);
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
