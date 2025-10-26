namespace DIG103_Ticket_platform_back.Model;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Image? ArtistImage { get; set; }
    public ICollection<Event> Events { get; set; } = new List<Event>();
}