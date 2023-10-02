using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Serilog;
using Sytycc_Service.Domain;

namespace Sytycc_Service.Api;

[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    // // POST api/email/participant
    // [HttpPost("participant")]
    // public async Task<IActionResult> SendEmailToParticipant([FromBody] RegistrationReferenceDto registrationReferenceDto)
    // {
    //     try
    //     {
    //         var result = await _emailService.SendEmailNotificationToParticipant(registrationReferenceDto);
    //         return Ok(result);
    //     }
    //     catch (AppException e)
    //     {
    //         return StatusCode(e.StatusCode, new AppExceptionResponse(e));
    //     }

    // }

    // // POST api/email/admin
    // [HttpPost("admin")]
    // public async Task<IActionResult> SendEmailToAdmin([FromBody] RegistrationReferenceDto registrationReferenceDto)
    // {
    //     try
    //     {
    //         var result = await _emailService.SendEmailNotificationToAdmin(registrationReferenceDto);
    //         return Ok(result);
    //     }
    //     catch (AppException e)
    //     {
    //         return StatusCode(e.StatusCode, new AppExceptionResponse(e));
    //     }
        
    // }
}

