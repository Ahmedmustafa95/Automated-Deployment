using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace AutomatedDeployment.Api.Services
{
  public  interface IDeletefilesrepository
    {
        void Deletefiles(int hubid, int appid, List<IFormFile> files);
        string[] GetAllfiles(int hubid, int appid);
    }
}
