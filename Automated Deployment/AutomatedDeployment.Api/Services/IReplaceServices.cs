using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
    public interface IReplaceServices
    {
        void Upload(List<IFormFile> formFiles, string path);
    }
}
