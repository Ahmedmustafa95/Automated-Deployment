using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.FactoryMethods
{
    public static class Factory
    {
        public static Deployment CreateDeployment(int hubId, int applicationID, DateTime deploymentDate, string approvedBy, string requestedBy)
        {
            return new Deployment()
            {
                HubID = hubId,
                AppID = applicationID,
                DeploymentDate = deploymentDate,
                ApprovedBy = approvedBy,
                RequestedBy = requestedBy,
            };
        }
        public static DeploymentFiles CreateDeploymentFile(int deploymentID, string fileName, status stat)
        {
            return new DeploymentFiles()
            {
                DeploymentID = deploymentID,
                FilesName = fileName,
                Status = stat
            };
        }


    }
}
