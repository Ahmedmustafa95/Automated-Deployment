using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutomatedDeployment.Core.Services
{
    public class StringManipulationRepository
    {
        private readonly EfgconfigurationdbContext efgconfigurationdbContext;
        private readonly IPathRepository PathRepository;
        private readonly IHubsApplicationsRepository hubsApplicationsRepository;
        public StringManipulationRepository(EfgconfigurationdbContext _efgconfigurationdbContext,
                                             IPathRepository _pathRepository,
                                             IHubsApplicationsRepository _hubsApplicationsRepository)
        {
            efgconfigurationdbContext = _efgconfigurationdbContext;
            PathRepository = _pathRepository;
            hubsApplicationsRepository = _hubsApplicationsRepository;
        }
        private string GetAppAssemblyPath(int HubID, int AppID) =>
            PathRepository.GetAssemblyPath(HubID, AppID);
        public List<ConfigSearchResult> FindConfigSetting(string ConfigName)
        {
            List<ConfigSearchResult> configSearches = new List<ConfigSearchResult>();

            List<HubsApplications> hubsApplications = hubsApplicationsRepository.GetAll().ToList();
            //looping over all the apps in the hubsApplications List
            foreach (var App in hubsApplications)
            {
                Dictionary<string, List<XmlConfigObj>> AppConfigData = this.GetAppConfigFilesData(App.HubID, App.AppID);
                //looping in every list from AppConfigData
                foreach (var KeyValue in AppConfigData.Values)
                {
                    //foundItems is the ones that match the condition
                    List<XmlConfigObj> foundItems= KeyValue.FindAll(i =>i.ElementKey.ToLower().Contains(ConfigName.ToLower())|| i.SectionName.ToLower().Contains(ConfigName.ToLower()));
                    //looping ovet foundItems to fill the array with the found result
                    for (int i = 0; i < foundItems.Count; i++)
                    {
                        ConfigSearchResult searchResult = new ConfigSearchResult() 
                        {
                            AppID=App.AppID, HubID=App.HubID,
                            ConfigurationSectionName= foundItems[i].SectionName,
                            ConfigurationName= foundItems[i].ElementKey,
                            ConfigurationValue=foundItems[i].ElementValue
                        };
                        configSearches.Add(searchResult);
                    }
                }
            }
            return configSearches;
        }

        public Dictionary<string, List<XmlConfigObj>> GetAppConfigFilesData(int HubID,int AppID)
        {
            //1- get assemply path for the appid and hub id
            //2- get all the .XML files
            //3- if exists then get all the key-value pairs in all of them and return them else return null
            // Key =file Name, value=array of object
            Dictionary<string, List<XmlConfigObj>> ConfigFilesKeysValues = new Dictionary<string, List<XmlConfigObj>>();
           
            string AppAssemblyPath = GetAppAssemblyPath(HubID, AppID);
            if (AppAssemblyPath is null) return new Dictionary<string, List<XmlConfigObj>>();
            string[] XMLFilesNames = Directory.GetFiles(AppAssemblyPath, "*.xml", SearchOption.TopDirectoryOnly);
        
            foreach (string fileName in XMLFilesNames)
            {
                XDocument doc = XDocument.Load(fileName);
                List<XElement> XmlAppSettings = doc?.Descendants("appSettings")?.ToList()?.Elements()?.ToList() ?? new List<XElement>();
                List<XElement> XmlConnectionStrings = doc?.Descendants("connectionStrings")?.ToList().Elements()?.ToList() ?? new List<XElement>();
                List<XmlConfigObj> ConfigFileElements = new List<XmlConfigObj>();
        
                for (int i = 0; i < XmlAppSettings.Count; i++)
                {
                    var Key = XmlAppSettings[i]?.Attribute("key").Value;
                    var Value = XmlAppSettings[i]?.Attribute("value").Value;
                    XmlConfigObj xmlConfigObj = new XmlConfigObj() 
                    { SectionName = "appSettings", ElementKey = Key, ElementValue = Value };
                    ConfigFileElements.Add(xmlConfigObj);
                 }


                for (int i = 0; i < XmlConnectionStrings.Count; i++)
                {
                    var Name = XmlConnectionStrings[i]?.Attribute("name").Value;
                    var ConnectionString = XmlConnectionStrings[i]?.Attribute("connectionString").Value;
                    XmlConfigObj xmlConfigObj = new XmlConfigObj()
                    { SectionName = "connectionStrings", ElementKey = Name, ElementValue = ConnectionString };

                    ConfigFileElements.Add(xmlConfigObj);
                }
                ConfigFilesKeysValues.Add(fileName, ConfigFileElements);
            }
        
            return ConfigFilesKeysValues;
        }
    }
}
