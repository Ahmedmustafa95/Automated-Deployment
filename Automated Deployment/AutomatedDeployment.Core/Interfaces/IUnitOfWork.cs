using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IApplicationRepository ApplicationRepository { get; }
        IHubRepository HubRepository { get; }
        IHubsApplicationsRepository HubsApplicationsRepository { get; }
        IDeploymentRepository  DeploymentRepository { get; }
        
    }
}
