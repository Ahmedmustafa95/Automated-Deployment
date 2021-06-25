using AutomatedDeployment.Domain.Entities;
using System.Collections.Generic;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IApplicationRepository:IGenericRepository<Application>,IGenericDeleteRepository<Application>,
                                            IGenericGetByIDRepository<Application>
    {
        public List<HubsApplications> GetAppsByHubID(int hubID);
    }
}
