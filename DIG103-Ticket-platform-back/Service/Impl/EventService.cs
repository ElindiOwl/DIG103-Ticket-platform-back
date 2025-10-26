using DIG103_Ticket_platform_back.DTO.Event;
using DIG103_Ticket_platform_back.Model;
using DIG103_Ticket_platform_back.Repository;

namespace DIG103_Ticket_platform_back.Service.Impl;

public class EventService(
    IEventRepository eventRepository,
    IImageService imageService,
    IMinioService minioService
    ) : IEventService
{
    public async Task<EventDto> CreateEventAsync(CreateEventDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new Exception("Name must be provided");
        }

        if (await eventRepository.ExistsByNameAsync(dto.Name))
        {
            throw new Exception("Event already exists");
        }

        var newEvent = new Event
        {
            Name = dto.Name,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Location = dto.Location,
            Description = dto.Description,
            IsFeatured = dto.IsFeatured,
            Theme = dto.Theme != null
            ? new EventTheme
            {
                ColorPrimary = dto.Theme.ColorPrimary,
                ColorPrimaryLight = dto.Theme.ColorPrimaryLight,
                ColorSecondary = dto.Theme.ColorSecondary,
                FontFamily = dto.Theme.FontFamily,
            }
            : null
        };

        newEvent = await eventRepository.CreateAsync(newEvent);

        if (dto.Image != null)
        {
            var image = await imageService.UploadAsync(dto.Image, $"events/{newEvent.Id}/image");
            newEvent.EventImage = image;
        }

        if (dto.Background != null)
        {
            var background = await imageService.UploadAsync(dto.Background, $"events/{newEvent.Id}/background");
            newEvent.EventBackground = background;
        }

        if (dto.Image != null || dto.Background != null)
        {
            await eventRepository.UpdateAsync(newEvent);
        }

        newEvent = await eventRepository.GetWithRelationsAsync(newEvent.Id);
        return MapToDto(newEvent!);
    }

    public async Task<List<EventDto>> GetFeaturedEventsAsync()
    {
        var featured = await eventRepository.GetFeaturedAsync();

        if (!featured.Any())
        {
            throw new KeyNotFoundException("No featured events available");
        }

        return featured.Select(MapToDto).ToList();
    }

    public async Task<EventDto> GetEventByIdAsync(int id)
    {
        var eventData = await eventRepository.GetWithRelationsAsync(id);

        if (eventData == null)
        {
            throw new KeyNotFoundException("Event not found");
        }

        return MapToDto(eventData);
    }

    public async Task<EventDto> UpdateEventAsync(int id, UpdateEventDto dto)
    {
        var eventData = await eventRepository.GetWithRelationsAsync(id);

        if (eventData == null)
        {
            throw new KeyNotFoundException("Event not found");
        }

        eventData.Name = dto.Name;
        eventData.StartDate = dto.StartDate;
        eventData.EndDate = dto.EndDate;
        eventData.Location = dto.Location;
        eventData.Description = dto.Description;
        eventData.IsFeatured = dto.IsFeatured;

        if (dto.Theme != null)
        {
            if (eventData.Theme == null)
            {
                eventData.Theme = new EventTheme();
            }
            
            eventData.Theme.ColorPrimary = dto.Theme.ColorPrimary;
            eventData.Theme.ColorPrimaryLight = dto.Theme.ColorPrimaryLight;
            eventData.Theme.ColorSecondary = dto.Theme.ColorSecondary;
            eventData.Theme.FontFamily = dto.Theme.FontFamily;
        }

        if (dto.Image != null)
        {
            if (eventData.EventImage != null)
            {
                await imageService.DeleteAsync(eventData.EventImage.Id);
            }
            
            var image = await imageService.UploadAsync(dto.Image, $"events/{id}/image");
            eventData.EventImage = image;
        }

        if (dto.Background != null)
        {
            if (eventData.EventBackground != null)
            {
                await imageService.DeleteAsync(eventData.EventBackground.Id);
            }

            var image = await imageService.UploadAsync(dto.Background, $"events/{id}/background");
            eventData.EventBackground = image;
        }

        await eventRepository.UpdateAsync(eventData);
        
        eventData = await eventRepository.GetWithRelationsAsync(id);
        return MapToDto(eventData!);
    }

    public async Task DeleteEventAsync(int id)
    {
        var eventData = await eventRepository.GetWithRelationsAsync(id);

        if (eventData == null)
        {
            throw new KeyNotFoundException("Event not found");
        }

        if (eventData.EventImage != null)
        {
            await imageService.DeleteAsync(eventData.EventImage.Id);
        }

        if (eventData.EventBackground != null)
        {
            await imageService.DeleteAsync(eventData.EventBackground.Id);
        }

        foreach (var feature in eventData.Features)
        {
            await eventRepository.DeleteFeatureAsync(id, feature.Id);
        }

        await eventRepository.DeleteAsync(eventData);
    }

    public async Task<EventDto> CreateEventFeatureAsync(int eventId, CreateEventFeatureDto dto)
    {
        var eventData = await eventRepository.GetWithRelationsAsync(eventId);

        if (eventData == null)
        {
            throw new KeyNotFoundException("Event not found");
        }

        var newEventFeature = new EventFeature
        {
            Description = dto.Description,
            EventId = eventId
        };

        if (dto.Image != null)
        {
            var image = await imageService.UploadAsync(dto.Image, $"events/{eventId}/features");
            newEventFeature.FeatureImage = image;
        }

        await eventRepository.CreateFeatureAsync(eventId, newEventFeature);

        eventData = await eventRepository.GetWithRelationsAsync(eventId);
        return MapToDto(eventData!);
    }

    public async Task<EventDto> UpdateEventFeatureAsync(int eventId, int id, UpdateEventFeatureDto dto)
    {
        var eventData = await eventRepository.GetWithRelationsAsync(eventId);

        if (eventData == null)
        {
            throw new KeyNotFoundException("Event not found");
        }

        var feature = eventData.Features.FirstOrDefault(f => f.Id == id);

        if (feature == null)
        {
            throw new KeyNotFoundException("Feature not found");
        }

        feature.Description = dto.Description;

        if (dto.Image != null)
        {
            if (feature.FeatureImage != null)
            {
                await imageService.DeleteAsync(feature.FeatureImage.Id);
            }

            var image = await imageService.UploadAsync(dto.Image, $"events/{eventId}/features");
            feature.FeatureImage = image;
        }

        await eventRepository.UpdateFeatureAsync(feature);

        eventData = await eventRepository.GetWithRelationsAsync(eventId);
        return MapToDto(eventData!);
    }

    public async Task<EventDto> DeleteEventFeatureAsync(int eventId, int id)
    {
        var eventData = await eventRepository.GetWithRelationsAsync(eventId);

        if (eventData == null)
        {
            throw new KeyNotFoundException("Event not found");
        }

        var feature = eventData.Features.FirstOrDefault(f => f.Id == id);

        if (feature == null)
        {
            throw new KeyNotFoundException("Feature not found");
        }

        if (feature.FeatureImage != null)
        {
            await imageService.DeleteAsync(feature.FeatureImage.Id);
        }

        await eventRepository.DeleteFeatureAsync(eventId, id);

        eventData = await eventRepository.GetWithRelationsAsync(eventId);
        return MapToDto(eventData!);
    }

    public async Task<EventDto> AddEventArtistAsync(int eventId, int id)
    {
        var eventData = await eventRepository.GetWithRelationsAsync(eventId);

        if (eventData == null)
        {
            throw new KeyNotFoundException("Event not found");
        }

        if (eventData.Artists.Any(a => a.Id == id))
        {
            throw new Exception("This artist is already added");
        }

        var artistExists = await eventRepository.IsArtistExistsAsync(id);

        if (!artistExists)
        {
            throw new KeyNotFoundException("Artist not found");
        }

        await eventRepository.AddArtistAsync(eventId, id);

        eventData = await eventRepository.GetWithRelationsAsync(eventId);
        return MapToDto(eventData!);
    }

    public async Task<EventDto> RemoveEventArtistAsync(int eventId, int id)
    {
        var eventData = await eventRepository.GetWithRelationsAsync(eventId);

        if (eventData == null)
        {
            throw new KeyNotFoundException("Event not found");
        }

        if (!eventData.Artists.Any(a => a.Id == id))
        {
            throw new Exception("Artist is not assigned to this event");
        }

        await eventRepository.RemoveArtistAsync(eventId, id);

        eventData = await eventRepository.GetWithRelationsAsync(eventId);
        return MapToDto(eventData!);
    }
    
    private EventDto MapToDto(Event eventInfo)
    {
        return new EventDto
        {
            Id = eventInfo.Id,
            Name = eventInfo.Name,
            StartDate = eventInfo.StartDate,
            EndDate = eventInfo.EndDate,
            Location = eventInfo.Location,
            Description = eventInfo.Description,
            ImageUrl = eventInfo.EventImage != null
                ? minioService.GetPublicUrl(eventInfo.EventImage.ImagePath)
                : null,
            BackgroundUrl = eventInfo.EventBackground != null
                ? minioService.GetPublicUrl(eventInfo.EventBackground.ImagePath)
                : null,
            IsFeatured = eventInfo.IsFeatured,
            Theme = eventInfo.Theme != null
                ? new EventThemeDto
                {
                    ColorPrimary = eventInfo.Theme.ColorPrimary,
                    ColorPrimaryLight = eventInfo.Theme.ColorPrimaryLight,
                    ColorSecondary = eventInfo.Theme.ColorSecondary,
                    FontFamily = eventInfo.Theme.FontFamily,
                }
                : null,
            Features = eventInfo.Features.Select(feature => new EventFeatureDto
            {
                Id = feature.Id,
                Description = feature.Description,
                ImageUrl = feature.FeatureImage != null
                    ? minioService.GetPublicUrl(feature.FeatureImage.ImagePath)
                    : null
            }).ToList(),
            ArtistIds = eventInfo.Artists.Select(artist => 
                artist.Id
            ).ToList()
        };
    }
}