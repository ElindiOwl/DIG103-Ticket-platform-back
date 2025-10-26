using DIG103_Ticket_platform_back.DTO.Event;

namespace DIG103_Ticket_platform_back.Service;

public interface IEventService
{
    Task<EventDto> CreateEventAsync(CreateEventDto dto);
    Task<List<EventDto>> GetFeaturedEventsAsync();
    Task<EventDto> GetEventByIdAsync(int id);
    Task<EventDto> UpdateEventAsync(int id, UpdateEventDto dto);
    Task DeleteEventAsync(int id);
    Task<EventDto> CreateEventFeatureAsync(int eventId, CreateEventFeatureDto dto);
    Task<EventDto> UpdateEventFeatureAsync(int eventId, int id, UpdateEventFeatureDto dto);
    Task<EventDto> DeleteEventFeatureAsync(int eventId, int id);
    Task<EventDto> AddEventArtistAsync(int eventId, int id);
    Task<EventDto> RemoveEventArtistAsync(int eventId, int id);
}