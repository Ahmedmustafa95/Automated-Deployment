using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
    public class PathRepository : IPathRepository
    {
        private readonly EfgconfigurationdbContext efgconfigurationdbContext;
        public PathRepository(EfgconfigurationdbContext _efgconfigurationdbContext)
        {
            efgconfigurationdbContext = _efgconfigurationdbContext;
        }
        public string GetAssemblyPath(int applicationid, int hubid)
        {
          var result=  efgconfigurationdbContext.Configurations.FirstOrDefault(c => c.AppID == applicationid && c.HubID == hubid);
            if(result!=null)
            {
                return result.AssemblyPath;

            }else
            {
                return null;
            }
        }

        public string GetConficPath(int applicationid, int hubid)
        {
            //var result = efgconfigurationdbContext.Configurations.FirstOrDefault(c => c.AppID == applicationid && c.HubID == hubid);
            //if (result != null)
            //{
            //    return result.ConfigurationPath;

            //}
            //else
            //{
            //    return null;
            //}
            return @"F:\"+applicationid+@"\"+hubid+@"\";

        }
    }
}
