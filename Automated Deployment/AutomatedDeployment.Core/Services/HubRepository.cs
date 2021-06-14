using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace AutomatedDeployment.Core.Services
{
    public class HubRepository : IHubRepository
    {
        private readonly EfgconfigurationdbContext _efgconfigurationdbContext;

        public HubRepository(EfgconfigurationdbContext efgconfigurationdbContext)
        {
            this._efgconfigurationdbContext = efgconfigurationdbContext;
        }
        public Hub Add(Hub entity)
        {
            throw new NotImplementedException();
            //if (entity is Hub)
            //{
            //    _efgconfigurationdbContext.Add(entity);
            //    try
            //    {
            //        _efgconfigurationdbContext.SaveChanges();
            //    }
            //    catch (Exception)
            //    {

            //    }
            //}
        }

        public Hub Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Hub> GetAll()
        {
            return _efgconfigurationdbContext.Hubs.ToList();
        }

        public Hub GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Hub Update(Hub entity)
        {
            throw new NotImplementedException();
        }
    }
}
