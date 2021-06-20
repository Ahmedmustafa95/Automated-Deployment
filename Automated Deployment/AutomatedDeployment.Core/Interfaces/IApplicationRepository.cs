using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IApplicationRepository:IGenericRepository<Application>,IGenericDeleteRepository<Application>,
                                            IGenericGetByIDRepository<Application>
    {
        public List<HubsApplications> GetAppsByHubID(int hubID);
    }
}
