using AutomatedDeployment.Domain.Entities;
using System.Collections.Generic;

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
