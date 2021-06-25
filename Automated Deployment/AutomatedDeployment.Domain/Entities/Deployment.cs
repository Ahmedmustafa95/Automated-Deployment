using System;
using System.Collections.Generic;

namespace AutomatedDeployment.Domain.Entities
{
    public class Deployment
    {
        public int DeploymentID { get; set; }
        public DateTime DeploymentDate { get; set; }
        public DeploymentType DeploymentType { get; set; }
        public int? OriginalDeployment { get; set; }
        public string DeployedBy { get; set; }
        public string RequestedBy { get; set; }
        public string ApprovedBy { get; set; }
        public ICollection<DeploymentDetails> DeploymentDetails { get; set;} 
            = new HashSet<DeploymentDetails>();

    }
}
