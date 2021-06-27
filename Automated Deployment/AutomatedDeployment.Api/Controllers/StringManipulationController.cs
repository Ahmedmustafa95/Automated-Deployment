using AutomatedDeployment.Api.Services;
using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
            _pathRepository = pathRepository;
        }

        private List<List<List<ConfigSearchResult>>> setTree(List<ConfigSearchResult> AppsContianKeyList) {
            //ConfigSearchResult[][][] theSolver = ;
            
            List<List<List<ConfigSearchResult>>> theSolver = new List<List<List<ConfigSearchResult>>>();
            Dictionary<string, Dictionary<int, List<ConfigSearchResult>>> mapData = new Dictionary<string, Dictionary<int, List<ConfigSearchResult>>>();
            AppsContianKeyList.ForEach(d =>
            {
                if(mapData.TryGetValue(d.OldConfigurationResult, out Dictionary<int, List<ConfigSearchResult>> value))
                {
                    if (value.TryGetValue(d.HubID, out List<ConfigSearchResult> configSearchResult)) {
                        value[d.HubID].Add(d);
                    }
                    else
                    {
                        //var newHub = new Dictionary<int, List<ConfigSearchResult>>();
                        var appList = new List<ConfigSearchResult>();
                        appList.Add(d);
                        //newHub.Add(d.HubID, appList);
                        value.Add(d.HubID, appList);
                    }
                }
                else
                {
                    var newHub = new Dictionary<int, List<ConfigSearchResult>>();
                    var appList = new List<ConfigSearchResult>();
                    appList.Add(d);
                    newHub.Add(d.HubID, appList);
                    mapData.Add(d.OldConfigurationResult, newHub);
                }
            });
            var hubs = mapData.Keys;
            foreach (KeyValuePair<string, Dictionary<int, List<ConfigSearchResult>>> entry in mapData)
            {
                var newValue = new List<List<ConfigSearchResult>>();
                foreach (KeyValuePair<int, List<ConfigSearchResult>> entry2 in entry.Value)
                {
                    newValue.Add(entry2.Value);
                }
                theSolver.Add(newValue);
            }
            /*AppsContianKeyList.ForEach(d => { 
            var isValueExist = false;
            for (var i = 0; i < theSolver.Count; i++)
            {
                var isHubExist = false;
                for (var j = 0; j < theSolver[i].Count; j++)
                {
                    for (var k = 0; k < theSolver[i][j].Count; k++)
                    {
                        isValueExist = theSolver[i][j][k].OldConfigurationResult == d.OldConfigurationResult;

                        isHubExist = theSolver[i][j][k].HubID == d.HubID;
                      if (isValueExist && isHubExist)
                        {
                            theSolver[i][j].Add(d);
                  }
                        break;
                    }
                    if (isHubExist)
                        break;
                }
                if (!isHubExist && isValueExist)
                {
                    var l = new List<ConfigSearchResult>();
                        l.Add(d);
                    theSolver[i].Add(l);
                break;
        }
    }
                if (!isValueExist)
                {
                    var _list = new List<ConfigSearchResult>();
                    var _listlist = new List<List<ConfigSearchResult>>();
                    _list.Add(d);
                    _listlist.Add(_list);
                    theSolver.Add(_listlist);
                }
            });
            */
            return theSolver;
        }
        [HttpGet("/api/[controller]/{key}")]
        public IActionResult GetAppsContainKey([FromRoute] string key)
        {
            if (!ModelState.IsValid) return BadRequest();
            List<ConfigSearchResult> AppsContianKeyList = _stringManipulationServices.FindConfigSetting(key);
            if (AppsContianKeyList is null) return StatusCode(StatusCodes.Status500InternalServerError);
            
            return Ok(setTree(AppsContianKeyList));
        }


        [HttpPut]
        public IActionResult UpdateAllAppsConfigFiles([FromBody] List<ConfigSearchResult> UpdatedConfig)
        {
            List<ConfigSearchResult> SuccessededResults = new List<ConfigSearchResult>();
            if (!ModelState.IsValid) return BadRequest();

            foreach (ConfigSearchResult SingleConfig  in UpdatedConfig)
            {
                try
                {
                    _backupServices.MoveTOBackUpFolder(SingleConfig.FileName, _pathRepository.GetBackupPath(SingleConfig.HubID, SingleConfig.AppID));
                    bool HasSuccedded = _stringManipulationServices.UpdateSingleConfigData(SingleConfig);
                    if (!HasSuccedded) continue;
                    SuccessededResults.Add(SingleConfig);
                }
                catch (Exception e)
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
