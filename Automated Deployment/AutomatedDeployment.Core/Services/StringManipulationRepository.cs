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
        public Dictionary<string,XmlConfigObj[]> GetAllConfigFiles(int HubID,int AppID)
        {
            //1- get assemply path for the appid and hub id
            //2- get all the .XML files
            //3- if exists then get all the key-value pairs in all of them and return them else return null
            string AppAssemblyPath = GetAppAssemblyPath(HubID, AppID);
            string[] XMLFilesNames = Directory.GetFiles(AppAssemblyPath, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (string fileName in XMLFilesNames)
            {
                XDocument doc = XDocument.Load(fileName);
                var result = doc.Descendants("YearsPlayed").Any(yearsplayed => Convert.ToInt32(yearsplayed.Value) > 15);
                // Copy file.
            }
            return new Dictionary<string, XmlConfigObj[]>();
        }
    }
}
