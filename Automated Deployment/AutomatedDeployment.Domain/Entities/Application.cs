using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
    public class Application
    {
        public Application()
        {
            HubsApplications = new HashSet<HubsApplications>();
            Deployments = new HashSet<Deployment>();
        }

        [Key]
        public int AppID { get; set; }
        [Required]
        [StringLength(50)]
        public string AppName { get; set; }

     
        public virtual ICollection<HubsApplications> HubsApplications { get; set; }

        public virtual ICollection<Deployment> Deployments { get; set; }
    }
}
