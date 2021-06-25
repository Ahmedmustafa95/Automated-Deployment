using AutomatedDeployment.Core.Interfaces.GenericRepositories;
using AutomatedDeployment.Domain.Entities;
using System.Collections.Generic;

namespace AutomatedDeployment.Core.Interfaces
{

    public interface IDeploymentFilesRepository:IGenericGetDictionaryByIDRepository<DeploymentFiles>
    {
        List<DeploymentFiles> AddDeploymentFiles(List<DeploymentFiles> deploymentFiles);
        DeploymentFiles AddDeploymentFile(DeploymentFiles deploymentFiles);
        Deployment GetLastDepolyment(int hubId, int applicationId);
        List<LastDeploymentviewmodel> GetLastDepolyment();
        //.GetLastDepolyment()
        //List<Hubviewmodel> Gethubslastdeployment();
        //List<Applicationviewmodel> Getapplicationsforhubsatlastdeployment(int hubid);




    }
}
