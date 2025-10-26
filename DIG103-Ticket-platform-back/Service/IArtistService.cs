using DIG103_Ticket_platform_back.DTO.Artist;

namespace DIG103_Ticket_platform_back.Service;

public interface IArtistService
{
    Task<ArtistDto> CreateArtistAsync(CreateArtistDto dto);
    Task<ArtistDto> UpdateArtistAsync(int id, UpdateArtistDto dto);
    Task<ArtistDto> GetArtistByIdAsync(int id);
    Task DeleteArtistAsync(int id);
}