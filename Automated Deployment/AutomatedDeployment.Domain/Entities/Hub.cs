using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
    public class Hub
    {
        public Hub()
        {
            HubsApplications = new HashSet<HubsApplications>();
        }

        [Key]
        public int HubID { get; set; }
        [Required]
        [StringLength(50)]
        public string HubName { get; set; }

        public virtual ICollection<HubsApplications> HubsApplications { get; set; }
    }
}
