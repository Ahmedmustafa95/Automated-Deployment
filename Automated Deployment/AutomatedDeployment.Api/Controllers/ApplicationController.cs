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
        public IEnumerable<Application> Get()
        {
         
            return applicationRepository.GetAll();
         
        }

        // GET api/<ApplicationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ApplicationController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ApplicationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApplicationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
