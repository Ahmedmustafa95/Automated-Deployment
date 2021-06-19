using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces.GenericRepositories
{
    public interface IGenericGetDictionaryByIDRepository<T> where T : class
    {
        Dictionary<string,status> GetById(int hubID,int applicationId);

    }
}
