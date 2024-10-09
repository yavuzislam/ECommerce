using Microsoft.AspNetCore.Http;
using Payment.BusinessLayer.Abstract;

namespace Payment.BusinessLayer.Concrete;

public class FileService : IFileService
{
    private readonly string[] _allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

    public async Task<string> UploadFileAsync(IFormFile file, string uploadsFolder)
    {
        if (file == null || file.Length == 0)
        {
            return null; // Dosya yoksa null dön
        }

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!_allowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException("Yalnızca .jpg, .jpeg, .png, .gif uzantılı dosyalar kabul edilmektedir.");
        }

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Dosyayı belirtilen klasöre kaydet
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        // Dosya yolunu geri döndür
        return "/images/" + uniqueFileName;
    }
}
