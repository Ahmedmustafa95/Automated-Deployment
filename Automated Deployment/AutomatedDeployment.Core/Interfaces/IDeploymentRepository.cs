using AutomatedDeployment.Domain.Entities;
using System.Collections.Generic;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IDeploymentRepository
    {
        public IReadOnlyList<Deployment> GetAll();
    
        int GetDeploymentCounts(int hubID, int applicationId); 
        Deployment AddDeployment(Deployment  deployment);

        int GetCurrentDeploymentId();
        Deployment GetLastDeployment();

        string[] GetAllfiles(int hubid, int appid);

        bool DeleteFromDeployment(int deploymentId);
       List<deploywithfilesviewmodel> Getallwithfiles(int id);






    }
}
