namespace DIG103_Ticket_platform_back.DTO.Artist;

public class UpdateArtistDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile? Image { get; set; }
}