
namespace Sytycc_Service.Domain;

public class ParticipantValidationService:IParticipantValidationService
{
      
      public AppException ValidateCreateParticipant(CreateParticipantDto createParticipantDto)
      {
        return new ErrorService().GetValidationExceptionResult(new CreateParticipantValidator().Validate(createParticipantDto));
      }   
      public AppException ValidateUpdateParticipant(UpdateParticipantDto updateParticipantDto)
      {
        return new ErrorService().GetValidationExceptionResult(new UpdateParticipantValidator().Validate(updateParticipantDto));
      }   
}