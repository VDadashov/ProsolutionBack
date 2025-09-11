using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSolution.BL.Services.Interfaces
{
    public interface ICloudStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string containerName);
        Task<string> UploadFileAsync(IFormFile file, string containerName, string fileName);
        Task DeleteFileAsync(string fileUrl);
        Task<string> GetFileUrlAsync(string fileName, string containerName);
    }
}
