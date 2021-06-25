namespace AutomatedDeployment.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IApplicationRepository ApplicationRepository { get; }
        IHubRepository HubRepository { get; }
        IHubsApplicationsRepository HubsApplicationsRepository { get; }
        IDeploymentRepository  DeploymentRepository { get; }
        IDeploymentFilesRepository DeploymentFilesRepository { get; }
        IDeploymentDetailsRepository DeploymentDetailsRepository { get; }
    }
}
