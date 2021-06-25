using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Core.Services;
using Microsoft.Extensions.DependencyInjection;

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
