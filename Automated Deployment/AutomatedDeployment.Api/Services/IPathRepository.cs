using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
  public  interface IPathRepository
    {
        string GetAssemblyPath(int applicationid, int hubid);
        string GetConficPath(int applicationid, int hubid);


    }
}
