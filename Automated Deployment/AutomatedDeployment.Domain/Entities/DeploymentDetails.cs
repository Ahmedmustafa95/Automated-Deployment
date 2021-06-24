using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
    public class DeploymentDetails
    {
        public int DeploymentDetailsId { get; set; }
        public int HubId { get; set; }
        public int AppId { get; set; }
        public HubsApplications HubsApplications { get; set; }
        public int DeploymentId { get; set; }
        public Deployment Deployment { get; set; }
        public ICollection<DeploymentFiles> DeploymentFiles { get; set; } = 
            new HashSet<DeploymentFiles>();
    }
}
