﻿using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Services
{
    public class PathRepository : IPathRepository
    {
        private readonly EfgconfigurationdbContext efgconfigurationdbContext;
        public PathRepository(EfgconfigurationdbContext _efgconfigurationdbContext)
        {
            efgconfigurationdbContext = _efgconfigurationdbContext;
        }
        public string GetAssemblyPath(int applicationid, int hubid)
        {
          HubsApplications result=  efgconfigurationdbContext.HubsApplications.AsNoTracking().FirstOrDefault(c => c.AppID == applicationid && c.HubID == hubid);
            if (result!=null)
            {
                return result.AssemblyPath;

            }else
            {
                return null;
            }
        }
 
        public string GetBackupPath(int applicationid, int hubid)
        {
            HubsApplications result = efgconfigurationdbContext.HubsApplications.AsNoTracking().FirstOrDefault(c => c.AppID == applicationid && c.HubID == hubid);
            if (result != null)
            {
                return result.BackupPath;

            }
            else
            {
                return null;
            }
        }

        public HubsApplications GetHubApplication(int applicationid, int hubid)
        {

            HubsApplications  result = efgconfigurationdbContext.HubsApplications.Include(i => i.Application).Include(i=>i.Hub)
                .AsNoTracking().FirstOrDefault(c => c.AppID == applicationid && c.HubID == hubid);
            return result;
        }
    }
}
