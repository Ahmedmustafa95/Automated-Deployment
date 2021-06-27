using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AutomatedDeployment.Core.Services
{
    public class PathRepository : IPathRepository
    {
        private readonly EfgconfigurationdbContext efgconfigurationdbContext;
        public PathRepository(EfgconfigurationdbContext _efgconfigurationdbContext)
        {
            efgconfigurationdbContext = _efgconfigurationdbContext;
        }
        public string GetAssemblyPath(int hubid,int applicationid)
        {
          HubsApplications result=  efgconfigurationdbContext.HubsApplications.AsNoTracking().FirstOrDefault(c => c.AppID == applicationid && c.HubID == hubid);
            if (result is not null)
            {
                return result.AssemblyPath;

            }else
            {
                return null;
            }
        }
 
        public string GetBackupPath(int hubid ,int applicationid)
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

        public string GetConfigFilePath(int hubid, int applicationid)
        {
            HubsApplications result = efgconfigurationdbContext.HubsApplications.AsNoTracking().FirstOrDefault(c => c.AppID == applicationid && c.HubID == hubid);
            if (result != null)
            {
                return result.ConfigFilepPath;

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
