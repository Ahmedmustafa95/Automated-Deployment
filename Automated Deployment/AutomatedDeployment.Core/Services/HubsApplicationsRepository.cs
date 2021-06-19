using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Services
{
    public class HubsApplicationsRepository : IHubsApplicationsRepository
    {
        private readonly EfgconfigurationdbContext _efgconfigurationdbContext;

        public HubsApplicationsRepository(EfgconfigurationdbContext efgconfigurationdbContext)
        {
            this._efgconfigurationdbContext = efgconfigurationdbContext;
        }

        public HubsApplications Add(HubsApplications entity)
        {
            _efgconfigurationdbContext.HubsApplications.Add(entity);
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
        public HubsApplications DeleteHubApplication(int HubID, int AppID)
        {
            try
            {
                HubsApplications hubsApplications = _efgconfigurationdbContext.HubsApplications
                                                   .FirstOrDefault(i => i.AppID == AppID && i.HubID == HubID);
                _efgconfigurationdbContext.HubsApplications.Remove(hubsApplications);
                _efgconfigurationdbContext.SaveChanges();
                return hubsApplications;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IReadOnlyList<HubsApplications> GetAll()
        {
            return _efgconfigurationdbContext.HubsApplications.Include(h=>h.Application).Include(h=>h.Hub).AsNoTracking().ToList();
        }

        public HubsApplications GetHubsApplicationByID(int HubID, int AppID)
        {
            return _efgconfigurationdbContext.HubsApplications.AsNoTracking()
                                             .FirstOrDefault(i => i.AppID == AppID 
                                                               && i.HubID == HubID);
        }

        public HubsApplications Update(HubsApplications entity,int id)
        {
            if (entity is HubsApplications && entity != null)
            {
                try
                {
                    var obj = GetHubsApplicationByID(entity.HubID, id);
                    _efgconfigurationdbContext.Remove(obj);
                    _efgconfigurationdbContext.SaveChanges();

                    obj.AppID = entity.AppID;

                    _efgconfigurationdbContext.Add(obj);
                   // _efgconfigurationdbContext.Entry(entity).State = EntityState.Modified;
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

        public HubsApplications Update(HubsApplications entity)
        {
            throw new NotImplementedException();
        }
    }
}
