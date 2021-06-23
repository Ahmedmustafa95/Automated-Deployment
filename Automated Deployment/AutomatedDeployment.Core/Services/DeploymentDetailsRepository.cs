using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Services
{

    public class DeploymentDetailsRepository : IDeploymentDetailsRepository
    {
        private readonly EfgconfigurationdbContext _efgconfigurationdbContext;


        public DeploymentDetailsRepository(EfgconfigurationdbContext efgconfigurationdbContext) =>
            _efgconfigurationdbContext = efgconfigurationdbContext;
        public List<DeploymentDetails> AddDeploymentDetails(List<DeploymentDetails> deploymentDetails)
        {
            if (deploymentDetails is not null)
            {
                try
                {
                    _efgconfigurationdbContext.DeploymentDetails.AddRange(deploymentDetails);
                    _efgconfigurationdbContext.SaveChanges();
                    return deploymentDetails;
                }
                catch
                {
                    return null;
                }

            }
            return null;
        }

        public int GetCurrentDeploymentDetailsId() =>
            _efgconfigurationdbContext.DeploymentDetails.Max(d => d.DeploymentDetailsId)+1;
            
        public DeploymentDetails AddDeploymentDetails(DeploymentDetails deploymentDetails)
        {
          try
          {
              var deployment1= _efgconfigurationdbContext.DeploymentDetails.Add(deploymentDetails);
               _efgconfigurationdbContext.SaveChanges();
                return deployment1.Entity;
          }
           catch(Exception e)
                {
                    return null;
                }
        }
        public DeploymentDetails GetLastDepolymentDetails(int hubId, int applicationId)
        {
            try
            {
                var deploymentDetails = _efgconfigurationdbContext.DeploymentDetails
                                                          .Where(D => D.HubId == hubId &&
                                                                      D.AppId == applicationId)
                                                          .OrderBy(D => D.DeploymentDetailsId)
                                                          .LastOrDefault();
                return deploymentDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetDeploymentDetailsIdByHubIdAndAppId(int hubId, int appId)
        {
            try
            {
                var deploymentDetailId = _efgconfigurationdbContext.DeploymentDetails
                                                            .OrderBy(D => D.DeploymentDetailsId)
                                                            .LastOrDefault
                                                            (
                                                               D => D.HubId == hubId &&
                                                               D.AppId == appId
                                                            );
                return deploymentDetailId?.DeploymentDetailsId ?? -1;
            }
            catch (Exception E)
            {

                return -1;
            }
           

        }
    }
}
