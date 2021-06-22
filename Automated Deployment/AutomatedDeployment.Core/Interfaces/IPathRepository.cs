using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces
{
  public  interface IPathRepository
    {
        string GetAssemblyPath(int applicationid, int hubid);
        string GetBackupPath(int applicationid, int hubid);

        string GetConfigFilePath(int applicationid, int hubid);
        HubsApplications GetHubApplication(int applicationid, int hubid);


    }
}
