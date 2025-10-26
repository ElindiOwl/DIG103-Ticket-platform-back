using DIG103_Ticket_platform_back.Model;

namespace DIG103_Ticket_platform_back.Repository;

public interface IArtistRepository
{
    Task<Artist> CreateAsync(Artist artist);
    Task<Artist?> GetByIdAsync(int id);
    Task<Artist?> UpdateAsync(Artist artist);
    Task DeleteAsync(Artist artist);
    Task<List<int>> GetEventIdsAsync(int id);
    Task<bool> ExistsByNameAsync(string name);
}