
using MongoDB.Bson.Serialization.Attributes;

namespace Sytycc_Service.Domain;

public class Registration
{
   [BsonId]
   public string Reference { get; set; }
   public string CourseReference { get; set; }
   public string ParticipantReference { get; set; }
   public int AmountPaid { get; set; } // Integer type
   public int Discount { get; set; } // Integer type
   public string PaymentIntentId { get; set; } // String type
   public string RegistrationTime { get; internal set; }

   public Registration(string paymentIntentId,CreateRegistrationDto createRegistrationDto, int amountPaid, int discount)
   {
      Reference = Guid.NewGuid().ToString();
      CourseReference = createRegistrationDto.CourseReference;
      ParticipantReference = createRegistrationDto.ParticipantReference;
      RegistrationTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      PaymentIntentId = paymentIntentId;
      AmountPaid = amountPaid;
      Discount = discount;
   }

   public Registration(string paymentIntentId,UpdateRegistrationDto updateRegistrationDto,int amountPaid, int discount)
   {
      CourseReference = updateRegistrationDto.CourseReference;
      ParticipantReference = updateRegistrationDto.ParticipantReference;
      RegistrationTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      PaymentIntentId = paymentIntentId;
      AmountPaid = amountPaid;
      Discount = discount;
   }
}

