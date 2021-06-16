﻿using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IHubRepository:IGenericRepository<Hub>, IGenericDeleteRepository<Hub>, IGenericGetByIDRepository<Hub>
    {
        
    }
}
