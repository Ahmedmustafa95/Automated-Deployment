using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection service)
        {
            service.AddScoped<IHubRepository, HubRepository>();
            service.AddScoped<IApplicationRepository,ApplicationRepository>();
            service.AddScoped<IHubsApplicationsRepository, HubsApplicationsRepository>();
            service.AddScoped<IPathRepository, PathRepository>();
            service.AddScoped<IDeploymentRepository, DeploymentRepository>();
        }
    }
}
