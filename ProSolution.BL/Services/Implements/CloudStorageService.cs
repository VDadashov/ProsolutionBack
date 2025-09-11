using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ProSolution.BL.Services.Interfaces;
using ProSolution.BL.Settings;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProSolution.BL.Services.Implements
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly Cloudinary _cloudinary;

        public CloudStorageService(IOptions<CloudinarySettings> options)
        {
            var account = new Account(
                options.Value.CloudName,
                options.Value.ApiKey,
                options.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName)
        {
            var fileName = GenerateFileName(file.FileName);
            return await UploadFileAsync(file, containerName, fileName);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName, string fileName)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, stream),
                Folder = containerName
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new Exception(uploadResult.Error.Message);

            return uploadResult.SecureUrl.ToString(); 
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
           
            var publicId = GetPublicIdFromUrl(fileUrl);
            var deletionParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deletionParams);

            if (result.Result != "ok")
                throw new Exception($"Failed to delete file from Cloudinary: {result.Error?.Message}");
        }

        public Task<string> GetFileUrlAsync(string fileName, string containerName)
        {
            var url = _cloudinary.Api.UrlImgUp
                .BuildUrl($"{containerName}/{fileName}");
            return Task.FromResult(url);
        }

        private string GenerateFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            return $"{Guid.NewGuid()}{extension}";
        }

        private string GetPublicIdFromUrl(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            var segments = uri.AbsolutePath.Split('/');
            var fileName = segments[^1];
            var publicId = Path.Combine(segments[^2], Path.GetFileNameWithoutExtension(fileName))
                                .Replace("\\", "/"); 
            return publicId;
        }
    }
}
