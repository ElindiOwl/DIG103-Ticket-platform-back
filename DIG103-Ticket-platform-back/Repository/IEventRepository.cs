using DIG103_Ticket_platform_back.Model;

namespace DIG103_Ticket_platform_back.Repository;

public interface IEventRepository
{
    Task<Event> CreateAsync(Event eventData);
    Task<List<Event>> GetFeaturedAsync();
    Task<Event?> GetWithRelationsAsync(int id);
    Task<Event?> UpdateAsync(Event eventData);
    Task DeleteAsync(Event eventData);
    
    Task<EventFeature> CreateFeatureAsync(int eventId, EventFeature eventFeature);
    Task<EventFeature?> UpdateFeatureAsync(EventFeature eventFeature);
    Task DeleteFeatureAsync(int eventId, int id);

    Task AddArtistAsync(int eventId, int id);
    Task RemoveArtistAsync(int eventId, int id);

    Task<bool> IsArtistExistsAsync(int id);
    Task<bool> ExistsByNameAsync(string name);
}