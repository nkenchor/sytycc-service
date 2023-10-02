
using Sytycc_Service.Domain;

namespace Sytycc_Service.Api;

public interface IFacilitatorService{
      

    Task<string> CreateFacilitator(CreateFacilitatorDto facilitator);
    Task<string> UpdateFacilitator(string reference, UpdateFacilitatorDto facilitator);
    Task<string> DeleteFacilitator(string reference);
    Task<FacilitatorDto> GetFacilitatorByReference(string reference);
    Task<List<FacilitatorDto>> GetFacilitatorList(int page);
    Task<List<FacilitatorDto>> SearchFacilitatorList(int page, string title);
    Task<FacilitatorDto> GetFacilitatorByFullName(string fullName);
      
}

