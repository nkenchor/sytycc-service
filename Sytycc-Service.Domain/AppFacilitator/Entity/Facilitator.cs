
using MongoDB.Bson.Serialization.Attributes;


namespace Sytycc_Service.Domain;
public class Facilitator
{ 
   [BsonId]
   public string Reference { get; set; }
   public string FullName {get;internal set;}
   public string FirstName { get; set; }
   public string LastName{get;set;}
   public string Bio{get;set;}
   public string Email{get;set;}
   public string Phone{get;set;}


   
   
   public Facilitator(CreateFacilitatorDto createFacilitatorDto)
   {
      Reference = Guid.NewGuid().ToString();
      FirstName = createFacilitatorDto.FirstName;
      LastName = createFacilitatorDto.LastName;
      FullName = $"{LastName}, {FirstName}";
      Bio = createFacilitatorDto.Bio;
      Email = createFacilitatorDto.Email;
      Phone = createFacilitatorDto.Phone;
 
   }
   public Facilitator(UpdateFacilitatorDto updateFacilitatorDto)
   {
      LastName = updateFacilitatorDto.LastName;
      FirstName = updateFacilitatorDto.FirstName;
      FullName = $"{LastName}, {FirstName}";
      Bio = updateFacilitatorDto.Bio;
      Email = updateFacilitatorDto.Email;
      Phone = updateFacilitatorDto.Phone;

   }
   public Facilitator()
   {
      
   }

   
}