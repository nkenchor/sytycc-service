using System.ComponentModel.DataAnnotations;

namespace Sytycc_Service.Domain;
public class RegistrationReferenceDto
{ 
   [Required(ErrorMessage = "Registration reference is required.")]
   [StringLength(36, MinimumLength = 36, ErrorMessage = "Registraion reference should be 36 characters (GUID format).")]
   public string RegistrationReference { get; set; }



}

