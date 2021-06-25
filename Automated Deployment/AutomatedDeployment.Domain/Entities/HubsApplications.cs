using System.Collections.Generic;

namespace AutomatedDeployment.Domain.Entities
{
    public class HubsApplications
    {
        public virtual Hub Hub { get; set; }

        public virtual Application  Application { get; set; }

        public int HubID { get; set; }

        public int AppID { get; set; }

        public string AssemblyPath { get; set; }

        public string BackupPath { get; set; }

        public string ConfigFilepPath { get; set; }

        public ICollection<DeploymentDetails> DeploymentDetails { get; set; } = 
            new HashSet<DeploymentDetails>();


    }
}
