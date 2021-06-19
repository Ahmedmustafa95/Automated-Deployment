using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IHubsApplicationsRepository
    {
        HubsApplications DeleteHubApplication(int HubID, int AppID);

        HubsApplications GetHubsApplicationByID(int HubID, int AppID);
        IReadOnlyList<HubsApplications> GetAll();

        HubsApplications Add(HubsApplications entity);
        HubsApplications Update(HubsApplications entity,int id);

    }
}
