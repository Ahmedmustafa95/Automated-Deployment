using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces
{
   public interface IDeploymentDetailsRepository
    {
        public DeploymentDetails AddDeploymentDetails(DeploymentDetails deploymentDetails);
        DeploymentDetails GetLastDepolymentDetails(int hubId, int applicationId);
    }

}
