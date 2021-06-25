using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedDeployment.Api.Controllers
{
    [Route("api/[controller]/{hubId}/{applicationId}")]
    [ApiController]
    public class HubsApplicationController : ControllerBase
    {
        private readonly IHubsApplicationsRepository _hubsApplicationsRepository;

        public HubsApplicationController(IHubsApplicationsRepository hubsApplicationsRepository)
        {
            _hubsApplicationsRepository = hubsApplicationsRepository;
        }
        [HttpGet]
        public IActionResult GetGubApplicationByID (int hubId , int applicationId)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                HubsApplications hubapplication = _hubsApplicationsRepository.GetHubsApplicationByID(hubId, applicationId);
                if (hubapplication is null) return NotFound();
                return Ok(hubapplication);
            }catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        //[HttpGet]
        //public IActionResult GetAllhubApplication()
        //{
        //    try
        //    {
        //        return Ok(hubsApplicationsRepository.GetAll());
        //    }catch
        //    {
        //        return NotFound();
        //    }

        //}

        [HttpPost]
        public IActionResult AddhubApplication( HubsApplications _hubsApplications)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                HubsApplications hubapplicaton = _hubsApplicationsRepository.Add(_hubsApplications);
                if(hubapplicaton is null) return StatusCode(StatusCodes.Status500InternalServerError);
                return Ok(hubapplicaton);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult UpdateHubsApplication (HubsApplications hubsApplications,
                                                    [FromRoute] int hubId,
                                                    [FromRoute] int applicationId)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (hubId !=hubsApplications.HubID || applicationId != hubsApplications.AppID)return BadRequest();
            try
            {
                HubsApplications hubsapplication = _hubsApplicationsRepository.Update(hubsApplications);
                if(hubsapplication is null) return StatusCode(StatusCodes.Status500InternalServerError);
                return Ok(hubsapplication);
            }catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public IActionResult DeleteHubsApplication (int hubId,int applicationId)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                HubsApplications hubsapplication = _hubsApplicationsRepository.DeleteHubApplication(hubId, applicationId);
                if(hubsapplication is null) return StatusCode(StatusCodes.Status500InternalServerError);
                return Ok(hubsapplication);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
