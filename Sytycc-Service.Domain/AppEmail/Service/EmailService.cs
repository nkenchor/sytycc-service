
namespace Sytycc_Service.Domain;

public class EmailValidationService:IEmailValidationService
{
      
      public AppException ValidateEmail(EmailDto emailDto)
      {
        return new ErrorService().GetValidationExceptionResult(new EmailValidator().Validate(emailDto));
      }   
     
}