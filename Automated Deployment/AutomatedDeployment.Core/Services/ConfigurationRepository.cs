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
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly EfgconfigurationdbContext _efgconfigurationdbContext;

        public ConfigurationRepository(EfgconfigurationdbContext efgconfigurationdbContext)
        {
            this._efgconfigurationdbContext = efgconfigurationdbContext;
        }
        public Configuration Add(Configuration entity)
        {
            throw new NotImplementedException();
        }

        public Configuration Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Configuration> GetAll()
        {
            throw new NotImplementedException();
        }

        public Configuration GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Configuration Update(Configuration entity)
        {
            throw new NotImplementedException();
        }
    }
}
