using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
    public class BackupServices:IBackupServices
    {
        FileStream stream;
        public void MoveTOBackUpFolder(List<IFormFile> files, string path)
        {
            string dir = path;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
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
            //string[] filePaths = Directory.GetFiles("Your Path");
            //    //foreach (var filename in filePaths)
            //    //{
            //    //    string file = filename.ToString();
            //    //    string str = "Your Destination" + file.ToString(),Replace("Your Path");
            //    //    if (!File.Exists(str))
            //    //    {
            //    //        File.Copy(file, str);
            //    //    }
            //    //}
        }
    }
}
