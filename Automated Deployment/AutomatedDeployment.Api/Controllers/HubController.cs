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
    public class HubController : ControllerBase
    {
        private readonly IHubRepository hubRepository;

        public HubController(IHubRepository _hubRepository)
        {
            hubRepository = _hubRepository;
        }
        // GET: api/<HubController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(hubRepository.GetAll());
            }catch(Exception)
            {
                return NotFound();
            }
        }

        // GET api/<HubController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var Hub = hubRepository.GetById(id);
                return Ok(Hub);
            }catch(Exception)
            {
                return NotFound();
            }
        }

        // POST api/<HubController>
        [HttpPost]
        public IActionResult Post([FromBody] Hub value)
        {   if (TryValidateModel(value, nameof(Hub)))
            {
                try
                {
                    hubRepository.Add(value);
                    return Ok();
                }
                catch
                {
                    return NotFound();
                }
            }else
            {
                return NotFound();
            }

        }

        // PUT api/<HubController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Hub value)
        {
            try
            {
                hubRepository.Update(value);
                return Ok(value);
            }catch(Exception)
            {
                return NotFound();
            }
        }

        // DELETE api/<HubController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
              var hub = hubRepository.Delete(id);
                if (hub != null)
                    return Ok();
                else
                    return NotFound();
            }catch(Exception)
            {
                return NotFound();
            }
        }

      

    }
}
