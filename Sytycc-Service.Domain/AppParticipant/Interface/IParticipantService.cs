

namespace Sytycc_Service.Domain;

public interface IParticipantValidationService{
      
      AppException ValidateCreateParticipant(CreateParticipantDto createParticipantDto);
      AppException ValidateUpdateParticipant(UpdateParticipantDto updateParticipantDto);
      
}

