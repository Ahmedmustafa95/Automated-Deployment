using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IApplicationRepository:IGenericRepository<Application>
    {
        public List<Application> GetAppsByHubID(int hubID);
    }
}
