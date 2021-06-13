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
            Configurations = new HashSet<Configuration>();
        }

        [Key]
        public int AppID { get; set; }
        [Required]
        [StringLength(50)]
        public string AppName { get; set; }

        [InverseProperty(nameof(Configuration.App))]
        public virtual ICollection<Configuration> Configurations { get; set; }
    }
}
