namespace DIG103_Ticket_platform_back.Model;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Location { get; set; }
    public string Description { get; set; }
    public Image? EventImage { get; set; }
    public Image? EventBackground { get; set; }
    public bool IsFeatured { get; set; }
    public EventTheme? Theme { get; set; }
    public ICollection<EventFeature> Features { get; set; } = new List<EventFeature>();
    public ICollection<Artist> Artists { get; set; } = new List<Artist>();
}