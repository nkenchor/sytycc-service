using Sytycc_Service.Domain;

namespace Sytycc_Service.Api;

public interface IRegistrationService
{

    Task<string> CreateRegistration(string paymentIntentId,CreateRegistrationDto registration);

    Task<string> DeleteRegistration(string reference);
    Task<RegistrationDto> GetRegistrationByReference(string reference);
    Task<List<RegistrationDto>> GetRegistrationList(int page);
    Task<List<RegistrationDto>> SearchRegistrationList(int page, string courseReference);

    Task<RegistrationDto> GetRegistrationByParticipantCourse(string courseReference, string participantReference);
    
}

