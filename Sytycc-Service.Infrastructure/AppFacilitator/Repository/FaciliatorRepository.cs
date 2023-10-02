using Sytycc_Service.Domain;
using MongoDB.Driver;
using Serilog;
using MongoDB.Bson;


namespace Sytycc_Service.Infrastructure;
public class FacilitatorRepository: IFacilitatorRepository
{ 
   
   
    readonly IDBProvider _dbProvider;
    readonly IMongoCollection<Facilitator> _facilitator; 
  

    public FacilitatorRepository(IDBProvider dbProvider)
    {
     
        _dbProvider = dbProvider;
        _facilitator =_dbProvider.Connect().GetCollection<Facilitator>(nameof(Facilitator).ToLower());
        
    }
    public async Task<string> CreateFacilitator(Facilitator  facilitator)
    {
        try
        {
            Log.Information("Inserting Facilitator Data");
            await _facilitator.InsertOneAsync(facilitator);
            Log.Information("Data Inserted");
            return facilitator.Reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Creating Facilitator: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> UpdateFacilitator(string reference, Facilitator facilitator)
    {
        try
        {

            Log.Information("Updating Data");
            await _facilitator.ReplaceOneAsync(facilitator => facilitator.Reference == reference, facilitator);
            Log.Information("Data Updated");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Updating Facilitator: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> DeleteFacilitator(string reference)
    {
        try
        {
        
            Log.Information("Deleting data");
            var result = await _facilitator.DeleteOneAsync(data => data.Reference == reference);
            Log.Information("Data Deleted");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Deleting Facilitator: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<Facilitator> GetFacilitatorByReference(string reference)
    {
        try
        {
            Log.Information("Getting data by reference {0}", reference);
          
            var result = await _facilitator.Find(facilitator => facilitator.Reference == reference).FirstOrDefaultAsync();
            return result;
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Facilitator: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
  
    public async Task<List<Facilitator>> GetFacilitatorList(int page)
    {
        try
        {
            Log.Information("Getting data by page {0}", page);
            
            return await _facilitator.Find(facilitator => true).Skip((page-1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();
           
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Facilitator: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<List<Facilitator>> SearchFacilitatorList(int page, string fullname)
    {
        try
        {
            Log.Information("Searching data by page {0}", page);
            
         

            var filterBuilder = Builders<Facilitator>.Filter;
            var fullnameFilter = filterBuilder.Regex(facilitator => facilitator.FullName, new BsonRegularExpression($"/{fullname}/"));
            
            var filter = fullnameFilter;

            return await _facilitator.Find(filter).Skip((page-1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();
        
        }
        catch (Exception e)
        {
            Log.Error("Error Searching Facilitator: {0}", e.Message );
          throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    public async Task<Facilitator> GetFacilitatorByFullName(string fullname)
    {
        try
        {
            Log.Information("Searching facilitator by full name: {0}", fullname);

            var filter = Builders<Facilitator>.Filter.Eq(facilitator => facilitator.FullName, fullname);
            
            return await _facilitator.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            Log.Error("Error retrieving facilitator by full name: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

}

