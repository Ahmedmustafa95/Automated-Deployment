using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutomatedDeployment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase

    {
        private readonly IApplicationRepository applicationRepository;

        public ApplicationController(IApplicationRepository _applicationRepository)
        {
            applicationRepository = _applicationRepository;
        }
        // GET: api/<ApplicationController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(applicationRepository.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<ApplicationController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                Application application = applicationRepository.GetById(id);
                if(application is null) return NotFound();
                return Ok(application);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<ApplicationController>
        [HttpPost]
        public IActionResult Post([FromBody] Application value)
        {
            if (TryValidateModel(value, nameof(Application)))
            {
                try
                {
                    Application NewApp=applicationRepository.Add(value);
                    if (NewApp is null) return BadRequest();
                    return Ok(NewApp);
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                return BadRequest();
            }

        }

        // PUT api/<ApplicationController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Application value)
        {
            if(!ModelState.IsValid) return BadRequest();
            try
            {
                Application UpdatedApp = applicationRepository.Update(value);
                if (UpdatedApp is null) return BadRequest();
                return Ok(UpdatedApp);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<ApplicationController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if(!ModelState.IsValid) return BadRequest();
            try
            {
                Application DeletedApp = applicationRepository.Delete(id);
                if(DeletedApp is null) return BadRequest();
                return Ok(DeletedApp);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
       
        [HttpGet("GetAppByhubID/{hubID}")]
        public IActionResult GetAllApps(int hubID)
        {
            if (!ModelState.IsValid) return BadRequest();
            List<HubsApplications> hubsApplicationsList = applicationRepository.GetAppsByHubID(hubID);
            //  if (hubsApplicationsList is null || hubsApplicationsList.Count == 0) return NotFound();

            return Ok(hubsApplicationsList);
        }

    }
}
