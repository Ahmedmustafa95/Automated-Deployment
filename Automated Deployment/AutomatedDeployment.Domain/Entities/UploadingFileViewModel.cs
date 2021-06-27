using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AutomatedDeployment.Domain.Entities
{
    public class UploadingFileViewModel
    {
        public List<IFormFile> files { get; set; }
        public List<HubsApplications> HubsApplications { get; set; }
        public string ApprovedBy { get; set; }
        public string RequestedBy { get; set; }
        public string DeployedBy { get; set; }
    }
}
