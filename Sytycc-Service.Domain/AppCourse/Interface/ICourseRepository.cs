namespace Sytycc_Service.Domain;

public interface ICourseRepository{

    #region Database
    Task<string> CreateCourse(Course course);
    Task<string> UpdateCourse(string reference, Course course);
    Task<string> DeleteCourse(string reference);
    Task<Course> GetCourseByReference(string reference);
    Task<List<Course>> GetCourseList(int page);
    Task<List<Course>> SearchCourseList(int page, string title);
    Task<Course> GetCourseByTitle(string title);
    Task<List<Course>> GetUpcomingCourses(int page);
    
    #endregion
}

