namespace DIG103_Ticket_platform_back.Service;

public interface IMinioService
{
    Task<string> UploadImageAsync(IFormFile file, string folder);
    string GetPublicUrl(string imagePath);
    Task DeleteImageAsync(string imagePath);
}