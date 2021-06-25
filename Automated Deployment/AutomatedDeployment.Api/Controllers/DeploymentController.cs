using AutomatedDeployment.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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
    }
}
