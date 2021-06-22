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
        public StringManipulationController(IStringManipulationServices stringManipulationServices)
        {
            _stringManipulationServices = stringManipulationServices;
        }


        [HttpGet("/{key}")]
        public IActionResult GetAppsContainKey([FromRoute] string key)
        {

            var AppsContianKeyList = _stringManipulationServices.FindConfigSetting(key);
            if (AppsContianKeyList is null) return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(AppsContianKeyList);
        }

        [HttpGet]
        [Route("/{hubid:int}/{applicationid:int}")]
        public IActionResult GetAppConfiguration([FromRoute] int hubid, [FromRoute] int applicationid)
        {
            Dictionary<string, List<XmlConfigObj>> AppConfigurtions = _stringManipulationServices.GetAppConfigFilesData(hubid, applicationid);

            if (AppConfigurtions is null) return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(AppConfigurtions);
        }

        [HttpPut("/ChangeElementValue")]
        public IActionResult ChangeElementValue(string key, string Value)
        {
            if (!ModelState.IsValid) return BadRequest("Not valid Data");
            bool IsSuccess = _stringManipulationServices.UpdateKeyInMultiApplication(key, Value);
            if (!IsSuccess) return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(new { key, Value });
        }

        [HttpPut]
        public IActionResult UpdateSingleConfigData([FromBody] ConfigSearchResult UpdatedConfig)
        {
            try
            {
                return Ok(_stringManipulationServices.UpdateSingleConfigData(UpdatedConfig));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        


    }

}
