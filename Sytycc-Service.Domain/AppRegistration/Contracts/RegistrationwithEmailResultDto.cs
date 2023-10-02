using System.ComponentModel.DataAnnotations;

namespace Sytycc_Service.Domain;
public class RegistrationWithEmailResultDto
{
    public string RegistrationResult { get; set; }
    public EmailResult ParticipantEmailResult { get; set; }
    public EmailResult AdminEmailResult { get; set; }
}
