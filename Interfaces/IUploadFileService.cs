using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Hero_Project.NetCore5.Interfaces
{
    public interface IUploadFileService
    {
        bool IsUpload(List<IFormFile> formFiles);
        string Validation(List<IFormFile> formFiles);
        Task<List<string>> UploadImages(List<IFormFile> formFiles);

    }
}