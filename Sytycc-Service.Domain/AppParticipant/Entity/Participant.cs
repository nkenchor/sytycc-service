
using MongoDB.Bson.Serialization.Attributes;


namespace Sytycc_Service.Domain;
public class Participant
{ 
   [BsonId]
   public string Reference { get; set; }
   public string FirstName { get; set; }
   public string FullName {get;internal set;}
   public string LastName{get;set;}
   public string Bio{get;set;}
   public string Email{get;set;}
   public string Phone{get;set;}

   
   public Participant(CreateParticipantDto createParticipantDto)
   {
      Reference = Guid.NewGuid().ToString();
      FirstName = createParticipantDto.FirstName;
      LastName = createParticipantDto.LastName;
      FullName = $"{LastName}, {FirstName}";
      Bio = createParticipantDto.Bio;
      Email = createParticipantDto.Email;
      Phone = createParticipantDto.Phone;

   }
   public Participant(UpdateParticipantDto updateParticipantDto)
   {
   
      FirstName = updateParticipantDto.FirstName;
      LastName = updateParticipantDto.LastName;
      FullName = $"{LastName}, {FirstName}";
      Bio = updateParticipantDto.Bio;
      Email = updateParticipantDto.Email;
      Phone = updateParticipantDto.Phone;

   }
}