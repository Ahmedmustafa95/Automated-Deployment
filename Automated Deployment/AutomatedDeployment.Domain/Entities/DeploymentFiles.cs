using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
    public class DeploymentFiles
    {
        
        public int DeploymentFilesID { get; set; }

        public int DeploymentDetailsId { get; set; }

        public string FilesName { get; set; }

        public status Status { get; set; }

        public DeploymentDetails DeploymentDetails { get; set; }

    }
}
