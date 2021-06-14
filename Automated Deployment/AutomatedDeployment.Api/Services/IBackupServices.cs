using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
    public interface IBackupServices
    {
        void MoveTOBackUpFolder(List<string> filesName, string assemblyPath, string backupPath);
    }
}
