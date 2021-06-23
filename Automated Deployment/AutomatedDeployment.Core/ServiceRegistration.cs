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
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IHubRepository, HubRepository>();
            services.AddScoped<IApplicationRepository,ApplicationRepository>();
            services.AddScoped<IHubsApplicationsRepository, HubsApplicationsRepository>();
            services.AddScoped<IPathRepository, PathRepository>();
            services.AddScoped<IDeploymentRepository, DeploymentRepository>();
            services.AddScoped<IDeploymentFilesRepository, DeploymentFilesRepository>();
            services.AddScoped<IDeploymentDetailsRepository, DeploymentDetailsRepository>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<StringManipulationRepository>();

        }
    }
}
