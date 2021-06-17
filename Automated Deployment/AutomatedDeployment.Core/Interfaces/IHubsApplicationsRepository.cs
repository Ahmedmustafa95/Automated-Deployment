﻿using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IHubsApplicationsRepository: IGenericRepository<HubsApplications>
    {
        HubsApplications DeleteHubApplication(int HubID, int AppID);

        HubsApplications GetHubsApplicationByID(int HubID, int AppID);
    }
}
