using AutomatedDeployment.Core.FactoryMethods;
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
        private readonly IPathRepository pathRepository;
        private readonly IUnitOfWork unitOfWork;

        public RollBackService(IUnitOfWork _unitOfWork, IPathRepository _pathRepository)
        {
            unitOfWork = _unitOfWork;
            pathRepository = _pathRepository;
        }
        public void HubRollback(int hubid, string deployedBy, string approvedBy, string requestedBy)
        {
            Deployment lastDeployment = unitOfWork.DeploymentRepository.GetLastDeployment();
            List<int> applicationIds = unitOfWork.DeploymentDetailsRepository.GetApplicationID(lastDeployment.DeploymentID, hubid);
            foreach (var applicationID in applicationIds)
            {
                SingleRollback(hubid, applicationID);
            }

        }


        public void SingleRollback (int hubid,int applicationid)
        {
            string AssemblyPath = pathRepository.GetAssemblyPath(hubid, applicationid);
            string BackUpPath = pathRepository.GetBackupPath(hubid, applicationid);
            var currentDate = DateTime.Now;
            var lastdeploymentDate = unitOfWork.DeploymentRepository.GetLastDeployment().DeploymentDate;
            var y = unitOfWork.DeploymentFilesRepository.GetById(hubid, applicationid);
            int x = RollbackHelp(hubid, applicationid, "shawky", "belal", "eslam", currentDate);
            Rollback(hubid, applicationid, BackUpPath, AssemblyPath, x, currentDate, y, lastdeploymentDate);
        }
        public int RollbackHelp(int hubid, int applicationid,string deployedBy , string requestedBy , string  approvedBy ,DateTime currentDate)
        {

            var lastDeployment = unitOfWork.DeploymentRepository.GetLastDeployment();
            //rollback Deployment
            var deployment =  Factory.createdeployment(currentDate, DeploymentType.Rollback, 
                            lastDeployment.DeploymentID, deployedBy, approvedBy, requestedBy);
            //create new  Deployment 
            // var newdep = unitOfWork.DeploymentFilesRepository.GetLastDepolyment(hubid,applicationid);
           var newdep = unitOfWork.DeploymentRepository.AddDeployment(deployment);
            //last DeploymentDetails ID
            //var dd = unitOfWork.DeploymentDetailsRepository.GetLastDepolymentDetails(hubid, applicationid).DeploymentDetailsId;
            //create new Deployment details Object
            var deploymentDetails = Factory.createDeploymentDetails(hubid, applicationid, newdep.DeploymentID);

        
            //Save new DeploymentDetails 
          var dd= unitOfWork.DeploymentDetailsRepository.AddDeploymentDetails(deploymentDetails).DeploymentDetailsId;
            return dd;
        }


        public void Rollback(int hubid, int applicationid, string BackupPath, string AssemblyPath,int deploymentDetailsId,DateTime currentDate, Dictionary<string, status> deploymentFiles,DateTime lastDeploymentDate)

        {
            try
            {


                string NewBackupPath = $"{BackupPath}\\BK_{currentDate.ToString("yyyy-MM-dd-hh-mm-ss")}".Trim();
                
                Directory.CreateDirectory(NewBackupPath);

                string lastDeploymentFolderDate = $"BK_{lastDeploymentDate.ToString("yyyy-MM-dd-hh-mm-ss")}".Trim();

                foreach (var file in deploymentFiles)
                {
                    if (file.Value == status.Modified)
                    {
                        
                        DeploymentFiles deploymentfile = new DeploymentFiles()
                        {
                            DeploymentDetailsId = deploymentDetailsId,
                            FilesName = file.Key,
                            Status = status.Modified
                        };

                     
                        unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);

                        MoveFiles(AssemblyPath, NewBackupPath, file.Key);
                        CopyFiles(AssemblyPath, BackupPath, file.Key, lastDeploymentFolderDate);
                      
 
                    }
                    else if (file.Value == status.Added)
                    {
                        // files.Add(file.Key);

                        // Error By change Database
                        DeploymentFiles deploymentfile = new DeploymentFiles()
                        {
                           DeploymentDetailsId= deploymentDetailsId,
                            FilesName = file.Key,
                            Status = status.Deleted
                        };

                        
                        unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
                        MoveFiles(AssemblyPath, NewBackupPath, file.Key);

                        //File.Copy(BackupPath + @" \ " + lastDeploymentFolderDate + @"\" + file.Key, AssemblyPath + @"\" + file.Key);
                    }
                    else if (file.Value == status.Deleted)
                    {
                        // files.Add(file.Key);

                        // Error By change Database
                        DeploymentFiles deploymentfile = new DeploymentFiles()
                        {
                            DeploymentDetailsId= deploymentDetailsId,
                            FilesName = file.Key,
                            Status = status.Added
                        };

                        // Error By change Database
                        unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
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
