using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomatedDeployment.Core.Services
{
    public class PathRepository : IPathRepository
    {
        private readonly EfgconfigurationdbContext efgconfigurationdbContext;
        private readonly IUnitOfWork _unitOfWork;

        public PathRepository(EfgconfigurationdbContext _efgconfigurationdbContext
                             , IUnitOfWork unitOfWork)
        {
            efgconfigurationdbContext = _efgconfigurationdbContext;
            _unitOfWork = unitOfWork;
        }

        public DeploymentDetails AddDeploymentDetailService(int hubId, int appId, int deploymentId)
        {
            try
            {
                return new DeploymentDetails
                {
                    HubId = hubId,
                    AppId = appId,
                    DeploymentId = deploymentId
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public DeploymentFiles AddDeploymentFiles(ConfigSearchResult configSearches)
        {
            DeploymentFiles deploymentFile = default;
            if (configSearches is null) return null;

            try
            {

                int deploymentDetailId = _unitOfWork.DeploymentDetailsRepository
                                                     .GetDeploymentDetailsIdByHubIdAndAppId
                                                     (
                                                        configSearches.HubID,
                                                        configSearches.AppID
                                                     );
                if (deploymentDetailId == -1) return null;

                deploymentFile = new DeploymentFiles()
                { DeploymentDetailsId = deploymentDetailId, FilesName = configSearches.FileName, Status = status.Modified };


                return deploymentFile;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public Deployment AddDeploymentService(string approvedBy, string requestedBy, string developedBy,
                                  DateTime currentDate)
        {
            try
            {
                return new Deployment()
                {
                    DeploymentDate = currentDate,
                    DeploymentType = DeploymentType.Deployment,
                    DeployedBy = developedBy,
                    ApprovedBy = approvedBy,
                    RequestedBy = requestedBy,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool CheckValidData(int hubid, int applicationid) =>
         _unitOfWork.HubsApplicationsRepository
                   .GetHubsApplicationByID(hubid, applicationid) != null;


        public string GetAssemblyPath(int hubid, int applicationid)
        {
            HubsApplications result = efgconfigurationdbContext.HubsApplications.AsNoTracking().FirstOrDefault(c => c.AppID == applicationid && c.HubID == hubid);
            if (result is not null)
            {
                return result.AssemblyPath;

            }
            else
            {
                return null;
            }
        }

        public string GetBackupPath(int hubid, int applicationid)
        {
            HubsApplications result = efgconfigurationdbContext.HubsApplications.AsNoTracking().FirstOrDefault(c => c.AppID == applicationid && c.HubID == hubid);
            if (result != null)
            {
                return result.BackupPath;
            }
            else
            {
                return null;
            }
        }

        public string GetConfigFilePath(int hubid, int applicationid)
        {
            HubsApplications result = efgconfigurationdbContext.HubsApplications.AsNoTracking().FirstOrDefault(c => c.AppID == applicationid && c.HubID == hubid);
            if (result != null)
            {
                return result.ConfigFilepPath;

            }
            else
            {
                return null;
            }
        }

        public HubsApplications GetHubApplication(int applicationid, int hubid)
        {

            HubsApplications result = efgconfigurationdbContext.HubsApplications.Include(i => i.Application).Include(i => i.Hub)
                .AsNoTracking().FirstOrDefault(c => c.AppID == applicationid && c.HubID == hubid);
            return result;
        }

        public UploadStatus UploadAndStringManipulation(ConfigSearchResult configSearch, string deployedBy, string approvedBy, string requestedBy)
        {
            ConfigSearchResult configSearchResult = configSearch;
            var deploymentDetails = new List<DeploymentDetails>();
            var deploymentFile = new DeploymentFiles();
            var currentDate = DateTime.Now;

            var deployment = AddDeploymentService(approvedBy, requestedBy, deployedBy, currentDate);
            if (_unitOfWork.DeploymentRepository.AddDeployment(deployment) is null)
                return UploadStatus.DatabaseFailure;

            int currentDeploymentId = _unitOfWork.DeploymentRepository.GetCurrentDeploymentId();


            if (!CheckValidData(configSearch.HubID, configSearch.AppID)) return UploadStatus.NotValidData;
            var deploymentDetail = AddDeploymentDetailService(configSearch.HubID, configSearch.AppID, currentDeploymentId);
            deploymentDetails.Add(deploymentDetail);

            if (_unitOfWork.DeploymentDetailsRepository.AddDeploymentDetails(deploymentDetails) is null)
                return UploadStatus.DatabaseFailure;

            deploymentFile = AddDeploymentFiles(configSearchResult);
            if (_unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentFile) is null)
                return UploadStatus.DatabaseFailure;
            return UploadStatus.Success;
        }


    }
}
