﻿using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Services
{
    public class DeploymentRepository : IDeploymentRepository
    {
        private readonly EfgconfigurationdbContext _efgconfigurationdbContext;

        public DeploymentRepository(EfgconfigurationdbContext efgconfigurationdbContext)=>
            _efgconfigurationdbContext = efgconfigurationdbContext;

        public Deployment AddDeployment(Deployment deployment)
        {
            if (deployment is not null)
            {
                _efgconfigurationdbContext.Deployments.Add(deployment);
                _efgconfigurationdbContext.SaveChanges();
                return deployment;
            }
            return deployment;
        }

        public int GetDeploymentCounts(int hubID, int applicationId) =>
         _efgconfigurationdbContext.Deployments.Count(d => d.HubID == hubID && d.AppID == applicationId);
          
    }
}
