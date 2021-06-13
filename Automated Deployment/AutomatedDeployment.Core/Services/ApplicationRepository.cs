using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Services
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly EfgconfigurationdbContext _efgconfigurationdbContext;

        public ApplicationRepository(EfgconfigurationdbContext efgconfigurationdbContext)
        {
            this._efgconfigurationdbContext = efgconfigurationdbContext;
        }
        public Application Add(Application entity)
        {
            throw new NotImplementedException();
        }

        public Application Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Application> GetAll()
        {
            throw new NotImplementedException();
        }

        public Application GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Application Update(Application entity)
        {
            throw new NotImplementedException();
        }
    }
}
