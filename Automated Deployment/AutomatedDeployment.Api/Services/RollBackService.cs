using AutomatedDeployment.Core.Interfaces;
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

        
        private readonly IUnitOfWork unitOfWork;

        public RollBackService( IUnitOfWork _unitOfWork)
        {
          
            unitOfWork = _unitOfWork;
        }
        public void Rollback(int hubid, int applicationid,string BackupPath,string AssemblyPath,Dictionary<string,status> deploymentFiles)
        {
            // string BackUpPath = pathRepository.GetBackupPath(hubid, applicationid);

            //if (!Directory.Exists(BackUpPath))
            //    Directory.CreateDirectory(BackUpPath);
            var currentDate = DateTime.Now;
            string NewBackupPath = $"{BackupPath} \\ BK_{currentDate.ToString("yyyy-MM-dd-hh-mm-ss")}";
            Directory.CreateDirectory(NewBackupPath);

            // Dictionary has Files Name as key and Files state as value

            //var filesName = files.Select(f => f.FileName).ToList();
            //ibackupService.MoveTOBackUpFolder(filesName, AssemblyPath, BackUpPath);
            //List<string> files = new List<string>();
            

           
         
            Deployment deployment = new Deployment()
            {
                HubID = hubid,
                AppID = applicationid,
                DeploymentDate = currentDate,
                ApprovedBy = "Shawky",
                RequestedBy = "Mustafa",
            };
            int addedID = unitOfWork.DeploymentRepository.AddDeployment(deployment).DeploymentID; 
           

            //read all files 
            //string[] allfiles = Directory.GetFiles(BackupPath);
            //string[] subs;
            //put files in new path

            foreach (var file in deploymentFiles)
            {
                if(file.Value == status.Modified)
                {
                    //files.Add(file.Key);
                    DeploymentFiles deploymentfile = new DeploymentFiles()
                    {
                        DeploymentID = addedID,
                        FilesName=file.Key,
                        Status = status.Modified
                    };
                    unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
                    File.Move(AssemblyPath+@"\"+file.Key, NewBackupPath + @"\" + file.Key);
                    File.Copy(BackupPath + @"\" + file.Key, AssemblyPath + @"\" + file.Key);

                }
                else if (file.Value==status.Added)
                {
                   // files.Add(file.Key);
                    DeploymentFiles deploymentfile = new DeploymentFiles()
                    {
                        DeploymentID = addedID,
                        FilesName = file.Key,
                        Status = status.Deleted
                    };
                    unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
                    File.Move(AssemblyPath + @"\" + file.Key, NewBackupPath + @"\" + file.Key);
                    

                }else if (file.Value==status.Deleted)
                {
                   // files.Add(file.Key);
                    DeploymentFiles deploymentfile = new DeploymentFiles()
                    {
                        DeploymentID = addedID,
                        FilesName = file.Key,
                        Status = status.Added
                    };
                    unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
                    File.Copy(BackupPath + @"\" + file.Key, AssemblyPath + @"\" + file.Key);
                }
            }
          //  ibackupService.MoveTOBackUpFolder(files, AssemblyPath, NewBackupPath);
            //foreach (var file in allfiles)
            //{
            //    subs = file.Split(@"\"); 
            //    if (deploymentFiles.TryGetValue(subs[subs.Length - 1], out status statues))
            //    {
            //        if (statues == status.Added)
            //            File.Delete(AssemblyPath + @"\" + subs[subs.Length - 1]);
            //        else if (statues == status.Modified)
            //            File.Move(file, AssemblyPath + @"\" + subs[subs.Length - 1], true);
            //        else if (statues==status.Deleted)
            //            File.Move(file, AssemblyPath + @"\" + subs[subs.Length - 1], true);
            //    }



            //}

        }

        
    }
}
