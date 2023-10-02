namespace Sytycc_Service.Domain;
public interface IFacilitatorRepository{

 
    Task<string> CreateFacilitator(Facilitator facilitator);
    Task<string> UpdateFacilitator(string reference, Facilitator facilitator);
    Task<string> DeleteFacilitator(string reference);
    Task<Facilitator> GetFacilitatorByReference(string reference);
    Task<List<Facilitator>> GetFacilitatorList(int page);
    Task<List<Facilitator>> SearchFacilitatorList(int page, string fullname);
    Task<Facilitator> GetFacilitatorByFullName(string fullname); //
    
  
}

