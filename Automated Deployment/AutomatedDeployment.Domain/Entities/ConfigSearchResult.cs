using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
    public class ConfigSearchResult
    {
        public int HubID { get; set; }
        public int AppID { get; set; }
        public string ConfigurationSectionName { get; set; }
        public string ConfigurationName { get; set; }
        public string ConfigurationValue { get; set; }

        public string FileName { get; set; }
    }
}
