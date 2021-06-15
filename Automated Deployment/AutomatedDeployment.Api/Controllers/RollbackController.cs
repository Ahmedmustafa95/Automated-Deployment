﻿using AutomatedDeployment.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Controllers
{
     [Route("api/[controller]/{hubid}/{applicationid}")]
    [ApiController]
    public class RollbackController : ControllerBase
    {
        private readonly IRollbackService rollbackService;
        private readonly IPathRepository pathRepository;


        public RollbackController(IRollbackService _rollbackService, IPathRepository _pathRepository)
        {
            rollbackService = _rollbackService;
            pathRepository = _pathRepository;
        }

        [HttpGet]
        public IActionResult RollBack(int hubid, int applicationid)
        {
            var paths = pathRepository.GetPaths(hubid, applicationid);
            rollbackService.Rollback(paths.BackupPath, paths.AssemblyPath);
            return Ok();
        }
    }
}