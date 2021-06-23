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

    }
}
