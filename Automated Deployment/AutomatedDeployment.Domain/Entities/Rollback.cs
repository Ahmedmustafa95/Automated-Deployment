using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
   public class Rollback
    {
        public Rollback()
        {
            Deployments = new HashSet<Deployment>();
        }
        public int Id { get; set; }
        [ForeignKey("Deployment")]
        public int DeploymentId { get; set; }
        public string BasedOn { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Deployment> Deployments { get; set; }

    }
}
