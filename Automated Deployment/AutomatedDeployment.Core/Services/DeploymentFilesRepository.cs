﻿using AutomatedDeployment.Core.Interfaces;
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

        public IReadOnlyList<DeploymentFiles> GetAll()
        {
            throw new NotImplementedException();
        }

        public DeploymentFiles Update(DeploymentFiles entity)
        {
            throw new NotImplementedException();
=======
        public DeploymentFilesRepository(EfgconfigurationdbContext efgconfigurationdbContext)
            => this._efgconfigurationdbContext = efgconfigurationdbContext;

        public Dictionary<string,status> GetById(int hubID, int applicationId)
        {
            try
            {
                var lastDeployemt = _efgconfigurationdbContext.Deployments
                                                         .Where(D => D.HubID == hubID &&
                                                                     D.AppID == applicationId)
                                                         .LastOrDefault();
                return _efgconfigurationdbContext.DeploymentFiles
                                          .Where(D => D.DeploymentID == lastDeployemt.DeploymentID)
                                          .ToDictionary(D => D.FilesName,D => D.Status);
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}