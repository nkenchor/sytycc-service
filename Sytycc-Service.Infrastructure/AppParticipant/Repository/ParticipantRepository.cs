using Sytycc_Service.Domain;
using MongoDB.Driver;
using Serilog;
using MongoDB.Bson;


namespace Sytycc_Service.Infrastructure;
public class ParticipantRepository: IParticipantRepository
{ 
  
   
    readonly IDBProvider _dbProvider;
    readonly IMongoCollection<Participant> _participant; 
  

    public ParticipantRepository(IDBProvider dbProvider)
    {
     
        _dbProvider = dbProvider;
      
        _participant =_dbProvider.Connect().GetCollection<Participant>(nameof(Participant).ToLower());
      
    }
    public async Task<string> CreateParticipant(Participant  participant)
    {
        try
        {
           
            Log.Information("Inserting Participant Data");
            await _participant.InsertOneAsync(participant);
            Log.Information("Data Inserted");
            return participant.Reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Creating Participant: {0}", e.Message );
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> UpdateParticipant(string reference, Participant participant)
    {
        try
        {
            
            Log.Information("Updating Data");
            var result = await _participant.ReplaceOneAsync(participant => participant.Reference == reference, participant);
            Log.Information("Data Updated");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Updating Participant: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> DeleteParticipant(string reference)
    {
        try
        {
           
            Log.Information("Deleting data");
            await _participant.DeleteOneAsync(data => data.Reference == reference);
            Log.Information("Data Deleted");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Deleting Participant: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<Participant> GetParticipantByReference(string reference)
    {
        try
        {
            Log.Information("Getting data by reference {0}", reference);
          
            return await _participant.Find(participant => participant.Reference == reference).FirstOrDefaultAsync();
          
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Participant: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
     public async Task<Participant> GetParticipantByEmail(string email)
    {
        try
        {
            Log.Information("Getting data by email {0}", email);
          
            return await _participant.Find(participant => participant.Email == email).FirstOrDefaultAsync();
          
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Participant: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
  
    public async Task<List<Participant>> GetParticipantList(int page)
    {
        try
        {
            Log.Information("Getting data by page {0}", page);
            
            return await _participant.Find(participant => true).Skip((page-1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();
      
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Participant: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<List<Participant>> SearchParticipantList(int page, string fullname)
    {
        try
        {
            Log.Information("Searching data by page {0}", page);

            var filterBuilder = Builders<Participant>.Filter;
            var fullnameFilter = filterBuilder.Regex(participant => participant.FullName, new BsonRegularExpression($"/{fullname}/"));
            
            var filter = fullnameFilter;

            return await _participant.Find(filter).Skip((page-1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();
         
  
    
        }
        catch (Exception e)
        {
            Log.Error("Error Searching Participant: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
}