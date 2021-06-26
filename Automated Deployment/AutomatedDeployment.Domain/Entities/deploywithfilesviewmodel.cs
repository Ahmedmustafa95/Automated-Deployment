using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
 public   class deploywithfilesviewmodel
    {
        public string filesname { get; set; }
        public status status { get; set; }
        public string hubname { get; set; }
        public string appname { get; set; }
    }
}
