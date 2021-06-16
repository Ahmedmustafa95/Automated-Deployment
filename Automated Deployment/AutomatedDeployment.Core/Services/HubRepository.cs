using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace AutomatedDeployment.Core.Services
{
    public class HubRepository : IHubRepository
    {
        private readonly EfgconfigurationdbContext _efgconfigurationdbContext;

        public HubRepository(EfgconfigurationdbContext efgconfigurationdbContext)
        {
            this._efgconfigurationdbContext = efgconfigurationdbContext;
        }
        public IReadOnlyList<Hub> GetAll()
        {
            return _efgconfigurationdbContext.Hubs.AsNoTracking().ToList();
        }

        public Hub GetById(int id)
        {
            try
            {
                var hub = _efgconfigurationdbContext.Hubs.AsNoTracking().FirstOrDefault(i => i.HubID == id);
                return hub;
            }
            catch(Exception)
            {
                return null;
            }
        }
        public Hub Add(Hub entity)
        {

            if (entity is Hub)
            {
                _efgconfigurationdbContext.Add(entity);
                try
                {
                    _efgconfigurationdbContext.SaveChanges();
                    return entity;
                }
                catch (Exception)
                {
                    return null;
                }
            }else
                return null;
            
        }

        public Hub Delete(int id)
        {
            try
            {
               var hub =  _efgconfigurationdbContext.Hubs.Find(id);
                _efgconfigurationdbContext.Hubs.Remove(hub);
                _efgconfigurationdbContext.SaveChanges();
                return hub;
            }
            catch(Exception)
            {
                return null;
            }
        }

    

        public Hub Update(Hub entity)
        {
            if (entity is Hub && entity != null)
            {
                try
                {

                    _efgconfigurationdbContext.Entry(entity).State = EntityState.Modified;
                    _efgconfigurationdbContext.SaveChanges();
                    return entity;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;

     
        }
    }
}
