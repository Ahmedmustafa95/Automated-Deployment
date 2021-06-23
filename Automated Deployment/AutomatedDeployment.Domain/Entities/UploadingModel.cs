using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
  public  class UploadingModel
  {
      public  List<IFormFile> files { get; set; }
      public  List<HubsApplications> HubsApplications { get; set; }
   
    }
}
