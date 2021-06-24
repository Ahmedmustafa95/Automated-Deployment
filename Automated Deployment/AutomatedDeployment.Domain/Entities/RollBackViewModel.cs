using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
    public class RollBackViewModel
    {
        public int hubId { get; set; }
        public int appID { get; set; }
        public string deployedBy { get; set; }
        public string requestedBy { get; set; }
        public string approvedBy { get; set; }
    }
}
