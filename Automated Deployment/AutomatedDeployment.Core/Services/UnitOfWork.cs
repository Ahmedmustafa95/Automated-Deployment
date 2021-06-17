using AutomatedDeployment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public IApplicationRepository ApplicationRepository { get; }

        public IHubRepository HubRepository { get; }

        public IHubsApplicationsRepository HubsApplicationsRepository { get; }

        public IDeploymentRepository DeploymentRepository { get; }
        public IDeploymentFilesRepository DeploymentFilesRepository { get; }

        public UnitOfWork(
                IApplicationRepository applicationRepository,
                IHubRepository hubRepository,
                IHubsApplicationsRepository configurationRepository,
                IDeploymentRepository deploymentRepository,
                IDeploymentFilesRepository deploymentFilesRepository
                )
        {
            ApplicationRepository = applicationRepository;
            HubRepository = hubRepository;
            HubsApplicationsRepository = configurationRepository;
            DeploymentRepository = deploymentRepository;
            DeploymentFilesRepository = deploymentFilesRepository;
        }

    } 
}
