using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutomatedDeployment.Domain.Entities
{
    public class Application
    {
        public Application()
        {
            HubsApplications = new HashSet<HubsApplications>();
            RequestDeploymentHubsApplications = new HashSet<RequestDeploymentHubsApplications>();
        }

        [Key]
        public int AppID { get; set; }
        [Required]
        [StringLength(50)]
        public string AppName { get; set; }
        public virtual ICollection<HubsApplications> HubsApplications { get; set; }
        public virtual ICollection<RequestDeploymentHubsApplications> RequestDeploymentHubsApplications { get; set; }

    }
}
