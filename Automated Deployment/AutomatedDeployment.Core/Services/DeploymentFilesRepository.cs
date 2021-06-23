using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Core.Interfaces.GenericRepositories;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
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


        public DeploymentFilesRepository(EfgconfigurationdbContext efgconfigurationdbContext) =>
            _efgconfigurationdbContext = efgconfigurationdbContext;



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
        //    try
        //    {

        //        return _efgconfigurationdbContext.DeploymentFiles
        //                                  .Where(D => D.DeploymentID == GetLastDepolyment(hubID,applicationId).DeploymentID)
        //                                  .ToDictionary(D => D.FilesName, D => D.Status);
        //    }
        //    catch (Exception e)
        //    {
                return null;
        }

        public Deployment GetLastDepolyment(int hubId, int applicationId)
        {
            return null;
            // Error By change Database
            //try
            //{
            //  var deployment =  _efgconfigurationdbContext.Deployments
            //                                            .Where(D => D.HubID == hubId &&
            //                                                        D.AppID == applicationId)
            //                                            .OrderBy(D => D.DeploymentID)
            //                                            .LastOrDefault();
            //    return deployment;
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }



    }
}
