namespace Sytycc_Service.Domain;

public interface IRegistrationRepository{

    #region Database
    Task<string> CreateRegistration(Registration registration);
    Task<string> UpdateRegistration(string reference, Registration registration );
    Task<string> DeleteRegistration(string reference);
    Task<Registration> GetRegistrationByReference(string reference);
    Task<List<Registration>> GetRegistrationList(int page);
    Task<List<Registration>> SearchRegistrationList(int page, string title);
    Task<Registration> GetRegistrationByParticipantCourse(string courseReference, string participantReference);
    
    #endregion
}

