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

        public RollBackService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public void Rollback(int hubid, int applicationid, string BackupPath, string AssemblyPath, Dictionary<string, status> deploymentFiles)

        {
            // string BackUpPath = pathRepository.GetBackupPath(hubid, applicationid);

            //if (!Directory.Exists(BackUpPath))
            //    Directory.CreateDirectory(BackUpPath);
            try
            {

                var currentDate = DateTime.Now;

                string NewBackupPath = $"{BackupPath}\\BK_{currentDate.ToString("yyyy-MM-dd-hh-mm-ss")}".Trim();
                Directory.CreateDirectory(NewBackupPath);

                // Dictionary has Files Name as key and Files state as value

                //var filesName = files.Select(f => f.FileName).ToList();
                //ibackupService.MoveTOBackUpFolder(filesName, AssemblyPath, BackUpPath);
                //List<string> files = new List<string>();

                var lastDeploymentFolderDate = $"BK_{unitOfWork.DeploymentFilesRepository.GetLastDepolyment(hubid, applicationid).DeploymentDate.ToString("yyyy-MM-dd-hh-mm-ss")}".Trim();
                // Error By change Database
                //Deployment deployment = new Deployment()
                //{
                //    HubID = hubid,
                //    AppID = applicationid,
                //    DeploymentDate = currentDate,
                //    DeployedBy="belal",
                //    ApprovedBy = "Shawky",
                //    RequestedBy = "Mustafa",
                //};



                // Error By change Database
                //int addedID = unitOfWork.DeploymentRepository.AddDeployment(deployment).DeploymentID;


                //read all files 
                //string[] allfiles = Directory.GetFiles(BackupPath);
                //string[] subs;
                //put files in new path

                foreach (var file in deploymentFiles)
                {
                    if (file.Value == status.Modified)
                    {
                        //files.Add(file.Key);


                        // Error By change Database
                        //DeploymentFiles deploymentfile = new DeploymentFiles()
                        //{
                        //    DeploymentID = addedID,
                        //    FilesName = file.Key,
                        //    Status = status.Modified
                        //};

                        // Error By change Database
                        //unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);

                        MoveFiles(AssemblyPath, NewBackupPath, file.Key);
                        

                        CopyFiles(AssemblyPath, BackupPath, file.Key, lastDeploymentFolderDate);

                        //File.Copy($"{BackupPath}{@"\"}{lastDeploymentFolderDate}{@"\"}{file.Key}"
                        //                                           .Trim()
                        //         ,$"{AssemblyPath}{@"\"}{file.Key}".Trim());

                    }
                    else if (file.Value == status.Added)
                    {
                        // files.Add(file.Key);

                        // Error By change Database
                        //DeploymentFiles deploymentfile = new DeploymentFiles()
                        //{
                        //    DeploymentID = addedID,
                        //    FilesName = file.Key,
                        //    Status = status.Deleted
                        //};

                        // Error By change Database
                        //unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
                        MoveFiles(AssemblyPath, NewBackupPath, file.Key);

                        //File.Copy(BackupPath + @" \ " + lastDeploymentFolderDate + @"\" + file.Key, AssemblyPath + @"\" + file.Key);
                    }
                    else if (file.Value == status.Deleted)
                    {
                        // files.Add(file.Key);

                        // Error By change Database
                        //DeploymentFiles deploymentfile = new DeploymentFiles()
                        //{
                        //    DeploymentID = addedID,
                        //    FilesName = file.Key,
                        //    Status = status.Added
                        //};

                        // Error By change Database
                        //unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
                        CopyFiles(AssemblyPath, BackupPath, file.Key, lastDeploymentFolderDate);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void MoveFiles(string assemblyPath,string newBackupPath,string fileName)
        {
            try
            {
                File.Move($"{assemblyPath}{@"\"}{fileName}".Trim(),
                          $"{newBackupPath}{@"\"}{fileName}".Trim());
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void CopyFiles(string assemblyPath, string backupPath, 
                              string fileName,string lastDeploymentDate)
        {
            try
            {
                File.Copy($"{backupPath}{@"\"}{lastDeploymentDate}{@"\"}{fileName}"
                                                           .Trim()
                         , $"{assemblyPath}{@"\"}{fileName}".Trim());
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }




    }
}
