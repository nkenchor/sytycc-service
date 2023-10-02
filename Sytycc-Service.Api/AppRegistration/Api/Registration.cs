using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sytycc_Service.Domain;


namespace Sytycc_Service.Api;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly IEmailService _emailService;

    public RegistrationController(IRegistrationService registrationService, IEmailService emailService)
    {
        _registrationService = registrationService;
        _emailService = emailService;
    }

    [HttpPost("{paymentIntentId}")]
    public async Task<IActionResult> CreateRegistration(string paymentIntentId, [FromBody] CreateRegistrationDto registrationDto)
    {
        var combinedResult = new RegistrationWithEmailResultDto();

        try
        {
            var registrationResult = await _registrationService.CreateRegistration(paymentIntentId, registrationDto);

            combinedResult.RegistrationResult = registrationResult;

            // If registration was successful, send emails
            if (registrationResult != null)  // Assuming your CreateRegistration method returns null in case of failure
            {
                var emailForParticipant = await _emailService.SendEmailNotificationToParticipant(new RegistrationReferenceDto { RegistrationReference = registrationResult });
                combinedResult.ParticipantEmailResult = emailForParticipant;

              

                var emailForAdmin = await _emailService.SendEmailNotificationToAdmin(new RegistrationReferenceDto { RegistrationReference = registrationResult });
                combinedResult.AdminEmailResult = emailForAdmin;

                
            }

            return Ok(combinedResult);
        }
        catch (AppException e)
        {
          
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }



    [HttpDelete("{reference}")]
    public async Task<IActionResult> DeleteRegistration(string reference)
    {
        try
        {
            var result = await _registrationService.DeleteRegistration(reference);
            return Ok(new { reference = result });
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("{reference}")]
    public async Task<IActionResult> GetRegistrationByReference(string reference)
    {
        try
        {
            var result = await _registrationService.GetRegistrationByReference(reference);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }


    [HttpGet("course/{courseReference}/participant/{participantReference}")]
    public async Task<IActionResult> GetRegistrationByParticipantCourse(string courseReference, string participantReference)
    {
        try
        {
            var result = await _registrationService.GetRegistrationByParticipantCourse(courseReference,participantReference);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
    
    [HttpGet("list/{page:int:min(1)}")]
    [Authorize]
    public async Task<IActionResult> GetRegistrationList(int page = 1)
    {
        try
        {
            var result = await _registrationService.GetRegistrationList(page);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpGet("search/{page:int:min(1)}")]
    [Authorize]
    public async Task<IActionResult> SearchRegistrationList(int page = 1, [FromQuery]string courseReference = "")
    {
        try
        {
            var result = await _registrationService.SearchRegistrationList(page, courseReference);
            return Ok(result);
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
}

