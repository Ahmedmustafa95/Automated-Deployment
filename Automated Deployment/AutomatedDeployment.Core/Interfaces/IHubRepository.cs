using AutomatedDeployment.Domain.Entities;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IHubRepository:IGenericRepository<Hub>, IGenericDeleteRepository<Hub>, IGenericGetByIDRepository<Hub>
    {
        
    }
}
