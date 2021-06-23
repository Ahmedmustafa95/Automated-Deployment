using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.FactoryMethods
{
  public static  class Factory
    {
        //public static Deployment CreateDeployment(int hubId, int applicationId,DateTime currentDate , string approvedBy,string requestedBy)
        //{
        //    return new Deployment() { HubID=hubId, 
        //                              AppID=applicationId , 
        //                              DeploymentDate=currentDate ,
        //                              ApprovedBy=approvedBy,
        //                              RequestedBy=requestedBy
        //                             };
        //}

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
