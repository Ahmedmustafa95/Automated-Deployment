using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
    public class DeploymentFiles
    {
        public int ID { get; set; }
        public int DeploymentID { get; set; }

        public string FilesName { get; set; }

        public status Status { get; set; }

        public virtual Deployment Deployments { get; set; } 

    }
}
