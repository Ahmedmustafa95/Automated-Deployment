using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AutomatedDeployment.Core.Services
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly EfgconfigurationdbContext _efgconfigurationdbContext;

        public ApplicationRepository(EfgconfigurationdbContext efgconfigurationdbContext)
        {
            this._efgconfigurationdbContext = efgconfigurationdbContext;
        }
        public IReadOnlyList<Application> GetAll()
        {
<<<<<<< HEAD
            return _efgconfigurationdbContext.Applications.Include(a=>a.Configurations).ThenInclude(a=>a.Hub).ToList();
=======
            return _efgconfigurationdbContext.Applications.AsNoTracking().ToList();
>>>>>>> 26ea56ad2940a100a098435abe95ab9188fd0d6c
        }

        public Application GetById(int id)
        {
            try
            {
                var application = _efgconfigurationdbContext.Applications.AsNoTracking().FirstOrDefault(i => i.AppID == id);
                return application;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Application Add(Application entity)
        {

            if (entity is Application)
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
            }
            else
                return null;

        }

        public Application Delete(int id)
        {
            try
            {
                var application = _efgconfigurationdbContext.Applications.Find(id);
                _efgconfigurationdbContext.Applications.Remove(application);
                _efgconfigurationdbContext.SaveChanges();
                return application;
            }
            catch (Exception)
            {
                return null;
            }
        }



        public Application Update(Application entity)
        {
            if (entity is Application && entity != null)
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
        public List<Application> GetAppsByHubID(int hubID)
        {
            List<Application> Apps = _efgconfigurationdbContext.HubsApplications.Where(h => h.HubID == hubID)
                .Include(a => a.Application).Select(a => a.Application).AsNoTracking().ToList();
            return Apps;
        }


      


    }
}
