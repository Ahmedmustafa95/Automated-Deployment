using System.Collections.Generic;

namespace AutomatedDeployment.Api.Services
{
    public interface IBackupServices
    {
        void MoveTOBackUpFolder(List<string> filesName, string assemblyPath, string backupPath);
        void MoveTOBackUpFolder(string fileName, string backupPath);

    }
}
