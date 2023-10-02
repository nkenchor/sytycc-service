
using FluentValidation.Results;

namespace Sytycc_Service.Domain;
public interface IAppExceptionService
   {
      AppException GetValidationExceptionResult(ValidationResult validationResult);
      
   }