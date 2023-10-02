
using System.ComponentModel.DataAnnotations;



namespace Sytycc_Service.Domain;
public class UpdateParticipantDto
{ 
         [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "First name should be between 1 and 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Last name should be between 1 and 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Bio is required.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Bio should be between 10 and 1000 characters.")]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(200, ErrorMessage = "Email should not exceed 200 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number should not exceed 20 characters.")]
        public string Phone { get; set; }
}