using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
    public class RollBackService : IRollbackService
    {
        public void Rollback(string BackupPath,string AssemblyPath,Dictionary<string,status> deploymentFiles)
        {

            //read all files 
            string[] allfiles = Directory.GetFiles(BackupPath);
            string[] subs;
            //put files in new path

            foreach (var file in allfiles)
            {
                subs = file.Split(@"\"); 
                if (deploymentFiles.TryGetValue(subs[subs.Length - 1], out status statues))
                {
                    if (statues == status.Added)
                        File.Delete(AssemblyPath + @"\" + subs[subs.Length - 1]);
                    else if (statues == status.Modified)
                        File.Move(file, AssemblyPath + @"\" + subs[subs.Length - 1], true);
                }



            }
            
        }

        
    }
}
