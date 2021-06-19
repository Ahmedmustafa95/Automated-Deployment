using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
   public interface IRollbackService
    {
        public void Rollback(int hubID, int ApplicationID,string backuppath,string assemblypath,Dictionary<string, status> deploymentFiles);
    }

}
