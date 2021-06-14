using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace AutomatedDeployment.Api.Services
{
  public  interface IReplaceService
    {
    void Upload(List<IFormFile> formFiles);

    }
}


    