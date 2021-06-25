using System.Collections.Generic;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IGenericRepository<T> where T :class
    {
        IReadOnlyList<T> GetAll();
      
        T Add(T entity);
        T Update(T entity);

       
    }



}
