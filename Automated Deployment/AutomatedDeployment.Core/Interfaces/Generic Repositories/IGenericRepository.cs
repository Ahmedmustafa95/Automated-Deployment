using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IGenericRepository<T> where T :class
    {
        IReadOnlyList<T> GetAll();
      
        T Add(T entity);
        T Update(T entity);
       
    }



}
