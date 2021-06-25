using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Core.Interfaces.GenericRepositories;
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
    public class DeploymentFilesRepository : IDeploymentFilesRepository
    {
        private readonly EfgconfigurationdbContext _efgconfigurationdbContext;
        private readonly IDeploymentDetailsRepository _deploymentDetailsRepository;

        // private readonly UnitOfWork _unitOfWork;

        public DeploymentFilesRepository(EfgconfigurationdbContext efgconfigurationdbContext,IDeploymentDetailsRepository deploymentDetailsRepository)
        {
            _efgconfigurationdbContext = efgconfigurationdbContext;
            _deploymentDetailsRepository = deploymentDetailsRepository;
          //  _unitOfWork = unitOfWork;
        }




        public List<DeploymentFiles> AddDeploymentFiles(List<DeploymentFiles> deploymentFiles)
        {
            if (deploymentFiles is not null)
            {
                try
                {
                    _efgconfigurationdbContext.DeploymentFiles.AddRange(deploymentFiles);
                    _efgconfigurationdbContext.SaveChanges();
                    return deploymentFiles;
                }
                catch
                {
                    return null;
                }

            }
            return null;
        }
        public DeploymentFiles AddDeploymentFile(DeploymentFiles deploymentFile)
        {
            if (deploymentFile is not null)
            {
                try
                {
                   var df = _efgconfigurationdbContext.DeploymentFiles.Add(deploymentFile);
                    _efgconfigurationdbContext.SaveChanges();
                    return df.Entity;
                }
                catch
                {
                    return null;
                }

            }
            return null;
        }

        public IReadOnlyList<DeploymentFiles> GetAll()
        {
            throw new NotImplementedException();
        }

        public DeploymentFiles Update(DeploymentFiles entity)
        {
            throw new NotImplementedException();
        }


        // Error By change Database
        public Dictionary<string, status> GetById(int hubID, int applicationId)
        {
            try
            {

                    var files = _efgconfigurationdbContext.DeploymentFiles
                                          .Where(D => D.DeploymentDetailsId == _deploymentDetailsRepository.GetLastDepolymentDetails(hubID,applicationId).DeploymentDetailsId)
                                          .ToDictionary(D => D.FilesName, D => D.Status);
                return files;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Deployment GetLastDepolyment(int hubId, int applicationId)
        {
           // return null;
            // Error By change Database
            try
            {

                var deployment = _efgconfigurationdbContext.Deployments
                                 .Where(d => d.DeploymentID == _deploymentDetailsRepository
                                 .GetLastDepolymentDetails(hubId, applicationId).DeploymentId)
                                 .SingleOrDefault();

                return deployment;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<LastDeploymentviewmodel> GetLastDepolyment()
        {
            // return null;
            // Error By change Database
            try
            {

                //  var deployment = _efgconfigurationdbContext.Deployments.Include(d => d.DeploymentDetails.SelectMany(d.HubId, d.AppId }));
                var deployment = _efgconfigurationdbContext.Deployments
                                    .Include(d => d.DeploymentDetails)
                                        .ThenInclude(i=>i.HubsApplications)
                                            .ThenInclude(i=>i.Application)
                                    .Include(d => d.DeploymentDetails)
                                        .ThenInclude(i => i.HubsApplications)
                                            .ThenInclude(i => i.Hub)
                                .OrderBy(i=>i.DeploymentID).LastOrDefault();

                List<LastDeploymentviewmodel> lastdeploy = deployment.DeploymentDetails
                    .Select(i => new LastDeploymentviewmodel { appId = i.AppId,hubId = i.HubId ,
                        ApplicationName=i.HubsApplications.Application.AppName,
                        HubName=i.HubsApplications.Hub.HubName
                    }).ToList();

                return lastdeploy;

              
            }
            catch (Exception ex)
            {

                return null;
            }
        }
      /*  public List<Hubviewmodel> Gethubslastdeployment()
        {
            // return null;
            // Error By change Database
            try
            {

                 var deployment = _efgconfigurationdbContext.Deployments.OrderBy(d=>d.DeploymentID).LastOrDefault();
                var res = _efgconfigurationdbContext.DeploymentDetails.Where(d => d.DeploymentId == deployment.DeploymentID).Include(d => d.HubsApplications).ThenInclude(d => d.Hub).ToList();

                HashSet<int> hubsid = new HashSet<int>();
                List<Hubviewmodel> hubs = new List<Hubviewmodel>();

                foreach (var x  in res)
                {
                    if (!hubsid.Contains(x.HubId))
                        {
                        var hub = new Hubviewmodel();
                        hub.HubID = x.HubId;
                        hub.HubName = x.HubsApplications.Hub.HubName;
                        hubs.Add(hub);
                        hubsid.Add(x.HubId);
                        }
                }


                  
                
                return hubs.ToList();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

     public List<Applicationviewmodel> Getapplicationsforhubsatlastdeployment(int hubid)
        {
            var deployment = _efgconfigurationdbContext.Deployments.OrderBy(d => d.DeploymentID).LastOrDefault();
            var res = _efgconfigurationdbContext.DeploymentDetails.Where(d => d.DeploymentId == deployment.DeploymentID).Include(d => d.HubsApplications).ThenInclude(d => d.Application).ToList();
            var deploymentDetailsathubs = res.Where(x => x.HubId == hubid);
            List<Applicationviewmodel> apps = new List<Applicationviewmodel>();

            foreach (var x in res)
            {
               
                    var app = new Applicationviewmodel();
                    app.AppID = x.AppId;
                    app.AppName = x.HubsApplications.Application.AppName;
                    apps.Add(app);
           
            }
            return apps;
        }
    */
    }
}
