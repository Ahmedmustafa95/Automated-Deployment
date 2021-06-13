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
            IConfigurationRepository configurationRepository
            )
        {
            ApplicationRepository = applicationRepository;
            HubRepository = hubRepository;
            ConfigurationRepository = configurationRepository;

        }
        public IApplicationRepository ApplicationRepository { get; }

        public IHubRepository HubRepository { get; }

        public IConfigurationRepository ConfigurationRepository { get; }
    }
}
