using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
    public class RequestDeploymentHubsApplications
    {
        public int HubId { get; set; }
        public Hub Hub { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public int RequestDeploymentId { get; set; }
        public RequestDeployment RequestDeployment { get; set; }
    }
}
