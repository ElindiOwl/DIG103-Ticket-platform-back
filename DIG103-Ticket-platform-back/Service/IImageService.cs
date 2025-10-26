using DIG103_Ticket_platform_back.Model;

namespace DIG103_Ticket_platform_back.Service;

public interface IImageService
{
    Task<Image> UploadAsync(IFormFile file, string folder);
    Task DeleteAsync(int id);
}