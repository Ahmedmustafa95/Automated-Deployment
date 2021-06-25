using AutomatedDeployment.Core.Interfaces;

namespace AutomatedDeployment.Core.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public IApplicationRepository ApplicationRepository { get; }

        public IHubRepository HubRepository { get; }

        public IHubsApplicationsRepository HubsApplicationsRepository { get; }

        public IDeploymentRepository DeploymentRepository { get; }
        public IDeploymentFilesRepository DeploymentFilesRepository { get; }
        public IDeploymentDetailsRepository DeploymentDetailsRepository { get; }


        public UnitOfWork(

                IApplicationRepository applicationRepository,
                IHubRepository hubRepository,
                IHubsApplicationsRepository configurationRepository,
                IDeploymentRepository deploymentRepository,
                IDeploymentFilesRepository deploymentFilesRepository,
                IDeploymentDetailsRepository deploymentDetailsRepository


                )


        {
            ApplicationRepository = applicationRepository;
            HubRepository = hubRepository;
            HubsApplicationsRepository = configurationRepository;
            DeploymentRepository = deploymentRepository;
            DeploymentFilesRepository = deploymentFilesRepository;
            DeploymentDetailsRepository = deploymentDetailsRepository;
        }

    }
}
