using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
   public interface IRollbackService
    {
        public void Rollback(string backuppath,string assemblypath);
    }

}
