namespace DIG103_Ticket_platform_back.Model;

public class EventFeature
{
    public int Id { get; set; }
    public string Description { get; set; }
    public Image? FeatureImage { get; set; }
    
    public int EventId { get; set; }
    public Event Event { get; set; }
}