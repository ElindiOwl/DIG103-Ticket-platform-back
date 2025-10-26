using DIG103_Ticket_platform_back.Data;
using DIG103_Ticket_platform_back.DTO.Event;
using DIG103_Ticket_platform_back.Model;
using Microsoft.EntityFrameworkCore;

namespace DIG103_Ticket_platform_back.Repository.Impl;

public class EventRepository(AppliactionDbContext context) : IEventRepository
{
    public async Task<Event> CreateAsync(Event eventData)
    {
        context.Events.Add(eventData);

        await context.SaveChangesAsync();
        return eventData;
    }

    public async Task<List<Event>> GetFeaturedAsync()
    {
        return await context.Events
            .Where(e => e.IsFeatured)
            .Include(e => e.Theme)
            .Include(e => e.Features)
            .ThenInclude(f => f.FeatureImage)
            .Include(e => e.Artists)
            .Include(e => e.EventImage)
            .Include(e => e.EventBackground)
            .ToListAsync();
    }
    
    public async Task<Event?> GetWithRelationsAsync(int id)
    {
        return await context.Events
            .Include(e => e.Theme)
            .Include(e => e.Features)
            .ThenInclude(f => f.FeatureImage)
            .Include(e => e.Artists)
            .Include(e => e.EventImage)
            .Include(e => e.EventBackground)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Event?> UpdateAsync(Event eventData)
    {
        context.Events.Update(eventData);

        await context.SaveChangesAsync();
        return eventData;
    }

    public async Task DeleteAsync(Event eventData)
    {
        context.Events.Remove(eventData);

        await context.SaveChangesAsync();
    }

    public async Task<EventFeature> CreateFeatureAsync(int eventId, EventFeature eventFeature)
    {
        var eventData = await context.Events
            .Include(e => e.Features)
            .FirstOrDefaultAsync(e => e.Id == eventId);
        
        eventData!.Features.Add(eventFeature);
        
        await context.SaveChangesAsync();
        return eventFeature;
    }

    public async Task<EventFeature?> UpdateFeatureAsync(EventFeature eventFeature)
    {
        context.Features.Update(eventFeature);

        await context.SaveChangesAsync();
        return eventFeature;
    }

    public async Task AddArtistAsync(int eventId, int id)
    {
        var eventData = await context.Events
            .Include(e => e.Artists)
            .FirstOrDefaultAsync(e => e.Id == eventId);

        var artist = await context.Artists.FindAsync(id);
        
        eventData!.Artists.Add(artist!);
        await context.SaveChangesAsync();
    }

    public async Task RemoveArtistAsync(int eventId, int id)
    {
        var eventData = await context.Events
            .Include(e => e.Artists)
            .FirstOrDefaultAsync(e => e.Id == eventId);

        var artist = eventData!.Artists.FirstOrDefault(a => a.Id == id);

        eventData.Artists.Remove(artist!);
        await context.SaveChangesAsync();
    }

    public async Task<bool> IsArtistExistsAsync(int id)
    {
        return await context.Artists.AnyAsync(a => a.Id == id);
    }
    public async Task DeleteFeatureAsync(int eventId, int id)
    {
        var feature = await context.Features
            .Include(f => f.FeatureImage)
            .FirstOrDefaultAsync(f => f.Id == id && f.EventId == eventId);

        if (feature != null)
        {
            context.Features.Remove(feature);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await context.Events
            .AnyAsync(e => e.Name == name);
    }
}