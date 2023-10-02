
namespace Sytycc_Service.Domain;

public interface IFacilitatorValidationService{
      
      AppException ValidateCreateFacilitator(CreateFacilitatorDto createFacilitatorDto);
      AppException ValidateUpdateFacilitator(UpdateFacilitatorDto updateFacilitatorDto);
      
}

