using DIG103_Ticket_platform_back.Model;

namespace DIG103_Ticket_platform_back.Repository;

public interface IImageRepository
{
    Task<Image> CreateAsync(Image image);
    Task<Image?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
}