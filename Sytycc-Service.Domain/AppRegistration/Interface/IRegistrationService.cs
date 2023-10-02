

namespace Sytycc_Service.Domain;

public interface IRegistrationValidationService{
      
      AppException ValidateCreateRegistration(CreateRegistrationDto createRegistrationDto);
      AppException ValidateUpdateRegistration(UpdateRegistrationDto updateRegistrationDto);
      
}

