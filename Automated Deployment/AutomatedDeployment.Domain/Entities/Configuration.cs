using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomatedDeployment.Domain.Entities
{
    public class Configuration
    {
        [Key]
        public int HubID { get; set; }
        [Key]
        public int AppID { get; set; }
        public string AssemblyPath { get; set; }
        public string ConfigurationPath { get; set; }
        [StringLength(50)]
        public string DeployedBy { get; set; }

        [StringLength(50)]
        public string RequestedBy { get; set; }
        [StringLength(50)]
        public string ApprovedBy { get; set; }

        public string BackupPath { get; set; }

        [ForeignKey(nameof(AppID))]
    //    [InverseProperty(nameof(Application.Configurations))]
        public virtual Application App { get; set; }
        [ForeignKey(nameof(HubID))]
        [InverseProperty("Configurations")]
        public virtual Hub Hub { get; set; }
    }
}
