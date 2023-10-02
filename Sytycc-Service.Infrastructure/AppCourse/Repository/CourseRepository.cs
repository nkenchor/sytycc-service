using Sytycc_Service.Domain;
using MongoDB.Driver;
using Serilog;
using MongoDB.Bson;
using System.Globalization;

namespace Sytycc_Service.Infrastructure;
public class CourseRepository: ICourseRepository
{ 
   
   
    readonly IDBProvider _dbProvider;
    readonly IMongoCollection<Course> _course; 
    readonly IMongoCollection<Facilitator> _facilitator;
    public CourseRepository(IDBProvider dbProvider)
    {
        _dbProvider = dbProvider;
        _course =_dbProvider.Connect().GetCollection<Course>(nameof(Course).ToLower());
        _facilitator =_dbProvider.Connect().GetCollection<Facilitator>(nameof(Facilitator).ToLower());
        
    }
    public async Task<string> CreateCourse(Course  course)
    {
        try
        {
            
            Log.Information("Inserting Course Data");
            await _course.InsertOneAsync(course);
            Log.Information("Data Inserted");
            return course.Reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Creating Course: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> UpdateCourse(string reference, Course course)
    {
        try
        {
            
            Log.Information("Updating Data");
            var result = await _course.ReplaceOneAsync(course => course.Reference == reference, course);
            Log.Information("Data Updated");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Updating Course: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<string> DeleteCourse(string reference)
    {
        try
        {
            
            Log.Information("Deleting data");
            var result = await _course.DeleteOneAsync(data => data.Reference == reference);
            Log.Information("Data Deleted");
            return reference;
        }
        catch (Exception e)
        {
            Log.Error("Error Deleting Course: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<Course> GetCourseByReference(string reference)
    {
        try
        {
            Log.Information("Getting data by reference {0}", reference);
          
           return await _course.Find(course => course.Reference == reference).FirstOrDefaultAsync();
        
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Course: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
  
    public async Task<List<Course>> GetCourseList(int page)
    {
        try
        {
            page = page <= 0 ? 1 : page;

            Log.Information("Getting data by page {0}", page);
            
            return await _course.Find(course => true).Skip((page-1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Course: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
    public async Task<List<Course>> SearchCourseList(int page, string title)
    {
        try
        { 
            Log.Information("Searching data by page {0}", page);
            
         

            var filterBuilder = Builders<Course>.Filter;
            var filter = filterBuilder.Regex(course => course.Title, new BsonRegularExpression($"/{title}/"));
            
            
            return await _course.Find(filter).Skip((page-1) * _dbProvider.GetPageLimit())
            .Limit(_dbProvider.GetPageLimit()).ToListAsync();

        }
        catch (Exception e)
        {
            Log.Error("Error Searching Course: {0}", e.Message );
         throw DatabaseExceptionHandler.HandleException(e);
        }
    }
   public async Task<List<Course>> GetUpcomingCourses(int page)
    {
        try
        {
            page = page <= 0 ? 1 : page;

            Log.Information("Getting upcoming courses by page {0}", page);

            var currentDate = DateTime.UtcNow; 

            var allCourses = await _course.Find(_ => true).ToListAsync();

            var filteredCourses = allCourses.Where(course => DateTime.ParseExact(course.StartDate, "dd-MM-yyyy", CultureInfo.InvariantCulture) > currentDate)
                                .Skip((page-1) * _dbProvider.GetPageLimit())
                                .Take(_dbProvider.GetPageLimit())
                                .ToList();

            return filteredCourses;
        }
        catch (Exception e)
        {
            Log.Error("Error Getting Upcoming Courses: {0}", e.Message);
            throw DatabaseExceptionHandler.HandleException(e);
        }
    }



    public async Task<Course> GetCourseByTitle(string title)
    {
        try
        {
            Log.Information("Searching course by title: {0}", title);

            var filter = Builders<Course>.Filter.Eq(course => course.Title, title);
            
            return await _course.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            Log.Error("Error retrieving course by title: {0}", e.Message);
           throw DatabaseExceptionHandler.HandleException(e);
        }
    }
}