namespace DIG103_Ticket_platform_back.DTO.Event;

public class CreateEventDto
{
    public string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Location { get; set; }
    public string Description { get; set; }
    public IFormFile? Image { get; set; }
    public IFormFile? Background { get; set; }
    public bool IsFeatured { get; set; }
    public EventThemeDto? Theme { get; set; }
}