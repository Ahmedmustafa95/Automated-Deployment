using AutomatedDeployment.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace AutomatedDeployment.Core.Interfaces
{
    public  interface IPathRepository
    {
        string GetAssemblyPath(int applicationid, int hubid);
        string GetBackupPath(int applicationid, int hubid);

        string GetConfigFilePath(int applicationid, int hubid);
        HubsApplications GetHubApplication(int applicationid, int hubid);


        UploadStatus UploadAndStringManipulation(ConfigSearchResult configSearch, string deployedBy
                                                                         , string approvedBy
                                                                         , string requestedBy);


        DeploymentFiles AddDeploymentFiles(ConfigSearchResult configSearches);
        DeploymentDetails AddDeploymentDetailService(int hubId, int appId, int deploymentId);
        Deployment AddDeploymentService(string approvedBy, string requestedBy, string developedBy,
                                DateTime currentDate);
                                               
        bool CheckValidData(int hubid, int applicationid);

    }
}
