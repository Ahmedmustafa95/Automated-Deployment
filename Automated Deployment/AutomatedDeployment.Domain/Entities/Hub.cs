using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
