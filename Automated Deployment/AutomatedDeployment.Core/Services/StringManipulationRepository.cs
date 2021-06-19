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
        public StringManipulationRepository(EfgconfigurationdbContext _efgconfigurationdbContext,
                                             IPathRepository _pathRepository)
        {
            efgconfigurationdbContext = _efgconfigurationdbContext;
            PathRepository = _pathRepository;
        }
        private string GetAppAssemblyPath(int HubID, int AppID) =>
            PathRepository.GetAssemblyPath(HubID, AppID);
        //private string[] GetAllXmlFilesNames(int HubID, int AppID)
        //{
            


        //}
        public Dictionary<string, List<XmlConfigObj>> GetAppConfigFilesData(int HubID,int AppID)
        {
            //1- get assemply path for the appid and hub id
            //2- get all the .XML files
            //3- if exists then get all the key-value pairs in all of them and return them else return null
            // Key =file Name     value=array of object
            Dictionary<string, List<XmlConfigObj>> ConfigFilesKeysValues = new Dictionary<string, List<XmlConfigObj>>();
           
            string AppAssemblyPath = GetAppAssemblyPath(HubID, AppID);
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
