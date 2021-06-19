using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using AutomatedDeployment.Core.FactoryMethods;
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

            var currentDate = DateTime.Now;
            string NewBackupPath = $"{BackupPath} \\ BK_{currentDate.ToString("yyyy-MM-dd-hh-mm-ss")}";
            Directory.CreateDirectory(NewBackupPath);


            var lastDeploymentFolderDate = $"BK_{unitOfWork.DeploymentFilesRepository.GetLastDepolyment(hubid, applicationid).DeploymentDate.ToString("yyyy-MM-dd-hh-mm-ss")}";

            var deployment = Factory.CreateDeployment(hubid, applicationid, currentDate, "shawky", "Belal");
            int addedID = unitOfWork.DeploymentRepository.AddDeployment(deployment).DeploymentID;

            foreach (var file in deploymentFiles)
            {
                RollBackState(file.Value, addedID, file.Key, AssemblyPath, BackupPath, NewBackupPath, lastDeploymentFolderDate);
            }


        }
        public void RollBackState(status stat, int addedID, string filname, string AssemblyPath, string BackupPath, string NewBackupPath, string lastDeploymentFolderDate)
        {
            if (stat == status.Modified)
            {
                try
                {
                    var deploymentfile = Factory.CreateDeploymentFile(addedID, filname, status.Modified);
                    unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
                    File.Move(AssemblyPath + @"\" + filname, NewBackupPath + @"\" + filname);
                    File.Copy(BackupPath + @" \ " + lastDeploymentFolderDate + @"\" + filname, AssemblyPath + @"\" + filname);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            else if (stat == status.Added)
            {
                try
                {
                    var deploymentfile = Factory.CreateDeploymentFile(addedID, filname, status.Deleted);
                    unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
                    File.Move(AssemblyPath + @"\" + filname, NewBackupPath + @"\" + filname);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if (stat == status.Deleted)
            {
                try
                {
                    var deploymentfile = Factory.CreateDeploymentFile(addedID, filname, status.Added);
                    unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
                    File.Copy(BackupPath + @" \ " + lastDeploymentFolderDate + @"\" + filname, AssemblyPath + @"\" + filname);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

    }
}
