using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EventManagmentApi.Service.Interfaces;
using EventManagmentApi.Helpers.Cloudinary;

namespace EventManagmentApi.Service
{
    public class UploadImageService : IUploadImageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly string[] _allowedFormats = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public UploadImageService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("El archivo de imagen no puede estar vacío.");
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!_allowedFormats.Contains(fileExtension))
            {
                throw new ArgumentException("Formato de imagen no permitido. Solo se permiten archivos .jpg, .jpeg, .png, .gif y .webp.");
            }

            try
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Quality(80).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new Exception($"Error en la subida de la imagen: {uploadResult.Error.Message}");
                }

                return uploadResult.SecureUrl?.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al subir la imagen: {ex.Message}");
            }
        }
    }
}
