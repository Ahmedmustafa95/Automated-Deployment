﻿using AutomatedDeployment.Core.Interfaces.GenericRepositories;
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
            
    }
}
