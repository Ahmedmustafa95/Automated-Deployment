using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
   public class LastDeploymentviewmodel
    {
        public int hubId { get; set; }
        public int appId { get; set; }
        public string ApplicationName { get; set; }
        public string HubName { get; set; }
    }
}
