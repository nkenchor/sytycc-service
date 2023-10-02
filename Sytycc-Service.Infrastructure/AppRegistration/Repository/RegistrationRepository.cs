using Sytycc_Service.Domain;
using MongoDB.Driver;
using Serilog;

namespace Sytycc_Service.Infrastructure;
public class RegistrationRepository: IRegistrationRepository
{ 
  
   
    readonly IDBProvider _dbProvider;
    readonly IMongoCollection<Registration> _registration; 
    readonly IMongoCollection<Course> _course;
    readonly IMongoCollection<Participant> _participant;
    readonly IMongoCollection<Facilitator> _facilitator;
    public RegistrationRepository(IDBProvider dbProvider)
    {
     
        _dbProvider = dbProvider;
       
        _registration =_dbProvider.Connect().GetCollection<Registration>(nameof(Registration).ToLower());
        _course =_dbProvider.Connect().GetCollection<Course>(nameof(Course).ToLower());
        _participant =_dbProvider.Connect().GetCollection<Participant>(nameof(Participant).ToLower());
        _facilitator =_dbProvider.Connect().GetCollection<Facilitator>(nameof(Facilitator).ToLower());
       
    }
    public async Task<string> CreateRegistration(Registration  registration)
    {
        try
        {
           
            Log.Information("Inserting Registration Data");
            await _registration.InsertOneAsync(registration);
            Log.Information("Data Inserted");
            return registration.Reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Creating Registration: {0}", e.Message );
             throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> UpdateRegistration(string reference, Registration registration)
    {
        try
        {
           
            Log.Information("Updating Data");
            var result = await _registration.ReplaceOneAsync(registration => registration.Reference == reference, registration);
            Log.Information("Data Updated");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Updating Registration: {0}", e.Message );
              throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> DeleteRegistration(string reference)
    {
        try
        {
         
            Log.Information("Deleting data");
            var result = await _registration.DeleteOneAsync(data => data.Reference == reference);
            Log.Information("Data Deleted");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Deleting Registration: {0}", e.Message );
              throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<Registration> GetRegistrationByReference(string reference)
    {
        try
        {
            
            Log.Information("Getting data by reference {0}", reference);
          
           return await _registration.Find(registration => registration.Reference == reference).FirstOrDefaultAsync();

     
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Registration: {0}", e.Message );
             throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<Registration> GetRegistrationByParticipantCourse(string courseReference, string participantReference)
    {
        try
        {
            Log.Information("Getting registration data for course reference {0} and participant reference {1}", courseReference, participantReference);

            // Create compound filter
            var filter = Builders<Registration>.Filter.And(
                Builders<Registration>.Filter.Eq(reg => reg.CourseReference, courseReference),
                Builders<Registration>.Filter.Eq(reg => reg.ParticipantReference, participantReference)
            );

            return await _registration.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Registration: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }

    public async Task<List<Registration>> GetRegistrationList(int page)
    {
        try
        {
            page = page <= 0 ? 1 : page;

            Log.Information("Getting data by page {0}", page);
            
            return await _registration.Find(registration => true).Skip((page-1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();
           
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Registration: {0}", e.Message );
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<List<Registration>> SearchRegistrationList(int page, string courseReference)
    {
        try
        {
            page = page <= 0 ? 1 : page;

            Log.Information("Searching data by page {0}", page);

            return await _registration.Find(registration => registration.CourseReference == courseReference).Skip((page-1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();

     
        }
        catch (Exception e)
        {
            Log.Error("Error Searching Registration: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }
}