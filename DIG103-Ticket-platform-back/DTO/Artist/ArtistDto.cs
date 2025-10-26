namespace DIG103_Ticket_platform_back.DTO.Artist;

public class ArtistDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public List<int>? EventIds { get; set; } = [];
}