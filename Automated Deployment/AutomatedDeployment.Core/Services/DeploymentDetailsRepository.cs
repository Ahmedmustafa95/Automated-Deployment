using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;

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
                Console.WriteLine(e.Message);
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
        public DeploymentDetails GetLastDepolymentDetails()
        {
            try
            {
                var deploymentDetails = _efgconfigurationdbContext.DeploymentDetails
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return -1;
            }
           

        }
        public List<int> GetApplicationID(int deploymentID,int hubId)
        {
            try
            {
                var deploymentDetailsHubs = _efgconfigurationdbContext.DeploymentDetails.Where(d => d.DeploymentId == deploymentID && d.HubId==hubId)
                                        .Select(d => d.AppId).ToList();
                return deploymentDetailsHubs;

            }
            catch
            {
                return null;
            }
        }
    }
}
