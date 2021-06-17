using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IDeploymentFilesRepository
    {
        List<DeploymentFiles> AddDeploymentFiles(List<DeploymentFiles> deploymentFiles);

    }
}
