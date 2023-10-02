

namespace Sytycc_Service.Domain;
public class RegistrationDto
{ 
 
   public Registration Registration{get;set;}
   public Course Course{get;set;}
   public Facilitator Facilitator{get;set;}
   public Participant Participant{get;set;}
   
   public RegistrationDto(Registration registration,Course course,Facilitator facilitator, Participant participant)
   {
      Registration = registration;
      Course = course;
      Facilitator = facilitator;
      Participant = participant;
   }
   
}