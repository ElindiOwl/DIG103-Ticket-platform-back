using DIG103_Ticket_platform_back.DTO.Artist;
using DIG103_Ticket_platform_back.Model;
using DIG103_Ticket_platform_back.Repository;

namespace DIG103_Ticket_platform_back.Service.Impl;

public class ArtistService(
    IArtistRepository artistRepository, 
    IImageService imageService, 
    IMinioService minioService
    ) : IArtistService
{
    public async Task<ArtistDto> CreateArtistAsync(CreateArtistDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new Exception("Name must be provided");
        }
        
        if (await artistRepository.ExistsByNameAsync(dto.Name))
        {
            throw new Exception("Artist already exists");
        }
        
        var artist = new Artist
        {
            Name = dto.Name,
            Description = dto.Description,
        };

        artist = await artistRepository.CreateAsync(artist);

        if (dto.Image != null)
        {
            var image = await imageService.UploadAsync(dto.Image, $"artists/{artist.Id}");
            artist.ArtistImage = image;
            
            await artistRepository.UpdateAsync(artist);
        }

        artist = await artistRepository.GetByIdWithRelationsAsync(artist.Id);
        return MapToDto(artist!);
    }

    public async Task<List<ArtistDto>> GetAllArtistsAsync()
    {
        var artists = await artistRepository.GetAllWithRelationsAsync();

        if (!artists.Any())
        {
            throw new KeyNotFoundException("No artists currently present");
        }

        return artists.Select(MapToDto).ToList();
    }

    public async Task<ArtistDto> UpdateArtistAsync(int id, UpdateArtistDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new Exception("Name must be provided");
        }
        
        var artist = await artistRepository.GetByIdAsync(id);
        
        if (artist == null)
        {
            throw new KeyNotFoundException("Artist not found");
        }

        artist.Name = dto.Name;
        artist.Description = dto.Description;

        if (dto.Image != null)
        {
            if (artist.ArtistImage != null)
            {
                await imageService.DeleteAsync(artist.ArtistImage.Id);
            }
            
            var image = await imageService.UploadAsync(dto.Image, $"artists/{artist.Id}");
            artist.ArtistImage = image;
        }
        
        await artistRepository.UpdateAsync(artist);
        artist = await artistRepository.GetByIdWithRelationsAsync(id);
        return MapToDto(artist!);
    }

    public async Task<ArtistDto> GetArtistByIdAsync(int id)
    {
        var artist = await artistRepository.GetByIdWithRelationsAsync(id);
        
        if (artist == null)
        {
            throw new KeyNotFoundException("Artist not found");
        }

        return MapToDto(artist);
    }

    public async Task DeleteArtistAsync(int id)
    {
        var artist = await artistRepository.GetByIdAsync(id);
        
        if (artist == null)
        {
            throw new KeyNotFoundException("Artist not found");
        }
        
        if (artist.ArtistImage != null)
        {
            await imageService.DeleteAsync(artist.ArtistImage.Id);
        }
        
        await artistRepository.DeleteAsync(artist);
    }

    private ArtistDto MapToDto(Artist artist)
    {
        return new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            Description = artist.Description,
            ImageUrl = artist.ArtistImage != null 
                ? minioService.GetPublicUrl(artist.ArtistImage.ImagePath)
                : null,
            EventIds = artist.Events.Select(e => e.Id).ToList()
        };
    }
}