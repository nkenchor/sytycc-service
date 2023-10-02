namespace Sytycc_Service.Domain;

public interface IParticipantRepository{

    #region Database
    Task<string> CreateParticipant(Participant participant);
    Task<string> UpdateParticipant(string reference, Participant participant);
    Task<string> DeleteParticipant(string reference);
    Task<Participant> GetParticipantByReference(string reference);
    Task<List<Participant>> GetParticipantList(int page);
    Task<List<Participant>> SearchParticipantList(int page, string title);
    Task<Participant> GetParticipantByEmail(string email);
    #endregion
}

