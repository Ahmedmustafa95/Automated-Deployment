using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomatedDeployment.Core.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutomatedDeployment.Infrastructure.Context;
using System.IO;
using AutomatedDeployment.Domain.Entities;

namespace AutomatedDeployment.Api.Services
{
    class Deletefilesrepository : IDeletefilesrepository
    {
        private readonly EfgconfigurationdbContext efgconfigurationdbContext;
        private readonly IUnitOfWork unitOfWork;
        public Deletefilesrepository(EfgconfigurationdbContext _efgconfigurationdbContext, IUnitOfWork _unitOfWork)
        {
            efgconfigurationdbContext = _efgconfigurationdbContext;
            unitOfWork = _unitOfWork;
        }
        public void Deletefiles(int hubid, int appid, List<IFormFile> files)
            {
            try
            {

               
                var hubappobject = efgconfigurationdbContext.HubsApplications.Where(h => h.HubID == hubid && h.AppID == appid).FirstOrDefault();

                var AssemblyPath = hubappobject.AssemblyPath;
                var BackUpPath = hubappobject.BackupPath;
                var currentDate = DateTime.Now;
                string NewBackupPath = $"{BackUpPath} \\ BK_{currentDate.ToString("yyyy-MM-dd-hh-mm-ss")}";
                Directory.CreateDirectory(NewBackupPath);
                // Dictionary has Files Name as key and Files state as value

                Deployment deployment = new Deployment()
                {
                    HubID = hubid,
                    AppID = appid,
                    DeploymentDate = currentDate,
                    ApprovedBy = "chris",
                    RequestedBy = "belal",
                };
                int addedID = unitOfWork.DeploymentRepository.AddDeployment(deployment).DeploymentID;
                foreach (var file in files)
                {
                    DeploymentFiles deploymentfile = new DeploymentFiles()
                    {
                        DeploymentID = addedID,
                        FilesName = file.FileName,
                        Status = status.Deleted
                    };
                    unitOfWork.DeploymentFilesRepository.AddDeploymentFile(deploymentfile);
                    File.Move(AssemblyPath + @"\" + file.FileName, NewBackupPath + @"\" + file.FileName);
                    
                }
            

            }
            catch
            {

            }
            
        }

        public string[] GetAllfiles(int hubid, int appid)
        {
            try
            {
                var assembly= efgconfigurationdbContext.HubsApplications.Where(h => h.HubID == hubid && h.AppID == appid).FirstOrDefault();
                string[] filePaths = Directory.GetFiles(assembly.AssemblyPath);
                return filePaths;
            }catch
            {
                return null;
            }
          
        }
    }
}
