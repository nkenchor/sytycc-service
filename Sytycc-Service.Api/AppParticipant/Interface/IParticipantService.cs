
using Sytycc_Service.Domain;

namespace Sytycc_Service.Api;

public interface IParticipantService{
      

    Task<string> CreateParticipant(CreateParticipantDto participant);
    Task<string> UpdateParticipant(string reference, UpdateParticipantDto participant);
    Task<string> DeleteParticipant(string reference);
    Task<ParticipantDto> GetParticipantByReference(string reference);
    Task<List<ParticipantDto>> GetParticipantList(int page);
    Task<List<ParticipantDto>> SearchParticipantList(int page, string title);
    Task<ParticipantDto> GetParticipantByEmail(string email);
      
}

