using Sytycc_Service.Domain;
using Serilog;
using System.Data;

namespace Sytycc_Service.Api;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IFacilitatorRepository _facilitatorRepository;
    private readonly ICourseValidationService _courseValidationService;

    public CourseService(
        ICourseRepository courseRepository,
        IFacilitatorRepository facilitatorRepository,
        ICourseValidationService courseValidationService)
    {
        _courseRepository = courseRepository;
        _facilitatorRepository = facilitatorRepository;
        _courseValidationService = courseValidationService;
    }
    public async Task<string> CreateCourse(CreateCourseDto courseDto)
    {
        try
        {
            var validationException = _courseValidationService.ValidateCreateCourse(courseDto);
            if (validationException != null) throw validationException;

            // var availableCourse = await _courseRepository.GetCourseByTitle(courseDto.Title);
            // if (availableCourse != null)
            // {
            //     Log.Warning($"There is already a course found with the given title: {courseDto.Title}.");
            //     throw new DuplicateNameException($"there is already a course found with the given title: {courseDto.Title}.");
            // }

            var facilitator = _facilitatorRepository.GetFacilitatorByReference(courseDto.FacilitatorReference) ?? throw new NotFoundException("Facilitator not found by the given reference.");
            var course = new Course(courseDto);
            return await _courseRepository.CreateCourse(course);
        }
        catch (BadRequestException e)
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (NotFoundException e)
        {
            Log.Error($"Resource Not Found Error: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Log.Error($"Error Creating Course: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }
    public async Task<string> UpdateCourse(string reference, UpdateCourseDto courseDto)
    {
        try
        {
            var validationException = _courseValidationService.ValidateUpdateCourse(courseDto);
            if (validationException != null) throw validationException;
            await GetCourseByReference(reference);
            var facilitator = _facilitatorRepository.GetFacilitatorByReference(courseDto.FacilitatorReference) ?? throw new NotFoundException("Facilitator not found by the given reference.");
            
            var course = new Course(courseDto)
            {
                Reference = reference
            };

            return await _courseRepository.UpdateCourse(reference, course);
        }
      catch (BadRequestException e)
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
        catch (ConflictException e)
        {
            Log.Error($"Conflict Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Database Error: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Log.Error($"Error Updating Course: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }

    public async Task<string> DeleteCourse(string reference)
    {
        try
        {
            await GetCourseByReference(reference);
            return await _courseRepository.DeleteCourse(reference);
        }
        catch (BadRequestException e)
        {
            Log.Error($"Bad Request Error: {e.Message}");
            throw;
        }
      
        catch (ConflictException e)
        {
            Log.Error($"Conflict Error: {e.Message}");
            throw;
        }
        catch (AppException e)
        {
            Log.Error($"Database Error: {e.Message}");
            throw;
        }
       
        catch (Exception e)
        {
            Log.Error($"Error Deleting Course: {e.Message}");
            throw new InternalServerException(e.Message);
        }
    }
    public async Task<CourseDto> GetCourseByReference(string reference)
    {
        try
        {
            var course = await _courseRepository.GetCourseByReference(reference);
            if (course == null)
            {
                Log.Warning($"No course found by the given reference: {reference}.");
                throw new NotFoundException($"Course not found by the given reference: {reference}.");
            }

            var facilitator = await _facilitatorRepository.GetFacilitatorByReference(course.FacilitatorReference);
            if (facilitator == null)
            {
                Log.Error($"Facilitator not found with reference: {course.FacilitatorReference} for course with reference: {reference}.");
                throw new NotFoundException($"Facilitator associated with the course (reference: {reference}) not found.");
            }

            return new CourseDto(course, facilitator);
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }

        catch (Exception e)  // Catching unexpected exceptions
        {
            Log.Error($"Error fetching course by reference: {e.Message}");
            throw new InternalServerException($"Error fetching course by reference: {e.Message}");
        }
    }
    
    public async Task<CourseDto> GetCourseByTitle(string title)
    {
        try
        {
            var course = await _courseRepository.GetCourseByTitle(title);
            if (course == null)
            {
                Log.Warning($"No course found by the given title: {title}.");
                throw new NotFoundException($"Course not found by the given title: {title}.");
            }

            var facilitator = await _facilitatorRepository.GetFacilitatorByReference(course.FacilitatorReference);
            if (facilitator == null)
            {
                Log.Error($"Facilitator not found with reference: {course.FacilitatorReference} for course with title: {title}.");
                throw new NotFoundException($"Facilitator associated with the course (title: {title}) not found.");
            }

            return new CourseDto(course, facilitator);
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }

        catch (Exception e)  // Catching unexpected exceptions
        {
            Log.Error($"Error fetching course by reference: {e.Message}");
            throw new InternalServerException($"Error fetching course by reference: {e.Message}");
        }
    }
    public async Task<List<CourseDto>> GetCourseList(int page)
    {
        try
        {
            var courses = await _courseRepository.GetCourseList(page);
            if (courses == null || !courses.Any())
            {
                Log.Warning($"No courses found for page: {page}.");
                throw new NotFoundException($"No courses found for page: {page}.");
            }

            var facilitators = new List<Facilitator>();
            foreach (var course in courses)
            {
                var facilitator = await _facilitatorRepository.GetFacilitatorByReference(course.FacilitatorReference);
                if (facilitator == null)
                {
                    Log.Error($"Facilitator not found with reference: {course.FacilitatorReference} for course with reference: {course.Reference}.");
                    throw new NotFoundException($"Facilitator associated with the course (reference: {course.Reference}) not found.");
                }
                facilitators.Add(facilitator);
            }

            return courses.Zip(facilitators, (course, facilitator) => new CourseDto(course, facilitator)).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            Log.Error($"Error fetching course list: {e.Message}");
            throw new InternalServerException($"Error fetching course list: {e.Message}");
        }
    }
     public async Task<List<CourseDto>> GetUpcomingCourses(int page)
    {
        try
        {
            var courses = await _courseRepository.GetUpcomingCourses(page);
            if (courses == null || !courses.Any())
            {
                Log.Warning($"No upcoming courses found for page: {page}.");
                throw new NotFoundException($"No upcoming courses found for page: {page}.");
            }

            var facilitators = new List<Facilitator>();
            foreach (var course in courses)
            {
                var facilitator = await _facilitatorRepository.GetFacilitatorByReference(course.FacilitatorReference);
                if (facilitator == null)
                {
                    Log.Error($"Facilitator not found with reference: {course.FacilitatorReference} for course with reference: {course.Reference}.");
                    throw new NotFoundException($"Facilitator associated with the course (reference: {course.Reference}) not found.");
                }
                facilitators.Add(facilitator);
            }

            return courses.Zip(facilitators, (course, facilitator) => new CourseDto(course, facilitator)).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            Log.Error($"Error fetching course list: {e.Message}");
            throw new InternalServerException($"Error fetching course list: {e.Message}");
        }
    }
    public async Task<List<CourseDto>> SearchCourseList(int page, string title)
    {
        try
        {
            var courses = await _courseRepository.SearchCourseList(page, title);
            if (courses == null || !courses.Any())
            {
                Log.Warning($"No courses found with the title: {title}.");
                throw new NotFoundException($"No courses found with the title: {title}.");
            }

            var facilitators = new List<Facilitator>();
            foreach (var course in courses)
            {
                var facilitator = await _facilitatorRepository.GetFacilitatorByReference(course.FacilitatorReference);
                if (facilitator == null)
                {
                    Log.Error($"Facilitator not found with reference: {course.FacilitatorReference} for course with title: {title}.");
                    throw new NotFoundException($"Facilitator associated with the course (title: {title}) not found.");
                }
                facilitators.Add(facilitator);
            }

            return courses.Zip(facilitators, (course, facilitator) => new CourseDto(course, facilitator)).ToList();
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }

        catch (Exception e)  // Catching unexpected exceptions
        {
            Log.Error($"Error searching course list by title: {e.Message}");
            throw new InternalServerException($"Error searching course list by title: {e.Message}");
        }
    }

}

