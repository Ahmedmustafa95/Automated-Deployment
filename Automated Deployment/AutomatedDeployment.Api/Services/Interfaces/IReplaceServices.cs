using AutomatedDeployment.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace AutomatedDeployment.Api.Services
{
    public interface IReplaceServices
    {
         UploadStatus UploadAndBackupFiles(UploadingFileViewModel fileViewModel);
         List<DeploymentFiles> AddDeploymentFiles(Dictionary<string, List<string>> filesState,
                                       List<HubsApplications> hubsApplications);
         DeploymentDetails AddDeploymentDetailService(int hubId, int appId, int deploymentId);
         Deployment AddDeploymentService(string approvedBy, string requestedBy, string developedBy,
                                 DateTime currentDate);
        Dictionary<string, List<string>> GenerateAndMoveToBackupFolder(List<IFormFile> files
                                                , string assemblyPath,
                                                 DateTime backupDate);
        bool CheckValidData(int hubid, int applicationid);
        bool Upload(List<IFormFile> files, string path);
        Dictionary<string, List<string>> CompareDeployFilesWithAssemblyFiles(List<IFormFile> Uploadedfiles, string assemblyPath);

        bool RemoveDeploymentFromDatabase(int deploymentId);

    }
}
