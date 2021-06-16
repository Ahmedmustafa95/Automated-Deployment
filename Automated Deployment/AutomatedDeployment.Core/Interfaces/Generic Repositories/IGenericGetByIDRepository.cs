using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Core.Interfaces
{
    public interface IGenericGetByIDRepository<T> where T : class
    {
        T GetById(int id);
    }
}
