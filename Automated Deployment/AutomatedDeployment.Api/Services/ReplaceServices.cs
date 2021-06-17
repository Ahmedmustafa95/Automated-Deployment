using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedDeployment.Api.Services
{
    public class ReplaceServices:IReplaceServices
    {
        FileStream stream;
        public void Upload(List<IFormFile> files, string path)
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



        //public async void Upload(IFormFile file)
        //{

        //            var filePath = Path.GetFullPath(@"F:\iti\repositories\trying\ahmed.txt");

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await file.CopyToAsync(stream);
        //            }
        //}
    }
}
