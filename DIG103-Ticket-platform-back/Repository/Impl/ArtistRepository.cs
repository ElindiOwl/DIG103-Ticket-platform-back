using DIG103_Ticket_platform_back.Model;
using DIG103_Ticket_platform_back.Data;
using Microsoft.EntityFrameworkCore;

namespace DIG103_Ticket_platform_back.Repository.Impl;

public class ArtistRepository(AppliactionDbContext context) : IArtistRepository
{
    public async Task<Artist> CreateAsync(Artist artist)
    {
        context.Artists.Add(artist);

        await context.SaveChangesAsync();
        return artist;
    }
    
    public async Task<Artist?> GetByIdAsync(int id)
    {
        return await context.Artists
            .Include(a => a.ArtistImage)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Artist?> UpdateAsync(Artist artist)
    {
        context.Artists.Update(artist);

        await context.SaveChangesAsync();
        return artist;
    }

    public async Task DeleteAsync(Artist artist)
    {
        context.Artists.Remove(artist);

        await context.SaveChangesAsync();
    }

    public async Task<List<int>> GetEventIdsAsync(int id)
    {
        return await context.Artists
            .Where(a => a.Id == id)
            .SelectMany(a => a.Events.Select(e => e.Id))
            .ToListAsync();
    }
    
    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await context.Artists
            .AnyAsync(a => a.Name == name);
    }
}