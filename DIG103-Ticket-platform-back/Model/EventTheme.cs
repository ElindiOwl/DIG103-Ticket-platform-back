namespace DIG103_Ticket_platform_back.Model;

public class EventTheme
{
    public int Id { get; set; }
    public string? ColorPrimary { get; set; }
    public string? ColorPrimaryLight { get; set; }
    public string? ColorSecondary { get; set; }
    public string? FontFamily { get; set; }
    
    public int EventId { get; set; }
    public Event Event { get; set; }
}