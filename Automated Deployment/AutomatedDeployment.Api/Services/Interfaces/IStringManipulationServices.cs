using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
    public interface IStringManipulationServices
    {
        List<ConfigSearchResult> FindConfigSetting(string ConfigName);
        Dictionary<string, List<XmlConfigObj>> GetAppConfigFilesData(int HubID, int AppID);

        bool UpdateAppConfigData(Dictionary<string, List<XmlConfigObj>> UpdatedXml, int HubID, int AppID);

        bool UpdateSingleConfigData(ConfigSearchResult UpdatedConfig);

        bool UpdateKeyInMultiApplication(string ConfigName, string NewValue);

    }
}
