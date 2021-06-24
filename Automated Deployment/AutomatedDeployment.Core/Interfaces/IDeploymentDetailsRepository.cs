using AutomatedDeployment.Core.Interfaces.GenericRepositories;
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
        List<DeploymentDetails> AddDeploymentDetails(List<DeploymentDetails> deploymentDetails);
        int GetCurrentDeploymentDetailsId();
        public DeploymentDetails AddDeploymentDetails(DeploymentDetails deploymentDetails);
        DeploymentDetails GetLastDepolymentDetails(int hubId, int applicationId);

        int GetDeploymentDetailsIdByHubIdAndAppId(int hubId, int appId);
        public DeploymentDetails GetLastDepolymentDetails();
      //  public DeploymentDetails GetDeploymentDetailsWithDeploymentID(int deploymentID);
        public List<int> GetApplicationID(int deploymentID, int hubId);
    }
}
