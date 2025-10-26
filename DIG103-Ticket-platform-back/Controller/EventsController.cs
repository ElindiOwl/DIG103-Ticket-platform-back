using DIG103_Ticket_platform_back.DTO.Event;
using DIG103_Ticket_platform_back.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DIG103_Ticket_platform_back.Controller;

[ApiController]
[Route("api/events")]

public class EventsController(IEventService eventService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent(
        [FromForm] CreateEventDto dto
            )
    {
        try
        {
            var result = await eventService.CreateEventAsync(dto);

            return CreatedAtAction(
                nameof(GetEventById),
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
    [HttpGet("featured-events")]
    public async Task<ActionResult<List<EventDto>>> GetFeaturedEvents()
    {
        try
        {
            var result = await eventService.GetFeaturedEventsAsync();

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventDto>> GetEventById(int id)
    {
        try
        {
            var result = await eventService.GetEventByIdAsync(id);
                
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<EventDto>> UpdateEvent(
        int id,
        [FromForm] UpdateEventDto dto
        )
    {
        try
        {
            var result = await eventService.UpdateEventAsync(id, dto);

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
    public async Task<ActionResult> DeleteEvent(int id)
    {
        try
        {
            await eventService.DeleteEventAsync(id);

            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpPost("{eventId:int}/features")]
    public async Task<ActionResult<EventDto>> CreateEventFeature(
        int eventId,
        [FromForm] CreateEventFeatureDto dto
        )
    {
        try
        {
            var result = await eventService.CreateEventFeatureAsync(eventId, dto);

            return StatusCode(StatusCodes.Status201Created, result);
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
    [HttpPut("{eventId:int}/features/{id:int}")]
    public async Task<ActionResult<EventDto>> UpdateEventFeature(
        int eventId, 
        int id, 
        [FromForm] UpdateEventFeatureDto dto)
    {
        try
        {
            var result = await eventService.UpdateEventFeatureAsync(eventId, id, dto);

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
    [HttpDelete("{eventId:int}/features/{id:int}")]
    public async Task<ActionResult<EventDto>> DeleteFeature(int eventId, int id)
    {
        try
        {
            var result = await eventService.DeleteEventFeatureAsync(eventId, id);

            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpPost("{eventId:int}/artists/{id:int}")]
    public async Task<ActionResult<EventDto>> AddArtist(int eventId, int id)
    {
        try
        {
            var result = await eventService.AddEventArtistAsync(eventId, id);

            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpDelete("{eventId:int}/artists/{id:int}")]
    public async Task<ActionResult<EventDto>> RemoveArtist(int eventId, int id)
    {
        try
        {
            var result = await eventService.RemoveEventArtistAsync(eventId, id);

            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}