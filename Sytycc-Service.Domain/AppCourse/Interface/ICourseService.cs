

namespace Sytycc_Service.Domain;

public interface ICourseValidationService{
      
      AppException ValidateCreateCourse(CreateCourseDto createCourseDto);
      AppException ValidateUpdateCourse(UpdateCourseDto updateCourseDto);
      
}

