using System.Collections.ObjectModel;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hero_Project.NetCore5.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Hero_Project.NetCore5.Services
{
    public class UploadFileService : IUploadFileService
    {
      
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        
        //DI IwebHost permission for plaste image in folder
        public UploadFileService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
        }
        //Image is not null and Size > 0
        public bool IsUpload(List<IFormFile> formFiles) => formFiles != null && formFiles.Sum(f => f.Length) > 0;
        

        public string UploadImage(List<IFormFile> formFiles)
        {
            throw new System.NotImplementedException();
        }

        //Upload image
        public async Task<List<string>> UploadImages(List<IFormFile> formFiles)
        {
            List<string> listFileName = new List<string>();
            //path of destination for upload image
            string uploadPath = $"{webHostEnvironment.WebRootPath}/images/";

            foreach (var formFile in formFiles) {
                //fileName is name of file in database
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                string fullPath = uploadPath + fileName;
                using (var stream = File.Create(fullPath)){
                    await formFile.CopyToAsync(stream);
                }
                listFileName.Add(fileName);
            }
            return listFileName;
        }

        public string Validation(List<IFormFile> formFiles)
        {
            foreach (var formFile in formFiles) {
                if (!ValidationExtension(formFile.FileName)){
                    return "Invalid file extension";
                }

                if (!ValidationSize(formFile.Length)) {
                    return "The file is too large";
                }
            }
            return null;
        }
        //Validate
        public bool ValidationExtension(string fileName){
            //type of file can upload
            string[] permittedExtensions = {".jpg", ".png"};
            //
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            //Validate null or empty or type file is wrong return false
            if(String.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext)){
                return false;
            } else {
                return true;
            }
        }

        //Validate file size
        public bool ValidationSize(long fileSize) => configuration.GetValue<long>("FileSizeLimit") > fileSize;
    }
}