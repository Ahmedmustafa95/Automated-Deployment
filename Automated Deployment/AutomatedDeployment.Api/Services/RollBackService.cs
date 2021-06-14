using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
    public class RollBackService : IRollbackService
    {
        public void Rollback(string BackupPath,string AssemblyPath)
        {

            //read all files 
            string[] allfiles = Directory.GetFiles(BackupPath);
            //put files in new path

            foreach (var file in allfiles)
            {
                string[] subs = file.Split(@"\"); 
                File.Move(file, AssemblyPath+ @"\" +subs[subs.Length - 1],true);
            }
        }
    }
}
