using DIG103_Ticket_platform_back.Model;

namespace DIG103_Ticket_platform_back.Repository;

public interface IArtistRepository
{
    Task<Artist> CreateAsync(Artist artist);
    Task<List<Artist>> GetAllWithRelationsAsync();
    Task<Artist?> GetByIdAsync(int id);
    Task<Artist?> GetByIdWithRelationsAsync(int id);
    Task<Artist?> UpdateAsync(Artist artist);
    Task DeleteAsync(Artist artist);
    Task<bool> ExistsByNameAsync(string name);
}