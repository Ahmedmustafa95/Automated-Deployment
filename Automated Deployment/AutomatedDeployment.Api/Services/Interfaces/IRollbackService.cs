using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
   public interface IRollbackService
    {
        public void Rollback(int hubID, int ApplicationID,string backuppath,string assemblypath, int deploymentDetailsId,DateTime currentDate, Dictionary<string, status> deploymentFiles,DateTime lastfolderdate);
        public int RollbackHelp(int hubid, int applicationid, string deployedBy, string requestedBy, string approvedBy,DateTime curerntDate);
        public void SingleRollback(int hubid, int applicationid);
        public void HubRollback(int hubid, string deployedBy, string approvedBy, string requestedBy);

    }

}
