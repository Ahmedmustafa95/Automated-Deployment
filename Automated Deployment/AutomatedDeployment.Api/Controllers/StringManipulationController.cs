using AutomatedDeployment.Api.Services;
using AutomatedDeployment.Domain.Entities;
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
    public class StringManipulationController : ControllerBase
    {
       private readonly IStringManipulationServices _stringManipulationServices;
        public StringManipulationController(IStringManipulationServices stringManipulationServices )
        {
            _stringManipulationServices = stringManipulationServices;
        }
        [HttpPost]
        public IActionResult ChangeElementValue(string key,string Value)
        {
            try
            {
                return Ok(_stringManipulationServices.UpdateKeyInMultiApplication(key, Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //public IActionResult Get(ConfigSearchResult UpdatedConfig)
        //{
        //    try
        //    {
        //        return Ok(_stringManipulationServices.UpdateSingleConfigData(UpdatedConfig));
        //    }
        //    catch (Exception)
        //    {
        //        return NotFound();
        //    }
        //}
    }
   
}
