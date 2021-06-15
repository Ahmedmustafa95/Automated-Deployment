﻿using Microsoft.AspNetCore.Http;
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
            DirectoryInfo di = new DirectoryInfo(backupPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }
            foreach (var item in filesName)
            {
                File.Move(assemblyPath+'\\'+item, backupPath + '\\' + item);
            }
           
        }
    }
}