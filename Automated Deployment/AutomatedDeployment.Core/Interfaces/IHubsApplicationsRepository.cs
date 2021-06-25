using AutomatedDeployment.Domain.Entities;
using System.Collections.Generic;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IHubsApplicationsRepository
    {
        HubsApplications DeleteHubApplication(int HubID, int AppID);

        HubsApplications GetHubsApplicationByID(int HubID, int AppID);
        IReadOnlyList<HubsApplications> GetAll();

        HubsApplications Add(HubsApplications entity);
        HubsApplications Update(HubsApplications entity);

    }
}
