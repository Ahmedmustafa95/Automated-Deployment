using AutomatedDeployment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection service
                                            , IConfiguration configuration)
        {
            service.AddDbContext<EfgconfigurationdbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MyDataBaseConnection"));
            });
        }
    }
}
