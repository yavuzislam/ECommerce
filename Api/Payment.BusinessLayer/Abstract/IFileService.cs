using Microsoft.AspNetCore.Http;

namespace Payment.BusinessLayer.Abstract;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, string uploadsFolder);
}
