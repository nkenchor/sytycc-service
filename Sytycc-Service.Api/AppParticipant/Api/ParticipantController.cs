using Microsoft.AspNetCore.Mvc;
using Sytycc_Service.Domain;

namespace Sytycc_Service.Api;

[ApiController]
[Route("api/[controller]")]
public class ParticipantController : ControllerBase
{
    private readonly IParticipantService _participantService;

    public ParticipantController(IParticipantService participantService)
    {
        _participantService = participantService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateParticipant([FromBody] CreateParticipantDto participantDto)
    {
        try
        {
            var result = await _participantService.CreateParticipant(participantDto);
            return Ok(new { reference = result });
    
        }
            catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("{reference}")]
    public async Task<IActionResult> GetParticipantByReference(string reference)
    {
        try
        {
            var participant = await _participantService.GetParticipantByReference(reference);
            return Ok(participant);
        }
            catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetParticipantByEmail(string email)
    {
        try
        {
            var participant = await _participantService.GetParticipantByEmail(email);
            return Ok(participant);
        }
            catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("list/{page:int:min(1)}")]
    public async Task<IActionResult> GetParticipantList(int page = 1)
    {
        try
        {
            var participants = await _participantService.GetParticipantList(page);
            return Ok(participants);
        }
            catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("search/{page:int:min(1)}")]
    public async Task<IActionResult> SearchParticipantList(int page, [FromQuery] string title)
    {
        try
        {
            var participants = await _participantService.SearchParticipantList(page, title);
            return Ok(participants);
        }
            catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPut("{reference}")]
    public async Task<IActionResult> UpdateParticipant(string reference, [FromBody] UpdateParticipantDto participantDto)
    {
        try
        {
            var result = await _participantService.UpdateParticipant(reference, participantDto);
            return Ok(new { reference = result });
        }
            catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpDelete("{reference}")]
    public async Task<IActionResult> DeleteParticipant(string reference)
    {
        try
        {
            var result = await _participantService.DeleteParticipant(reference);
            return Ok(new { reference = result });
        }
            catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
}

