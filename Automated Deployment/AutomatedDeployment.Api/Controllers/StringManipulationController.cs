using AutomatedDeployment.Api.Services;
using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class StringManipulationController : ControllerBase
    {
        private readonly StringManipulationRepository _stringManipulationServices;
        private readonly IBackupServices _backupServices;
        private readonly IPathRepository _pathRepository;

        public StringManipulationController(StringManipulationRepository stringManipulationServices,
                                            IBackupServices backupServices, IPathRepository pathRepository)
        {
            _stringManipulationServices = stringManipulationServices;
            _backupServices = backupServices;
            this._pathRepository = pathRepository;
        }


        [HttpGet("/api/[controller]/{key}")]
        public IActionResult GetAppsContainKey([FromRoute] string key)
        {
            if (!ModelState.IsValid) return BadRequest();
            var AppsContianKeyList = _stringManipulationServices.FindConfigSetting(key);
            if (AppsContianKeyList is null) return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(AppsContianKeyList);
        }


        [HttpPut]
        public IActionResult UpdateAllAppsConfigFiles([FromBody] List<ConfigSearchResult> UpdatedConfig)
        {
            List<ConfigSearchResult> SuccessededResults = new List<ConfigSearchResult>();
            if (!ModelState.IsValid) return BadRequest();

            foreach (ConfigSearchResult SingleConfig in UpdatedConfig)
            {
                try
                {
                    _backupServices.MoveTOBackUpFolder(SingleConfig.FileName, _pathRepository.GetBackupPath(SingleConfig.HubID, SingleConfig.AppID));
                    bool HasSuccedded = _stringManipulationServices.UpdateSingleConfigData(SingleConfig);
                    if (!HasSuccedded) continue;
                    SuccessededResults.Add(SingleConfig);
                }
                catch (Exception)
                {
                    continue;
                }
            }
            if(SuccessededResults.Count==0)return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(SuccessededResults);
        }
        //[HttpGet]
        //[Route("/{hubid:int}/{applicationid:int}")]
        //public IActionResult GetAppConfiguration([FromRoute] int hubid, [FromRoute] int applicationid)
        //{
        //    //Dictionary<string, List<XmlConfigObj>> AppConfigurtions = _stringManipulationServices.GetAppConfigFilesData(hubid, applicationid);

        //    //if (AppConfigurtions is null) return StatusCode(StatusCodes.Status500InternalServerError);
        //    //return Ok(AppConfigurtions);
        //    return (Ok());
        //}

        //[HttpPut("/ChangeElementValue")]
        //public IActionResult ChangeElementValue(string key, string Value)
        //{
        //    if (!ModelState.IsValid) return BadRequest("Not valid Data");
        //    bool IsSuccess = _stringManipulationServices.UpdateKeyInMultiApplication(key, Value);
        //    if (!IsSuccess) return StatusCode(StatusCodes.Status500InternalServerError);
        //    return Ok(new { key, Value });
        //}



    }

}
//using AutomatedDeployment.Api.Services;
//using AutomatedDeployment.Core.Interfaces;
//using AutomatedDeployment.Domain.Entities;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AutomatedDeployment.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StringManipulationController : ControllerBase
//    {
//       private readonly IStringManipulationRepository _stringManipulationServices;
//        public StringManipulationController(IStringManipulationRepository stringManipulationServices )
//        {
//            _stringManipulationServices = stringManipulationServices;
//        }
//        [HttpPost]
//        public IActionResult ChangeElementValue(string key,string Value)
//        {
//            try
//            {
//                return Ok(_stringManipulationServices.UpdateKeyInMultiApplication(key, Value));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//        [HttpGet]
//        public IActionResult findConfig(string key)
//        {
//            try
//            {
//                return Ok(_stringManipulationServices.FindConfigSetting(key));
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        //public IActionResult Get(ConfigSearchResult UpdatedConfig)
//        //{
//        //    try
//        //    {
//        //        return Ok(_stringManipulationServices.UpdateSingleConfigData(UpdatedConfig));
//        //    }
//        //    catch (Exception)
//        //    {
//        //        return NotFound();
//        //    }
//        //}
//    }

//}
