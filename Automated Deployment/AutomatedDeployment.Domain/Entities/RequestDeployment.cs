using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
    public class RequestDeployment
    {
        public RequestDeployment()
        {
            RequestDeploymentHubsApplications = new HashSet<RequestDeploymentHubsApplications>();
        }
        [Key]
        public int Id { get; set; }
        public string SignOff { get; set; }
        public string RollbackPlan { get; set; }
        public DateTime DeploymentTime { get; set; }
        public string RequestBy { get; set; }

        public virtual ICollection<RequestDeploymentHubsApplications> RequestDeploymentHubsApplications { get; set; }

    }
}
