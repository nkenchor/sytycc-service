
namespace Sytycc_Service.Domain;

public class CourseValidationService:ICourseValidationService
{
      
      public AppException ValidateCreateCourse(CreateCourseDto createCourseDto)
      {
        return new ErrorService().GetValidationExceptionResult(new CreateCourseValidator().Validate(createCourseDto));
      }   
      public AppException ValidateUpdateCourse(UpdateCourseDto updateCourseDto)
      {
        return new ErrorService().GetValidationExceptionResult(new UpdateCourseValidator().Validate(updateCourseDto));
      }   
}