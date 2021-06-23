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
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<HubController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                Hub Hub = hubRepository.GetById(id);
                if (Hub is null) return NotFound();
                return Ok(Hub);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/<HubController>
        [HttpPost]
        public IActionResult Post([FromBody] Hub value)
        {  
            if (TryValidateModel(value, nameof(Hub)))
            {
                try
                {
                    Hub NewHub = hubRepository.Add(value);
                    if (NewHub is null) return BadRequest();
                    return Ok(NewHub);
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

        // PUT api/<HubController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Hub value)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                Hub UpdatedHub = hubRepository.Update(value);
                if (UpdatedHub is null) return BadRequest();
                return Ok(UpdatedHub);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<HubController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                Hub DeletedHub = hubRepository.Delete(id);
                if (DeletedHub is null) return BadRequest();
                return Ok(DeletedHub);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
