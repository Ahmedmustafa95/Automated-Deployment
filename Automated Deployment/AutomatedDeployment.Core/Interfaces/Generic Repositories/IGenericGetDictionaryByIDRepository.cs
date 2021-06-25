using AutomatedDeployment.Domain.Entities;
using System.Collections.Generic;

namespace AutomatedDeployment.Core.Interfaces.GenericRepositories
{
    public interface IGenericGetDictionaryByIDRepository<T> where T : class
    {
        Dictionary<string,status> GetById(int hubID,int applicationId);

    }
}
