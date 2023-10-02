using Sytycc_Service.Domain;

namespace Sytycc_Service.Api;

public interface IEmailService
{
    Task<EmailResult> SendEmailNotificationToParticipant(RegistrationReferenceDto registrationReferenceDto);
    Task<EmailResult> SendEmailNotificationToAdmin(RegistrationReferenceDto registrationReferenceDto);
}

