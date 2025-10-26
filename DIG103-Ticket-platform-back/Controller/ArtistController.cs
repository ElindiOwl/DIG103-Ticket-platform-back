using DIG103_Ticket_platform_back.DTO.Artist;
using DIG103_Ticket_platform_back.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DIG103_Ticket_platform_back.Controller;

[ApiController]
[Route("api/artist")]

public class ArtistController(IArtistService artistService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ArtistDto>> CreateArtist(
        [FromForm] CreateArtistDto dto
        )
    {
        try
        {
            var result = await artistService.CreateArtistAsync(dto);
           
            return CreatedAtAction(
                nameof(GetArtistById),
                new { id = result.Id },
                result
            );
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ArtistDto>> GetArtistById(int id)
    {
        try
        {
            var result = await artistService.GetArtistByIdAsync(id);
            
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ArtistDto>> UpdateArtist(
        int id, 
        [FromForm] UpdateArtistDto dto
        )
    {
        try
        {
            var result = await artistService.UpdateArtistAsync(id, dto);
            
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteArtist(int id)
    {
        try
        {
            await artistService.DeleteArtistAsync(id);
            
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}