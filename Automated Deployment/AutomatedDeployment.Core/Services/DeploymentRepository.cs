using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutomatedDeployment.Core.Services
{
    public class DeploymentRepository : IDeploymentRepository
    {
        private readonly EfgconfigurationdbContext _efgconfigurationdbContext;

        public DeploymentRepository(EfgconfigurationdbContext efgconfigurationdbContext)=>
            _efgconfigurationdbContext = efgconfigurationdbContext;


        public IReadOnlyList<Deployment> GetAll()
        {

            try
            {
                return _efgconfigurationdbContext.Deployments.AsNoTracking().ToList();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }
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

        public int GetDeploymentCounts(int hubID, int applicationId) =>
                _efgconfigurationdbContext.DeploymentDetails.Count(d => d.HubId == hubID && d.AppId == applicationId);



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

        public bool DeleteFromDeployment(int deploymentId)
        {
            try
            {
                var deletedDeployment = _efgconfigurationdbContext.Deployments.Find(deploymentId);
                if (deletedDeployment is null) return false;
                _efgconfigurationdbContext.Deployments.Remove(deletedDeployment);
                _efgconfigurationdbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return false;
            }

        }
        public Deployment GetLastDeployment()
        {
            try
            {
                Deployment deployment =_efgconfigurationdbContext.Deployments
        
                                                          .OrderBy(D => D.DeploymentID)
                                                          .LastOrDefault();

                return deployment;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public  List<deploywithfilesviewmodel> Getallwithfiles(int id)
        {
            try
            {
                var deploymentfiles = _efgconfigurationdbContext.DeploymentFiles
                       .Include(d => d.DeploymentDetails)
                         .ThenInclude(h => h.HubsApplications).
                           ThenInclude(h => h.Hub)
                       .Include(d => d.DeploymentDetails)
                         .ThenInclude(h => h.HubsApplications)
                           .ThenInclude(h => h.Application).ToList();
                var deployfiles = deploymentfiles.Where(d => d.DeploymentDetails.DeploymentId == id).ToList();

                var deploywithfiles = deployfiles.Select(i => new deploywithfilesviewmodel
                {
                    status = i.Status,
                    filesname = i.FilesName,
                    hubname = i.DeploymentDetails.HubsApplications.Hub.HubName,
                    appname = i.DeploymentDetails.HubsApplications.Application.AppName
                }).ToList();
                return deploywithfiles;






            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }


}
