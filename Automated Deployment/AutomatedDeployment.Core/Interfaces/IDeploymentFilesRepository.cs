
﻿using AutomatedDeployment.Core.Interfaces.GenericRepositories;
using AutomatedDeployment.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces
{

    public interface IDeploymentFilesRepository:IGenericGetDictionaryByIDRepository<DeploymentFiles>
    {
        List<DeploymentFiles> AddDeploymentFiles(List<DeploymentFiles> deploymentFiles);
        DeploymentFiles AddDeploymentFile(DeploymentFiles deploymentFiles);
        Deployment GetLastDepolyment(int hubId, int applicationId);
        List<DeploymentDetails> GetLastDepolyment();



    }
}
