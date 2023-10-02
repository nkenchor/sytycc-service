
using System.ComponentModel.DataAnnotations;

namespace Sytycc_Service.Domain;
public class UpdateRegistrationDto
{ 
 
        [Required(ErrorMessage = "Course reference is required.")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "Course reference should be 36 characters (GUID format).")]
        public string CourseReference { get; set; }

        [Required(ErrorMessage = "Participant reference is required.")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "Participant reference should be 36 characters (GUID format).")]
        public string ParticipantReference { get; set; }

        

}