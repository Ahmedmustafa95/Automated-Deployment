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
        public bool GenralRollback(List<RollBackViewModel> rollBackViewModels)
        {
            if (rollBackViewModels is null ||  rollBackViewModels.Count == 0) return false;
           
            Deployment lastDeployment = unitOfWork.DeploymentRepository.GetLastDeployment();
            var lastdeploymentDate = lastDeployment.DeploymentDate;


            if (lastDeployment is null) return false;
            RollBackViewModel firstitem = rollBackViewModels[0];
            var currentDate = DateTime.Now;
            int addedID = CreatAndSaveDeployment(firstitem.deployedBy, firstitem.requestedBy, firstitem.approvedBy, currentDate);
            if (addedID == 0) return false;
            //List<int> applicationIds = unitOfWork.DeploymentDetailsRepository.GetApplicationID(lastDeployment.DeploymentID, hubid);
            foreach (var rollback in rollBackViewModels)
            {
                // SingleRollback(hubid, applicationID);
                string AssemblyPath = pathRepository.GetAssemblyPath(rollback.hubId, rollback.appID);
                string BackUpPath = pathRepository.GetBackupPath(rollback.hubId, rollback.appID);
                if (AssemblyPath is null || BackUpPath is null) continue; 
              
                var deploymentFiles = unitOfWork.DeploymentFilesRepository.GetById(rollback.hubId, rollback.appID);
                int addeddeploymentDetailsId = CreateAndSaveDeploymentDetails(rollback.hubId,rollback.appID,addedID);
                Rollback(rollback.hubId, rollback.appID, BackUpPath, AssemblyPath, addeddeploymentDetailsId, currentDate, deploymentFiles, lastdeploymentDate);
            }
            return true;

        }

        public int CreatAndSaveDeployment (string deployedBy, string requestedBy, string approvedBy, DateTime currentDate)
        {
            try
            {
                var lastDeployment = unitOfWork.DeploymentRepository.GetLastDeployment();
                //rollback Deployment
                var deployment = Factory.createdeployment(currentDate, DeploymentType.Rollback,
                                lastDeployment.DeploymentID, deployedBy, approvedBy, requestedBy);
                var newdep = unitOfWork.DeploymentRepository.AddDeployment(deployment);
                return newdep.DeploymentID;
            }
            catch(Exception e)
            {
                return 0;
            }

        }

        public int CreateAndSaveDeploymentDetails (int hubId, int appId , int newCreatedID)
        {
            try
            {
                var deploymentDetails = Factory.createDeploymentDetails(hubId, appId, newCreatedID);


                //Save new DeploymentDetails 
                var newDeploymentDetailsID = unitOfWork.DeploymentDetailsRepository.AddDeploymentDetails(deploymentDetails).DeploymentDetailsId;
                return newDeploymentDetailsID;
            }catch(Exception e)
            {
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
