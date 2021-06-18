using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
    public class Deployment
    {
        public int DeploymentID { get; set; }
        [ForeignKey("Hub")]
        public int HubID { get; set; }
        [ForeignKey("Application")]
        public int AppID { get; set; }
        public virtual Hub Hub { get; set; }

        public virtual Application Application { get; set; }

        public DateTime DeploymentDate { get; set; }

        [Required(ErrorMessage = "You must enter your name")]
        public string RequestedBy { get; set; }
        public string ApprovedBy { get; set; }
      
       
        public virtual ICollection<DeploymentFiles> DeploymentFiles { get; set; } = new HashSet<DeploymentFiles>();

    }
}
