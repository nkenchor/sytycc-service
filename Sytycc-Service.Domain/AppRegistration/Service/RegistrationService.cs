
namespace Sytycc_Service.Domain;

public class RegistrationValidationService:IRegistrationValidationService
{
      
      public AppException ValidateCreateRegistration(CreateRegistrationDto createRegistrationDto)
      {
        return new ErrorService().GetValidationExceptionResult(new CreateRegistrationValidator().Validate(createRegistrationDto));
      }   
      public AppException ValidateUpdateRegistration(UpdateRegistrationDto updateRegistrationDto)
      {
        return new ErrorService().GetValidationExceptionResult(new UpdateRegistrationValidator().Validate(updateRegistrationDto));
      }   
}