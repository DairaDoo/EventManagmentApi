namespace EventManagmentApi.Service.Interfaces
{
    public interface IUploadImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
