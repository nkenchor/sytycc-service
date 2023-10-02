

namespace Sytycc_Service.Domain;

public interface IEmailValidationService{
      
      AppException ValidateEmail(EmailDto emailDto);

      
}

