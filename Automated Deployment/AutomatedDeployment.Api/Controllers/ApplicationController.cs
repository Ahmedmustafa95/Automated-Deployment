using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
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
                return NotFound();
            }
        }

        // GET api/<ApplicationController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var application = applicationRepository.GetById(id);
                return Ok(application);
            }
            catch (Exception)
            {
                return NotFound();
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
                    applicationRepository.Add(value);
                    return Ok();
                }
                catch
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }

        }

        // PUT api/<ApplicationController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Application value)
        {
            try
            {
                applicationRepository.Update(value);
                return Ok(value);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // DELETE api/<ApplicationController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {

                var app = applicationRepository.Delete(id);
                if (app != null)
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPost("{hubID}")]
       
        public IActionResult GetAllApps(int hubID)
        {
            return Ok(applicationRepository.GetAppsByHubID(hubID));
        }

    }
}
