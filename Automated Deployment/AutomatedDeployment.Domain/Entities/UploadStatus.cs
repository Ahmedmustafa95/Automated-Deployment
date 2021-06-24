using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Domain.Entities
{
    public enum UploadStatus
    {
        Success,
        DatabaseFailure,
        AssembyNotExist,
        NotValidData,
        Uploadfailed,
        DeletedFailed

    }
}
