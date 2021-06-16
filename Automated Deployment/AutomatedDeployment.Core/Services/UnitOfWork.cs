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
        public UnitOfWork(
            IApplicationRepository applicationRepository,
            IHubRepository hubRepository,
            IHubsApplicationsRepository configurationRepository
            )
        {
            ApplicationRepository = applicationRepository;
            HubRepository = hubRepository;
            HubsApplicationsRepository = configurationRepository;

        }

        public IApplicationRepository ApplicationRepository { get; }

        public IHubRepository HubRepository { get; }

        public IHubsApplicationsRepository HubsApplicationsRepository { get; }
    }
}
