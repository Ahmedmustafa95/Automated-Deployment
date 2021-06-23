using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Services
{
    public class DeploymentRepository : IDeploymentRepository
    {
        private readonly EfgconfigurationdbContext _efgconfigurationdbContext;

        public DeploymentRepository(EfgconfigurationdbContext efgconfigurationdbContext)=>
            _efgconfigurationdbContext = efgconfigurationdbContext;

        public Deployment AddDeployment(Deployment deployment)
        {
            if (deployment is not null)
            {
                try
                {
                   var deployment1= _efgconfigurationdbContext.Deployments.Add(deployment);
                    _efgconfigurationdbContext.SaveChanges();
                    return deployment1.Entity;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
               
            }
            return null;
        }

        public int GetCurrentDeploymentId()=>
            _efgconfigurationdbContext.Deployments.Max(d=>d.DeploymentID);



        public int GetDeploymentCounts(int hubID, int applicationId) => 0;

        // Error By change Database
        // _efgconfigurationdbContext.Deployments.Count(d => d.HubID == hubID && d.AppID == applicationId);

        public string[] GetAllfiles(int hubid, int appid)
        {
            try
            {
                var assembly = _efgconfigurationdbContext.HubsApplications.Where(h => h.HubID == hubid && h.AppID == appid).FirstOrDefault();
                string[] filePaths = Directory.GetFiles(assembly.AssemblyPath);
                string[] filanames = new string [filePaths.Length];
   
                string[] subs; 
               for(int i=0 ; i< filePaths.Length; i++)
                {
                    subs = filePaths[i].Split(@"\");
                    filanames[i] = subs[subs.Length - 1];
                }
                return filanames;
            }
            catch
            {
                return null;
            }

        }
    }


}
