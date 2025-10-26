namespace DIG103_Ticket_platform_back.DTO.Event;

public class EventDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Location { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? BackgroundUrl { get; set; }
    public bool IsFeatured { get; set; }
    public EventThemeDto? Theme { get; set; }
    public List<EventFeatureDto>? Features { get; set; } = [];
    public List<int>? ArtistIds { get; set; } = [];
}