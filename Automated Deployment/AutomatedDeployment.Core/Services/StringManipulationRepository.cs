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
using System.Xml;

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
       

        //  get all applications in all hubs containing that key
        public List<ConfigSearchResult> FindConfigSetting(string ConfigName)
        {
            
            List<ConfigSearchResult> configSearches = new List<ConfigSearchResult>();

            List<HubsApplications> hubsApplications = hubsApplicationsRepository.GetAll().ToList();
            foreach (var App in hubsApplications)
            {
                string ConvertedXMLFile = this.ConvertConfigFileTostring(App.HubID, App.AppID);
                if (ConvertedXMLFile is null) continue;
                string ConfigSectionText = this.SearchForConfig(ConvertedXMLFile, ConfigName);
                if (ConfigSectionText is null) continue;
                ConfigSearchResult searchResult = new ConfigSearchResult()
                {
                    AppID = App.AppID,
                    HubID = App.HubID,
                    AppName = App.Application.AppName,
                    HubName = App.Hub.HubName,
                    ConfigurationResult= ConfigSectionText
                };
                configSearches.Add(searchResult);
            }
            if (configSearches.Count == 0) return null;
            return configSearches;
        }
        
        private string SearchForConfig(string convertedXMLFile, string configName)
        {
            if (convertedXMLFile.Contains(configName)) 
            {
                int LocatedIndex = convertedXMLFile.IndexOf(configName);
                string leftPart=convertedXMLFile.Substring(0, LocatedIndex);
                int startIndex = leftPart.LastIndexOf('<');
                int EndIndex;
                if (LocatedIndex - startIndex == 1) 
                {
                    EndIndex = convertedXMLFile.IndexOf($"</{configName}>", startIndex + configName.Length);
                    if (EndIndex == -1) return null;
                    EndIndex += configName.Length + 3;
                } 
                else
                {
                    EndIndex = convertedXMLFile.IndexOf("/>", startIndex + configName.Length);
                    if (EndIndex == -1) return null;
                    EndIndex +=2;
                }
                if (EndIndex==-1) return null;
                return convertedXMLFile.Substring(startIndex, EndIndex - startIndex);
            }
            
            //convertedXMLFile.Substring();
            return null;

        }

        public string ConvertConfigFileTostring(int HubID, int AppID)
        {
            string AppConfigFilePath = PathRepository.GetConfigFilePath(HubID,AppID);
            try
            {
                string ConvertedXMLFile = File.ReadAllText(AppConfigFilePath);
                return ConvertedXMLFile;
            }
            catch (Exception)
            {
                return null;
            } 
        }
        
        public bool UpdateSingleConfigData(ConfigSearchResult UpdatedConfig)
        {
            try
            {
                string AppConfigFilePath = PathRepository.GetConfigFilePath(HubID, AppID);
                if (AppConfigFilePath is not null)
                {
                    XDocument doc = XDocument.Load($"{AppAssemblyPath}\\{UpdatedConfig.FileName}");

                    var updatedElement = doc.Descendants(UpdatedConfig.ConfigurationSectionName).Elements()
                           .Where(x => x.Attribute(UpdatedConfig.ConfigurationSectionName == "appSettings" ? "key" : "name").Value == UpdatedConfig.ConfigurationName)
                           .FirstOrDefault();
                    if (updatedElement is not null)
                        updatedElement.SetValue(UpdatedConfig.ConfigurationValue);

                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }



        }
















        public bool UpdateKeyInMultiApplication(string ConfigName, string NewValue)
        {
            try
            {
                List<ConfigSearchResult> ConfigSearchResultList = new List<ConfigSearchResult>();
                ConfigSearchResultList = FindConfigSetting(ConfigName);
                if (ConfigSearchResultList is null) return false;
                foreach (var item in ConfigSearchResultList)
                {
                    item.ConfigurationValue = NewValue;
                    UpdateSingleConfigData(item);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        public bool UpdateAppConfigData(Dictionary<string, List<XmlConfigObj>> UpdatedXml, int HubID, int AppID)
        {
            //1-check if objects in config file exist
            //2-if exist update if not exist insert

            Dictionary<string, List<XmlConfigObj>> OldXml = GetAppConfigFilesData(HubID, AppID);

            List<XmlConfigObj> OldList = new List<XmlConfigObj>();
            List<XmlConfigObj> UpdatedList = new List<XmlConfigObj>();

            //bool XmlFileFlag = false;
            //bool SectionNameFlag = false;

            XDocument doc = new XDocument();
            if (OldXml is not null && UpdatedXml is not null)
            {
                //getting the List<xmlConfigObj> of the Old XML using the key(xml file name)
                //check if Updated file exist 
                //if exist >> get the Old list (old xml), Updated list(updated xml)
                foreach (var fileName in UpdatedXml.Keys)
                {
                    //backupFile

                    try
                    {
                        if (OldXml.ContainsKey(fileName))
                        {
                            // XmlFileFlag = true;
                            OldList = OldXml[fileName];
                            UpdatedList = UpdatedXml[fileName];
                            doc = XDocument.Load(fileName);

                            // xml file exist
                            // start comparing the updated xml file details with the old one
                            //>> Comparing OldList and UpdatedList

                            for (int i = 0; i < UpdatedList.Count; i++)
                            {
                                XmlConfigObj FoundElemnt = OldList.Find(j => j.SectionName == UpdatedList[i].SectionName &&
                                  j.ElementKey == UpdatedList[i].ElementKey);

                                //backup the whole xml file first
                                //update the old value with the new value
                                if (FoundElemnt is not null)
                                {

                                    var updatedElement = doc.Descendants(UpdatedList[i].SectionName).Elements()
                                           .Where(x => x.Attribute(UpdatedList[i].SectionName == "appSettings" ? "key" : "name").Value == UpdatedList[i].ElementKey)
                                           .FirstOrDefault();
                                    if (updatedElement is not null)
                                        updatedElement.SetValue(UpdatedList[i].ElementValue);

                                }
                                else
                                {
                                    //insert
                                    XElement newElement;
                                    if (UpdatedList[i].SectionName == "appSettings")
                                    {
                                        newElement = new XElement("add", new XAttribute("key", UpdatedList[i].ElementKey),
                                            new XAttribute("value", UpdatedList[i].ElementValue));

                                    }
                                    else
                                    {
                                        newElement = new XElement("add", new XAttribute("name", UpdatedList[i].ElementKey),
                                           new XAttribute("connectionString", UpdatedList[i].ElementValue));
                                    }
                                    doc.Descendants(UpdatedList[i].SectionName).Elements().Append(newElement);

                                }


                            }

                            doc.Save(fileName);

                        }
                    }
                    catch (Exception)
                    {

                        return false;
                    }
                }
                return true;
            }
            else
            {
                //Assembly path doesn't exist 
                return false;
            }
        }

        public Dictionary<string, List<XmlConfigObj>> GetAppConfigFilesData(int HubID, int AppID)
        {
            //[{"1.xml":[{sectioname="AppSetting",Key=LastName,Value="ahmed"}]}]
            //1- get assemply path for the appid and hub id
            //2- get all the .XML files
            //3- if exists then get all the key-value pairs in all of them and return them else return null
            // Key = fileName, value=array of XmlConfigObj object
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


public class Old_StringManipulationRepository
{
    private readonly EfgconfigurationdbContext efgconfigurationdbContext;
    private readonly IPathRepository PathRepository;
    private readonly IHubsApplicationsRepository hubsApplicationsRepository;
    public Old_StringManipulationRepository(EfgconfigurationdbContext _efgconfigurationdbContext,
                                         IPathRepository _pathRepository,
                                         IHubsApplicationsRepository _hubsApplicationsRepository)
    {
        efgconfigurationdbContext = _efgconfigurationdbContext;
        PathRepository = _pathRepository;
        hubsApplicationsRepository = _hubsApplicationsRepository;
    }
    private string GetAppAssemblyPath(int HubID, int AppID) =>
        PathRepository.GetAssemblyPath(HubID, AppID);


    //  get all applications in all hubs containing that key
    public List<ConfigSearchResult> FindConfigSetting(string ConfigName)
    {
        //File.ReadAllText()
        List<ConfigSearchResult> configSearches = new List<ConfigSearchResult>();

        List<HubsApplications> hubsApplications = hubsApplicationsRepository.GetAll().ToList();
        //looping over all the apps in the hubsApplications List
        foreach (var App in hubsApplications)
        {
            Dictionary<string, List<XmlConfigObj>> AppConfigData = this.GetAppConfigFilesData(App.HubID, App.AppID);
            //looping in every list from AppConfigData
            foreach (var KeyValue in AppConfigData)
            {
                //foundItems is the ones that match the condition
                List<XmlConfigObj> foundItems = KeyValue.Value.FindAll(i => i.ElementKey.ToLower().Contains(ConfigName.ToLower()) || i.SectionName.ToLower().Contains(ConfigName.ToLower()));
                //looping ovet foundItems to fill the array with the found result


                for (int i = 0; i < foundItems.Count; i++)
                {
                    ConfigSearchResult searchResult = new ConfigSearchResult()
                    {
                        AppID = App.AppID,
                        HubID = App.HubID,
                        AppName = App.Application.AppName,
                        HubName = App.Hub.HubName,
                        ConfigurationSectionName = foundItems[i].SectionName,
                        ConfigurationName = foundItems[i].ElementKey,
                        ConfigurationValue = foundItems[i].ElementValue,
                        FileName = KeyValue.Key
                    };
                    configSearches.Add(searchResult);
                }
            }
        }
        return configSearches;
    }

    // Get All Keys_Values Pair For application in specified Hub
    public Dictionary<string, List<XmlConfigObj>> GetAppConfigFilesData(int HubID, int AppID)
    {
        //[{"1.xml":[{sectioname="AppSetting",Key=LastName,Value="ahmed"}]}]
        //1- get assemply path for the appid and hub id
        //2- get all the .XML files
        //3- if exists then get all the key-value pairs in all of them and return them else return null
        // Key = fileName, value=array of XmlConfigObj object
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

    public bool UpdateAppConfigData(Dictionary<string, List<XmlConfigObj>> UpdatedXml, int HubID, int AppID)
    {
        //1-check if objects in config file exist
        //2-if exist update if not exist insert

        Dictionary<string, List<XmlConfigObj>> OldXml = GetAppConfigFilesData(HubID, AppID);

        List<XmlConfigObj> OldList = new List<XmlConfigObj>();
        List<XmlConfigObj> UpdatedList = new List<XmlConfigObj>();

        //bool XmlFileFlag = false;
        //bool SectionNameFlag = false;

        XDocument doc = new XDocument();
        if (OldXml is not null && UpdatedXml is not null)
        {
            //getting the List<xmlConfigObj> of the Old XML using the key(xml file name)
            //check if Updated file exist 
            //if exist >> get the Old list (old xml), Updated list(updated xml)
            foreach (var fileName in UpdatedXml.Keys)
            {
                //backupFile

                try
                {
                    if (OldXml.ContainsKey(fileName))
                    {
                        // XmlFileFlag = true;
                        OldList = OldXml[fileName];
                        UpdatedList = UpdatedXml[fileName];
                        doc = XDocument.Load(fileName);

                        // xml file exist
                        // start comparing the updated xml file details with the old one
                        //>> Comparing OldList and UpdatedList

                        for (int i = 0; i < UpdatedList.Count; i++)
                        {
                            XmlConfigObj FoundElemnt = OldList.Find(j => j.SectionName == UpdatedList[i].SectionName &&
                              j.ElementKey == UpdatedList[i].ElementKey);

                            //backup the whole xml file first
                            //update the old value with the new value
                            if (FoundElemnt is not null)
                            {

                                var updatedElement = doc.Descendants(UpdatedList[i].SectionName).Elements()
                                       .Where(x => x.Attribute(UpdatedList[i].SectionName == "appSettings" ? "key" : "name").Value == UpdatedList[i].ElementKey)
                                       .FirstOrDefault();
                                if (updatedElement is not null)
                                    updatedElement.SetValue(UpdatedList[i].ElementValue);

                            }
                            else
                            {
                                //insert
                                XElement newElement;
                                if (UpdatedList[i].SectionName == "appSettings")
                                {
                                    newElement = new XElement("add", new XAttribute("key", UpdatedList[i].ElementKey),
                                        new XAttribute("value", UpdatedList[i].ElementValue));

                                }
                                else
                                {
                                    newElement = new XElement("add", new XAttribute("name", UpdatedList[i].ElementKey),
                                       new XAttribute("connectionString", UpdatedList[i].ElementValue));
                                }
                                doc.Descendants(UpdatedList[i].SectionName).Elements().Append(newElement);

                            }


                        }

                        doc.Save(fileName);

                    }
                }
                catch (Exception)
                {

                    return false;
                }
            }
            return true;
        }
        else
        {
            //Assembly path doesn't exist 
            return false;
        }
    }

    //update only one key in single xml file
    public bool UpdateSingleConfigData(ConfigSearchResult UpdatedConfig)
    {
        try
        {
            string AppAssemblyPath = GetAppAssemblyPath(UpdatedConfig.HubID, UpdatedConfig.AppID);
            if (AppAssemblyPath is not null)
            {
                XDocument doc = XDocument.Load($"{AppAssemblyPath}\\{UpdatedConfig.FileName}");

                var updatedElement = doc.Descendants(UpdatedConfig.ConfigurationSectionName).Elements()
                       .Where(x => x.Attribute(UpdatedConfig.ConfigurationSectionName == "appSettings" ? "key" : "name").Value == UpdatedConfig.ConfigurationName)
                       .FirstOrDefault();
                if (updatedElement is not null)
                    updatedElement.SetValue(UpdatedConfig.ConfigurationValue);

            }
            return true;
        }
        catch (Exception)
        {

            return false;
        }



    }


    public bool UpdateKeyInMultiApplication(string ConfigName, string NewValue)
    {
        try
        {
            List<ConfigSearchResult> ConfigSearchResultList = new List<ConfigSearchResult>();
            ConfigSearchResultList = FindConfigSetting(ConfigName);
            if (ConfigSearchResultList is null) return false;
            foreach (var item in ConfigSearchResultList)
            {
                item.ConfigurationValue = NewValue;
                UpdateSingleConfigData(item);
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }

}












//using AutomatedDeployment.Core.Interfaces;
//using AutomatedDeployment.Domain.Entities;
//using AutomatedDeployment.Infrastructure.Context;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Xml.Linq;
//using System.Xml;

//namespace AutomatedDeployment.Core.Services
//{
//    public class StringManipulationRepository : IStringManipulationRepository
//    {
//        private readonly EfgconfigurationdbContext efgconfigurationdbContext;
//        private readonly IPathRepository PathRepository;
//        private readonly IHubsApplicationsRepository hubsApplicationsRepository;
//        public StringManipulationRepository(EfgconfigurationdbContext _efgconfigurationdbContext,
//                                             IPathRepository _pathRepository,
//                                             IHubsApplicationsRepository _hubsApplicationsRepository)
//        {
//            efgconfigurationdbContext = _efgconfigurationdbContext;
//            PathRepository = _pathRepository;
//            hubsApplicationsRepository = _hubsApplicationsRepository;
//        }
//        private string GetAppAssemblyPath(int HubID, int AppID) =>
//            PathRepository.GetAssemblyPath(HubID, AppID);


//        //  get all applications in all hubs containing that key
//        public List<ConfigSearchResult> FindConfigSetting(string ConfigName)
//        {
//            List<ConfigSearchResult> configSearches = new List<ConfigSearchResult>();
//            // Add Same Paths In DB
//            List<HubsApplications> hubsApplications = hubsApplicationsRepository.GetAll().Where(h => h.AppID == 4 || h.AppID == 6 || h.AppID == 1 || h.AppID == 2).ToList();
//            //looping over all the apps in the hubsApplications List
//            if (hubsApplications is null) return null;
//            foreach (var App in hubsApplications)
//            {
//                Dictionary<string, List<XmlConfigObj>> AppConfigData = this.GetAppConfigFilesData(App.HubID, App.AppID);
//                //looping in every list from AppConfigData
//                foreach (var KeyValue in AppConfigData)
//                {
//                    //foundItems is the ones that match the condition
//                    List<XmlConfigObj> foundItems = KeyValue.Value.FindAll(i => i.ElementKey.ToLower().Contains(ConfigName.ToLower()) 
//                                                                            || i.SectionName.ToLower().Contains(ConfigName.ToLower())
//                                                                            );
//                    //looping ovet foundItems to fill the array with the found result

//                    for (int i = 0; i < (foundItems?.Count??0); i++)
//                    {
//                        ConfigSearchResult searchResult = new ConfigSearchResult()
//                        {
//                            AppID = App.AppID,
//                            HubID = App.HubID,
//                            AppName = App.Application.AppName,
//                            HubName = App.Hub.HubName,
//                            ConfigurationSectionName = foundItems[i].SectionName,
//                            ConfigurationName = foundItems[i].ElementKey,
//                            ConfigurationValue = foundItems[i].ElementValue,
//                            FileName = KeyValue.Key
//                        };
//                        configSearches.Add(searchResult);
//                    }
//                }
//            }
//            if (configSearches.Count == 0) return null;
//            return configSearches;
//        }

//        // Get All Keys_Values Pair For application in specified Hub
//        public Dictionary<string, List<XmlConfigObj>> GetAppConfigFilesData(int HubID, int AppID)
//        {
//            //[{"1.xml":[{sectioname="AppSetting",Key=LastName,Value="ahmed"}]}]
//            //1- get assemply path for the appid and hub id
//            //2- get all the .XML files
//            //3- if exists then get all the key-value pairs in all of them and return them else return null
//            // Key = fileName, value=array of XmlConfigObj object
//            Dictionary<string, List<XmlConfigObj>> ConfigFilesKeysValues = new Dictionary<string, List<XmlConfigObj>>();

//            string AppAssemblyPath = GetAppAssemblyPath(HubID, AppID);
//            if (AppAssemblyPath is null) return new Dictionary<string, List<XmlConfigObj>>();
//            string[] XMLFilesNames = Directory.GetFiles(AppAssemblyPath, "*.xml", SearchOption.TopDirectoryOnly);

//            foreach (string fileName in XMLFilesNames)
//            {
//                XDocument doc = XDocument.Load(fileName);
//                List<XElement> XmlAppSettings = doc?.Descendants("appSettings")?.ToList()?.Elements()?.ToList() ?? new List<XElement>();
//                List<XElement> XmlConnectionStrings = doc?.Descendants("connectionStrings")?.ToList().Elements()?.ToList() ?? new List<XElement>();
//                List<XmlConfigObj> ConfigFileElements = new List<XmlConfigObj>();

//                for (int i = 0; i < XmlAppSettings.Count; i++)
//                {
//                    var Key = XmlAppSettings[i]?.Attribute("key").Value;
//                    var Value = XmlAppSettings[i]?.Attribute("value").Value;
//                    XmlConfigObj xmlConfigObj = new XmlConfigObj()
//                    { SectionName = "appSettings", ElementKey = Key, ElementValue = Value };
//                    ConfigFileElements.Add(xmlConfigObj);
//                }
//                for (int i = 0; i < XmlConnectionStrings.Count; i++)
//                {
//                    var Name = XmlConnectionStrings[i]?.Attribute("name").Value;
//                    var ConnectionString = XmlConnectionStrings[i]?.Attribute("connectionString").Value;
//                    XmlConfigObj xmlConfigObj = new XmlConfigObj()
//                    { SectionName = "connectionStrings", ElementKey = Name, ElementValue = ConnectionString };
//                    ConfigFileElements.Add(xmlConfigObj);
//                }
//                ConfigFilesKeysValues.Add(fileName, ConfigFileElements);
//            }

//            return ConfigFilesKeysValues;
//        }

//        public bool UpdateAppConfigData(Dictionary<string, List<XmlConfigObj>> UpdatedXml, int HubID, int AppID)
//        {
//            //1-check if objects in config file exist
//            //2-if exist update if not exist insert

//            Dictionary<string, List<XmlConfigObj>> OldXml = GetAppConfigFilesData(HubID, AppID);

//            List<XmlConfigObj> OldList = new List<XmlConfigObj>();
//            List<XmlConfigObj> UpdatedList = new List<XmlConfigObj>();

//            //bool XmlFileFlag = false;
//            //bool SectionNameFlag = false;

//            XDocument doc = new XDocument();
//            if (OldXml is not null && UpdatedXml is not null)
//            {
//                //getting the List<xmlConfigObj> of the Old XML using the key(xml file name)
//                //check if Updated file exist 
//                //if exist >> get the Old list (old xml), Updated list(updated xml)
//                foreach (var fileName in UpdatedXml.Keys)
//                {
//                    //backupFile

//                    try
//                    {
//                        if (OldXml.ContainsKey(fileName))
//                        {
//                            // XmlFileFlag = true;
//                            OldList = OldXml[fileName];
//                            UpdatedList = UpdatedXml[fileName];
//                            doc = XDocument.Load(fileName);

//                            // xml file exist
//                            // start comparing the updated xml file details with the old one
//                            //>> Comparing OldList and UpdatedList

//                            for (int i = 0; i < UpdatedList.Count; i++)
//                            {
//                                XmlConfigObj FoundElemnt = OldList.Find(j => j.SectionName == UpdatedList[i].SectionName &&
//                                  j.ElementKey == UpdatedList[i].ElementKey);

//                                //backup the whole xml file first
//                                //update the old value with the new value
//                                if (FoundElemnt is not null)
//                                {

//                                    var updatedElement = doc.Descendants(UpdatedList[i].SectionName).Elements()
//                                           .Where(x => x.Attribute(UpdatedList[i].SectionName == "appSettings" ? "key" : "name").Value == UpdatedList[i].ElementKey)
//                                           .FirstOrDefault();
//                                    if (updatedElement is not null)
//                                        updatedElement.SetValue(UpdatedList[i].ElementValue);

//                                }
//                                else
//                                {
//                                    //insert
//                                    XElement newElement;
//                                    if (UpdatedList[i].SectionName == "appSettings")
//                                    {
//                                        newElement = new XElement("add", new XAttribute("key", UpdatedList[i].ElementKey),
//                                            new XAttribute("value", UpdatedList[i].ElementValue));

//                                    }
//                                    else
//                                    {
//                                        newElement = new XElement("add", new XAttribute("name", UpdatedList[i].ElementKey),
//                                           new XAttribute("connectionString", UpdatedList[i].ElementValue));
//                                    }
//                                    doc.Descendants(UpdatedList[i].SectionName).Elements().Append(newElement);

//                                }


//                            }

//                            doc.Save(fileName);

//                        }
//                    }
//                    catch (Exception)
//                    {

//                        return false;
//                    }
//                }
//                return true;
//            }
//            else
//            {
//                //Assembly path doesn't exist 
//                return false;
//            }
//        }

//        //update only one key in single xml file
//        public bool UpdateSingleConfigData(ConfigSearchResult UpdatedConfig)
//        {
//            try
//            {
//                string AppAssemblyPath = GetAppAssemblyPath(UpdatedConfig.HubID, UpdatedConfig.AppID);

//                if (AppAssemblyPath is not null)
//                {
//                    XDocument doc = XDocument.Load($"{UpdatedConfig.FileName}");
//                    //XDocument doc = XDocument.Load($"{AppAssemblyPath}\\{UpdatedConfig.FileName}");

//                    var updatedElement = doc.Descendants(UpdatedConfig.ConfigurationSectionName).Elements()
//                           .Where(x => x.Attribute(UpdatedConfig.ConfigurationSectionName == "appSettings" ? "key" : "name").Value == UpdatedConfig.ConfigurationName)
//                           .FirstOrDefault();
//                    if (updatedElement is not null)
//                        updatedElement.Attribute("value").Value = (UpdatedConfig.ConfigurationValue);
//                    //doc.Save($"{AppAssemblyPath}\\{UpdatedConfig.FileName}");
//                    doc.Save($"{UpdatedConfig.FileName}");

//                }
//                return true;
//            }
//            catch (Exception ex)
//            {

//                return false;
//            }



//        }

//        public bool UpdateKeyInMultiApplication(string ConfigName, string NewValue)
//        {
//            try
//            {
//                List<ConfigSearchResult> ConfigSearchResultList = new List<ConfigSearchResult>();
//                ConfigSearchResultList = FindConfigSetting(ConfigName);
//                if (ConfigSearchResultList is null) return false;
//                foreach (var item in ConfigSearchResultList)
//                {
//                    item.ConfigurationValue = NewValue;
//                    UpdateSingleConfigData(item);
//                }
//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }

//        }

//    }






//    //key >> Clinet Validation
//    //app1 - true
//    //app2 - false

//    //Key >> WebPages
//    //app1 - index.html
//    //app2 - app.html


//    /*
//     public List<ConfigSearchResult> FindConfigSetting(string ConfigName)
//     {
//         List<ConfigSearchResult> configSearches = new List<ConfigSearchResult>();

//         List<HubsApplications> hubsApplications = hubsApplicationsRepository.GetAll().ToList();
//         //looping over all the apps in the hubsApplications List
//         foreach (var App in hubsApplications)
//         {
//             Dictionary<string, List<XmlConfigObj>> AppConfigData = this.GetAppConfigFilesData(App.HubID, App.AppID);
//             //looping in every list from AppConfigData
//             foreach (var KeyValue in AppConfigData)
//             {
//                 //foundItems is the ones that match the condition
//                 List<XmlConfigObj> foundItems= KeyValue.Value.FindAll(i =>i.ElementKey.ToLower().Contains(ConfigName.ToLower())|| i.SectionName.ToLower().Contains(ConfigName.ToLower()));
//                 //looping ovet foundItems to fill the array with the found result
//                 for (int i = 0; i < foundItems.Count; i++)
//                 {
//                     ConfigSearchResult searchResult = new ConfigSearchResult() 
//                     {
//                         AppID=App.AppID, HubID=App.HubID,
//                         ConfigurationSectionName= foundItems[i].SectionName,
//                         ConfigurationName= foundItems[i].ElementKey,
//                         FileName=KeyValue.Key,
//                         ConfigurationValue=foundItems[i].ElementValue
//                     };
//                     configSearches.Add(searchResult);
//                 }
//             }
//         }
//         return configSearches;
//     }

//     public Dictionary<string, List<XmlConfigObj>> GetAppConfigFilesData(int HubID,int AppID)
//     {
//         //1- get assemply path for the appid and hub id
//         //2- get all the .XML files
//         //3- if exists then get all the key-value pairs in all of them and return them else return null
//         // Key =file Name, value=array of object
//         Dictionary<string, List<XmlConfigObj>> ConfigFilesKeysValues = new Dictionary<string, List<XmlConfigObj>>();

//         string AppAssemblyPath = GetAppAssemblyPath(HubID, AppID);
//         if (AppAssemblyPath is null) return new Dictionary<string, List<XmlConfigObj>>();
//         string[] XMLFilesNames = Directory.GetFiles(AppAssemblyPath, "*.xml", SearchOption.TopDirectoryOnly);

//         foreach (string fileName in XMLFilesNames)
//         {
//             XDocument doc = XDocument.Load(fileName);
//             List<XElement> XmlAppSettings = doc?.Descendants("appSettings")?.ToList()?.Elements()?.ToList() ?? new List<XElement>();
//             List<XElement> XmlConnectionStrings = doc?.Descendants("connectionStrings")?.ToList().Elements()?.ToList() ?? new List<XElement>();
//             List<XmlConfigObj> ConfigFileElements = new List<XmlConfigObj>();

//             for (int i = 0; i < XmlAppSettings.Count; i++)
//             {
//                 var Key = XmlAppSettings[i]?.Attribute("key").Value;
//                 var Value = XmlAppSettings[i]?.Attribute("value").Value;
//                 XmlConfigObj xmlConfigObj = new XmlConfigObj() 
//                 { SectionName = "appSettings", ElementKey = Key, ElementValue = Value };
//                 ConfigFileElements.Add(xmlConfigObj);
//              }


//             for (int i = 0; i < XmlConnectionStrings.Count; i++)
//             {
//                 var Name = XmlConnectionStrings[i]?.Attribute("name").Value;
//                 var ConnectionString = XmlConnectionStrings[i]?.Attribute("connectionString").Value;
//                 XmlConfigObj xmlConfigObj = new XmlConfigObj()
//                 { SectionName = "connectionStrings", ElementKey = Name, ElementValue = ConnectionString };

//                 ConfigFileElements.Add(xmlConfigObj);
//             }
//             ConfigFilesKeysValues.Add(fileName, ConfigFileElements);
//         }

//         return ConfigFilesKeysValues;
//     }
//     */


//}
