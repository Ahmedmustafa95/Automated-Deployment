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
    class DeploymentDetailsRepository : IDeploymentDetailsRepository
    {
        private readonly EfgconfigurationdbContext _efgconfigurationdbContext;
      //  private readonly UnitOfWork _unitOfWork;

        public DeploymentDetailsRepository(EfgconfigurationdbContext efgconfigurationdbContext)
        {
            _efgconfigurationdbContext = efgconfigurationdbContext;
          //  _unitOfWork = unitOfWork;
        }

           public DeploymentDetails AddDeploymentDetails(DeploymentDetails deploymentDetails)
        {
            if (deploymentDetails is not null)
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
            return null;
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
    }
}
