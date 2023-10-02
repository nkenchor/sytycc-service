using Sytycc_Service.Domain;

namespace Sytycc_Service.Api;

public interface ICourseService{

   
    Task<string> CreateCourse(CreateCourseDto course);
    Task<string> UpdateCourse(string reference, UpdateCourseDto course);
    Task<string> DeleteCourse(string reference);
    Task<CourseDto> GetCourseByReference(string reference);
    Task<List<CourseDto>> GetCourseList(int page);
    Task<List<CourseDto>> SearchCourseList(int page, string title);
    Task<CourseDto> GetCourseByTitle(string title);
    Task<List<CourseDto>> GetUpcomingCourses(int page);
   
}

