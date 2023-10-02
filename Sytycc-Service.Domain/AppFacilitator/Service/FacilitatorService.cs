
namespace Sytycc_Service.Domain;

public class FacilitatorValidationService:IFacilitatorValidationService
{
      
      public AppException ValidateCreateFacilitator(CreateFacilitatorDto createFacilitatorDto)
      {
        return new ErrorService().GetValidationExceptionResult(new CreateFacilitatorValidator().Validate(createFacilitatorDto));
      }   
      public AppException ValidateUpdateFacilitator(UpdateFacilitatorDto updateFacilitatorDto)
      {
        return new ErrorService().GetValidationExceptionResult(new UpdateFacilitatorValidator().Validate(updateFacilitatorDto));
      }   
}