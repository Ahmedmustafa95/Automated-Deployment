using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Core.Services;
using AutomatedDeployment.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Controllers
{
    [Route("api/[controller]/{hubId}/{applicationId}")]
    [ApiController]
    public class HubsApplicationController : ControllerBase
    {
        private readonly IHubsApplicationsRepository hubsApplicationsRepository;

        public HubsApplicationController(IHubsApplicationsRepository _hubsApplicationsRepository)
        {
            hubsApplicationsRepository = _hubsApplicationsRepository;
        }
        [HttpGet]
        public IActionResult GetGubApplicationByID (int hubId , int applicationId)
        {
            try
            {
                var hubapplication = hubsApplicationsRepository.GetHubsApplicationByID(hubId, applicationId);
                return Ok(hubapplication);
            }catch
            {
                return NotFound();
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
            try
            {
                var hubapplicaton = hubsApplicationsRepository.Add(_hubsApplications);
                return Ok(hubapplicaton);
            }
            catch
            {
                return NotFound(); 
            }
        }

        [HttpPut]
        public IActionResult UpdateHubsApplication (HubsApplications _hubsApplications,
                                                    [FromRoute] int hubId,
                                                    [FromRoute] int applicationId
                                                   )
        {
            if (hubId != _hubsApplications.HubID || applicationId != _hubsApplications.AppID)
                return NotFound();
            try
            {

                var hubsapplication = hubsApplicationsRepository.Update(_hubsApplications);
                return Ok();
            }catch
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public IActionResult DeleteHubsApplication (int hubId,int applicationId)
        {
            try
            {
                var hubsapplication = hubsApplicationsRepository.DeleteHubApplication(hubId, applicationId);
                return Ok(hubsapplication);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
