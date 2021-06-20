using AutomatedDeployment.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrialController : ControllerBase
    {
        private readonly StringManipulationRepository stringManipulationRepo;

        public TrialController(StringManipulationRepository stringManipulationRepo)
        {
            this.stringManipulationRepo = stringManipulationRepo;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(stringManipulationRepo.FindConfigSetting("connectionStrings"));
                //return Ok(stringManipulationRepo.GetAppConfigFilesData(1,1));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
