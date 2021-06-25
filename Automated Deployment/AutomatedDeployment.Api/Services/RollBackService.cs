using AutomatedDeployment.Core.FactoryMethods;
using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutomatedDeployment.Api.Services
{
    public class RollBackService : IRollbackService
    {
        private readonly IPathRepository _pathRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RollBackService(IUnitOfWork unitOfWork, IPathRepository pathRepository)
        {
            _unitOfWork = unitOfWork;
            _pathRepository =pathRepository;
        }
        public bool GenralRollback(List<RollBackViewModel> rollBackViewModels)
        {
            if (rollBackViewModels is null ||  rollBackViewModels.Count == 0) return false;
           
            Deployment lastDeployment = _unitOfWork.DeploymentRepository.GetLastDeployment();
            DateTime lastdeploymentDate = lastDeployment.DeploymentDate;

            if (lastDeployment is null) return false;
            RollBackViewModel firstitem = rollBackViewModels[0];
            DateTime currentDate = DateTime.Now;
            int addedID = CreatAndSaveDeployment(firstitem.deployedBy, firstitem.requestedBy, firstitem.approvedBy, currentDate);
            if (addedID == 0) return false;
            foreach (var rollback in rollBackViewModels)
            {
                string AssemblyPath = _pathRepository.GetAssemblyPath(rollback.hubId, rollback.appID);
                string BackUpPath = _pathRepository.GetBackupPath(rollback.hubId, rollback.appID);
                if (AssemblyPath is null || BackUpPath is null) continue; 
              
                var deploymentFiles = _unitOfWork.DeploymentFilesRepository.GetById(rollback.hubId, rollback.appID);
                int addeddeploymentDetailsId = CreateAndSaveDeploymentDetails(rollback.hubId,rollback.appID,addedID);
                Rollback(rollback.hubId, rollback.appID, BackUpPath, AssemblyPath, addeddeploymentDetailsId, currentDate, deploymentFiles, lastdeploymentDate);
            }
            return true;

        }

        public int CreatAndSaveDeployment (string deployedBy, string requestedBy, string approvedBy, DateTime currentDate)
        {
            try
            {
                Deployment lastDeployment = _unitOfWork.DeploymentRepository.GetLastDeployment();
                Deployment deployment = Factory.createdeployment(currentDate, DeploymentType.Rollback,
                                lastDeployment.DeploymentID, deployedBy, approvedBy, requestedBy);
                Deployment newdep = _unitOfWork.DeploymentRepository.AddDeployment(deployment);
                return newdep.DeploymentID;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }

        }

        public int CreateAndSaveDeploymentDetails (int hubId, int appId , int newCreatedID)
        {
            try
            {
                DeploymentDetails deploymentDetails = Factory.createDeploymentDetails(hubId, appId, newCreatedID);
                int newDeploymentDetailsID = _unitOfWork.DeploymentDetailsRepository.AddDeploymentDetails(deploymentDetails).DeploymentDetailsId;
                return newDeploymentDetailsID;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
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
                        _unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
                        MoveFiles(AssemblyPath, NewBackupPath, file.Key);
                        CopyFiles(AssemblyPath, BackupPath, file.Key, lastDeploymentFolderDate);
                      
 
                    }
                    else if (file.Value == status.Added)
                    {
                        // Error By change Database
                        DeploymentFiles deploymentfile = new DeploymentFiles()
                        {
                           DeploymentDetailsId= deploymentDetailsId,
                            FilesName = file.Key,
                            Status = status.Deleted
                        };
                        _unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
                        MoveFiles(AssemblyPath, NewBackupPath, file.Key);

                        //File.Copy(BackupPath + @" \ " + lastDeploymentFolderDate + @"\" + file.Key, AssemblyPath + @"\" + file.Key);
                    }
                    else if (file.Value == status.Deleted)
                    {
                        // Error By change Database
                        DeploymentFiles deploymentfile = new DeploymentFiles()
                        {
                            DeploymentDetailsId= deploymentDetailsId,
                            FilesName = file.Key,
                            Status = status.Added
                        };

                        // Error By change Database
                        _unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
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
