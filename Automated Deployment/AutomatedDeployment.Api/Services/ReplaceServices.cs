using AutomatedDeployment.Core.Interfaces;
using AutomatedDeployment.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
    public class ReplaceServices : IReplaceServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBackupServices _backupServices;
        private readonly IPathRepository _pathRepository;

        public ReplaceServices(IUnitOfWork unitOfWork, IBackupServices backupServices
                                                     , IPathRepository pathRepository)
        {
            _unitOfWork = unitOfWork;
            _backupServices = backupServices;
            _pathRepository = pathRepository;
        }

        FileStream stream;
        public bool Upload(List<IFormFile> files, string path)
        {
            string dir = path;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            try
            {
                foreach (var formFile in files)
                {

                    var filePath = Path.GetFullPath(dir + formFile.FileName);

                    // full path to file in temp location
                    //var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.
                    // filePaths.Add(filePath);
                    using (stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           

        }

        public Dictionary<string, List<string>> CompareDeployFilesWithAssemblyFiles(List<IFormFile> Uploadedfiles, string assemblyPath)
        {
            Dictionary<string, List<string>> filesState = new Dictionary<string, List<string>>();

            string[] assemblyFiles = Directory.GetFiles(assemblyPath);

            List<string> ModifiedFiles = new List<string>();
            List<string> AddedFiles = new List<string>();
            foreach (var item in Uploadedfiles)
            {
                if (assemblyFiles.Contains($"{assemblyPath}{item.FileName}"))
                    ModifiedFiles.Add(item.FileName);
                else
                    AddedFiles.Add(item.FileName);
            }
            filesState.Add("Added", AddedFiles);
            filesState.Add("Modified", ModifiedFiles);
            return filesState;


        }

        public bool CheckValidData(int hubid, int applicationid) =>
         _unitOfWork.HubsApplicationsRepository
                   .GetHubsApplicationByID(hubid, applicationid) != null;


        public Dictionary<string,List<string>> GenerateAndMoveToBackupFolder(List<IFormFile> files
                                                 , string assemblyPath,
                                                  DateTime backupDate)
        {
            Dictionary<string, List<string>> filesState = default;
            string BackUpPath = $"{assemblyPath}backups".Trim();
            filesState = CompareDeployFilesWithAssemblyFiles(files, assemblyPath);

            if (filesState["Modified"].Count > 0)
            {

                List<string> backupFiles = new List<string>();
                backupFiles.AddRange(filesState["Modified"]);
                string NewBackupPath = $"{BackUpPath}\\BK_{backupDate.ToString("yyyy-MM-dd-hh-mm-ss")}".Trim();
                Directory.CreateDirectory(NewBackupPath);
                _backupServices.MoveTOBackUpFolder(backupFiles, assemblyPath, NewBackupPath);
            }
            return filesState;
        }

        public Deployment AddDeploymentService(string approvedBy, string requestedBy, string developedBy,
                                  DateTime currentDate)
        {
            try
            {
                return new Deployment()
                {
                    DeploymentDate = currentDate,
                    DeploymentType = DeploymentType.Deployment,
                    DeployedBy = developedBy,
                    ApprovedBy = approvedBy,
                    RequestedBy = requestedBy,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DeploymentDetails AddDeploymentDetailService(int hubId, int appId, int deploymentId)
        {
            try
            {
                return new DeploymentDetails
                {
                    HubId = hubId,
                    AppId = appId,
                    DeploymentId = deploymentId
                };
            }
            catch (Exception E)
            {
                return null;
            }



        }


        public List<DeploymentFiles>AddDeploymentFiles(Dictionary<string, List<string>> filesState,
                                       List<HubsApplications> hubsApplications)
        {
            var deploymentFiles = new List<DeploymentFiles>();
            DeploymentFiles deploymentFile=default;
            if (hubsApplications is null || filesState is null) return null;

            try
            {
                foreach (var hubsApplication in hubsApplications)
                {
                    if (_unitOfWork.DeploymentRepository.GetDeploymentCounts(hubsApplication.HubID, hubsApplication.AppID) > 0)
                    {

                        int deploymentDetailId = _unitOfWork.DeploymentDetailsRepository
                                                             .GetDeploymentDetailsIdByHubIdAndAppId
                                                             (
                                                                hubsApplication.HubID,
                                                                hubsApplication.AppID
                                                             );
                        if (deploymentDetailId == -1) return null;
                        foreach (var fileName in filesState["Modified"])
                        {
                            deploymentFile = new DeploymentFiles()
                            { DeploymentDetailsId = deploymentDetailId, FilesName = fileName, Status = status.Modified };
                            deploymentFiles.Add(deploymentFile);
                        }

                        foreach (var fileName in filesState["Added"])
                        {
                            deploymentFile = new DeploymentFiles()
                            { DeploymentDetailsId = deploymentDetailId, FilesName = fileName, Status = status.Added };
                            deploymentFiles.Add(deploymentFile);
                        }

                    }

                }
                return deploymentFiles;
            }
            catch (Exception E)
            {

                return null;
            }
            
        }

        public UploadStatus UploadAndBackupFiles(UploadingFileViewModel fileViewModel)
        {
            List<IFormFile>files = fileViewModel.files;
            List<HubsApplications> hubsApplications = fileViewModel.HubsApplications;
            //var hubsApplications = new List<HubsApplications>
            //{
            //    new HubsApplications
            //    {
            //        HubID=17,
            //        AppID=8
            //    },
            //     new HubsApplications
            //    {
            //        HubID=17,
            //        AppID=9
            //    },
            //      new HubsApplications
            //    {
            //        HubID=17,
            //        AppID=10
            //    }
            //};
            var deploymentDetails = new List<DeploymentDetails>();
            var deploymentFiles = new List<DeploymentFiles>();
            var currentDate = DateTime.Now;

            var deployment = AddDeploymentService(fileViewModel.ApprovedBy ,fileViewModel.RequestedBy,fileViewModel.DeployedBy, currentDate);
            if (_unitOfWork.DeploymentRepository.AddDeployment(deployment) is null)
                return UploadStatus.DatabaseFailure;

            int currentDeploymentId = _unitOfWork.DeploymentRepository.GetCurrentDeploymentId();
            Dictionary<string, List<string>> filesState = default;

            foreach (var hubsApplication in hubsApplications)
            {
                if (!CheckValidData(hubsApplication.HubID, hubsApplication.AppID)) return UploadStatus.NotValidData;
                string assemblyPath = $"{_pathRepository.GetAssemblyPath(hubsApplication.HubID,hubsApplication.AppID)}{@"\"}".Trim();
                if (assemblyPath is null) { return UploadStatus.AssembyNotExist; }

                if (_unitOfWork.DeploymentRepository.GetDeploymentCounts(hubsApplication.HubID, hubsApplication.AppID) > 0)
                {
                    filesState= GenerateAndMoveToBackupFolder(files, assemblyPath, currentDate);
                    bool isUploadSuccess= Upload(files, assemblyPath);

                    if (!isUploadSuccess)
                    {
                        if(!RemoveDeploymentFromDatabase(currentDeploymentId)) return UploadStatus.DeletedFailed;
                        return UploadStatus.Uploadfailed;

                    }

                    var deploymentDetail = AddDeploymentDetailService(hubsApplication.HubID, hubsApplication.AppID,currentDeploymentId);
                    deploymentDetails.Add(deploymentDetail);

                }
                else
                {
                    Upload(files, assemblyPath);

                    var deploymentDetail = AddDeploymentDetailService(hubsApplication.HubID, hubsApplication.AppID, currentDeploymentId);
                    deploymentDetails.Add(deploymentDetail);

                }

            }

          

            if (_unitOfWork.DeploymentDetailsRepository.AddDeploymentDetails(deploymentDetails) is null)
                return UploadStatus.DatabaseFailure;

            deploymentFiles = AddDeploymentFiles(filesState,hubsApplications);
            if ((deploymentFiles?.Count?? 0) > 0)
            {
                if (_unitOfWork.DeploymentFilesRepository.AddDeploymentFiles(deploymentFiles) is null)
                    return UploadStatus.DatabaseFailure;
            }
            
            return UploadStatus.Success;
        }
        
        public bool RemoveDeploymentFromDatabase(int deploymentId)
            => _unitOfWork.DeploymentRepository.DeleteFromDeployment(deploymentId);
       
    }
}
