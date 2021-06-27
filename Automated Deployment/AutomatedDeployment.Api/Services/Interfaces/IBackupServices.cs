using System;
using System.Collections.Generic;

namespace AutomatedDeployment.Api.Services
{
    public interface IBackupServices
    {
        void MoveTOBackUpFolder(List<string> filesName, string assemblyPath, string backupPath);
        DateTime MoveTOBackUpFolder(DateTime currentDate, string fileName, string backupPath);

    }
}
