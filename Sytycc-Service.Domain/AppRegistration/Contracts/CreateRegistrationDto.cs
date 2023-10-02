using System.ComponentModel.DataAnnotations;

namespace Sytycc_Service.Domain;
public class CreateRegistrationDto
{ 
   [Required(ErrorMessage = "Course reference is required.")]
   [StringLength(36, MinimumLength = 36, ErrorMessage = "Course reference should be 36 characters (GUID format).")]
   public string CourseReference { get; set; }


   [Required(ErrorMessage = "Participant reference is required.")]
   [StringLength(36, MinimumLength = 36, ErrorMessage = "Participant reference should be 36 characters (GUID format).")]
   public string ParticipantReference { get; set; }

   [Required(ErrorMessage = "Amount paid is required.")]
   [Range(0, 10000, ErrorMessage = "Amount paid should be between 0 and 10,000.")]
   public int AmountPaid { get; set; }

}

