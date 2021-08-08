using AutomatedDeployment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace AutomatedDeployment.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection service
                                            , IConfiguration configuration)
        {
            service.AddDbContext<EfgconfigurationdbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MyDataBaseConnection"))
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                        LogLevel.Information)
                       .EnableSensitiveDataLogging();
            });
        }
    }
}
