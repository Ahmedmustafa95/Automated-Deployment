using AutomatedDeployment.Domain.Entities;
using System;

namespace AutomatedDeployment.Core.FactoryMethods
{
    public static  class Factory
    {
        public static Deployment createdeployment( DateTime currentdate,DeploymentType deploymentType,int originaldeploymentID,
            string deployedBy ,string approvedby, string requestedby)
        {
            return new Deployment()
            {
                DeploymentDate= currentdate,
                DeploymentType=deploymentType,
                OriginalDeployment=originaldeploymentID,
                DeployedBy =deployedBy,
                ApprovedBy=approvedby,
                RequestedBy=requestedby
            };
        }

        public static DeploymentDetails createDeploymentDetails(int hubId, int appID, int deploymentID)
        {
            return new DeploymentDetails()
            {
                DeploymentId = deploymentID,
                HubId = hubId,
                AppId = appID
            };
        }

        //public static DeploymentFiles CreateDeploymentFile(int addedID,string fileName,status stat)
        //{
        //    return new  DeploymentFiles()
        //    {
        //        DeploymentID = addedID,
        //        FilesName = fileName,
        //        Status = stat
        //    };
        //}

        //public static ConfigSearchResult CreateConfigSearchResult (int appId, int hubId, string appName, string hubName ,
        //                        string ConfigurationSectionName, string ConfigurationName , string ConfigurationValue , string FileName)
        //{
        //    return new ConfigSearchResult()
        //    {
        //        AppID = appId,
        //        HubID = hubId,
        //        AppName = appName,
        //        HubName = hubName,
        //        ConfigurationSectionName = ConfigurationSectionName,
        //        ConfigurationName = ConfigurationName,
        //        ConfigurationValue = ConfigurationValue,
        //        FileName = FileName
        //    };
        //}

    }
}
