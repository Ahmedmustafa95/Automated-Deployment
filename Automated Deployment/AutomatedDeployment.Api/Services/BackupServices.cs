using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
    public class BackupServices:IBackupServices
    {
       
        public void MoveTOBackUpFolder(List<string> filesName, string assemblyPath,string backupPath)
        {
            //DirectoryInfo di = new DirectoryInfo(backupPath);

            //foreach (FileInfo file in di.GetFiles())
            //{
            //    file.Delete();
            //}
            //if (!Directory.Exists(backupPath))
            //{
            //    Directory.CreateDirectory(backupPath);
            //}
            foreach (var item in filesName)
            {
                if(File.Exists(assemblyPath +'\\'+ item))
                    File.Move($"{assemblyPath}{@"\"}{item}".Trim(),$"{backupPath}{@"\"}{item}".Trim(),true);
                    //File.Move(assemblyPath + '\\' + item, backupPath + '\\' + item);

            }
           
        }
        public void MoveTOBackUpFolder(string fileName,string backupPath)
        {
            DateTime currentDate = DateTime.Now;

            if (File.Exists(fileName))
            {
                string NewBackupPath = $"{backupPath}\\BK_{currentDate.ToString("yyyy-MM-dd-hh-mm-ss")}".Trim();
                Directory.CreateDirectory(NewBackupPath);
                int lastSlashIndex=fileName.LastIndexOf('\\');
                File.Copy($"{fileName}".Trim(), $"{NewBackupPath}{@"\"}{fileName.Substring(lastSlashIndex)}".Trim());

            }

           
        }

    }
}
