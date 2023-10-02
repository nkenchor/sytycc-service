using Sytycc_Service.Domain;
using Serilog;
using System.Globalization;

namespace Sytycc_Service.Api;
public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly IFacilitatorRepository _facilitatorRepository;
    private readonly IRegistrationValidationService _validationService;


    public RegistrationService(IRegistrationRepository registrationRepository,
                                ICourseRepository courseRepository,
                                IParticipantRepository participantRepository,
                                IFacilitatorRepository facilitatorRepository,
                                IRegistrationValidationService validationService)
    {
        _registrationRepository = registrationRepository;
        _courseRepository = courseRepository;
        _participantRepository = participantRepository;
        _facilitatorRepository = facilitatorRepository;
        _validationService = validationService;
    }

    public async Task<string> CreateRegistration(string paymentIntentId,CreateRegistrationDto registrationDto)
    {
        try 
        {
            var validationException = _validationService.ValidateCreateRegistration(registrationDto);
            if (validationException != null) throw validationException;

            var course = await _courseRepository.GetCourseByReference(registrationDto.CourseReference) ?? throw new NotFoundException("Course not found by the given reference.");
            var participant = await _participantRepository.GetParticipantByReference(registrationDto.ParticipantReference) ?? throw new NotFoundException("Participant not found by the given reference.");
            
            // Check if the course has already ended
            DateTime courseEndDate = DateTime.ParseExact(course.EndDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            if (DateTime.Now >= courseEndDate)
            {
                Log.Error("Attempt to register for a course that has already ended.");
                throw new BadRequestException("You cannot register for a course that has already ended.");
            }
            
            int discountedPrice = course.Price - course.Discount;

            if (registrationDto.AmountPaid != course.Price && registrationDto.AmountPaid != discountedPrice)
            {
                Log.Error("AmountPaid is neither the course's full price nor its discounted price.");
                throw new BadRequestException("The amount paid must be either the course's full price or its discounted price.");
            }

            var registration = await _registrationRepository.GetRegistrationByParticipantCourse(registrationDto.CourseReference,registrationDto.ParticipantReference);

            
            if (registration != null)
            {
                Log.Error($"Sorry, you have already registered for this course: {registrationDto.CourseReference} with participant reference: {registrationDto.ParticipantReference}");
                throw new ConflictException($"Sorry, you have already registered for this course: {registrationDto.CourseReference} with participant reference: {registrationDto.ParticipantReference}");
            }
            
            registration = new Registration(paymentIntentId,registrationDto, registrationDto.AmountPaid,course.Discount);
            return await _registrationRepository.CreateRegistration(registration);
        } 
        
        catch (ConflictException e)
        {
            Log.Error($"Conflict Error: {e.Message}");
            throw;
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
     
        catch (Exception e)  // Catching unexpected exceptions
        {
            Log.Error($"Error Creating Registration: {e.Message}");
            throw new InternalServerException($"Error Creating Registration: {e.Message}");
        }
    }

    

    public async Task<string> DeleteRegistration(string reference)
    {
        try 
        {
            await GetRegistrationByReference(reference);
            return await _registrationRepository.DeleteRegistration(reference);
        } 
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            Log.Error($"Error Deleting Registration: {e.Message}");
            throw new InternalServerException($"Error Deleting Registration: {e.Message}");
        }
    }

    public async Task<RegistrationDto> GetRegistrationByReference(string reference)
    {
        try 
        {
            var registration = await _registrationRepository.GetRegistrationByReference(reference);
            if (registration == null)
            {
                Log.Error($"No registration found with reference: {reference}");
                throw new NotFoundException($"No registration found with reference: {reference}");
            }

            var course = await _courseRepository.GetCourseByReference(registration.CourseReference);
            if (course == null)
            {
                Log.Error($"No course found with reference: {registration.CourseReference}");
                throw new NotFoundException($"No course found with reference: {registration.CourseReference}");
            }

            var facilitator = await _facilitatorRepository.GetFacilitatorByReference(course.FacilitatorReference);
            if (facilitator == null)
            {
                Log.Error($"No facilitator found with reference: {course.FacilitatorReference}");
                throw new NotFoundException($"No facilitator found with reference: {course.FacilitatorReference}");
            }

            var participant = await _participantRepository.GetParticipantByReference(registration.ParticipantReference);
            if (participant == null)
            {
                Log.Error($"No participant found with reference: {registration.ParticipantReference}");
                throw new NotFoundException($"No participant found with reference: {registration.ParticipantReference}");
            }

            return new RegistrationDto(registration, course, facilitator, participant);
        } 
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            Log.Error($"Error Fetching Registration by Reference: {e.Message}");
            throw new InternalServerException($"Error Fetching Registration by Reference: {e.Message}");
        }
    }
public async Task<RegistrationDto> GetRegistrationByParticipantCourse(string courseReference, string participantReference)
{
    try 
    {
        var registration = await _registrationRepository.GetRegistrationByParticipantCourse(courseReference, participantReference);
        if (registration == null)
        {
            Log.Error($"No registration found for course reference: {courseReference} and participant reference: {participantReference}");
            throw new NotFoundException($"No registration found for course reference: {courseReference} and participant reference: {participantReference}");
        }

        var course = await _courseRepository.GetCourseByReference(registration.CourseReference);
        if (course == null)
        {
            Log.Error($"No course found with reference: {registration.CourseReference}");
            throw new NotFoundException($"No course found with reference: {registration.CourseReference}");
        }

        var facilitator = await _facilitatorRepository.GetFacilitatorByReference(course.FacilitatorReference);
        if (facilitator == null)
        {
            Log.Error($"No facilitator found with reference: {course.FacilitatorReference}");
            throw new NotFoundException($"No facilitator found with reference: {course.FacilitatorReference}");
        }

        var participant = await _participantRepository.GetParticipantByReference(registration.ParticipantReference);
        if (participant == null)
        {
            Log.Error($"No participant found with reference: {registration.ParticipantReference}");
            throw new NotFoundException($"No participant found with reference: {registration.ParticipantReference}");
        }

        return new RegistrationDto(registration, course, facilitator, participant);
    } 
    catch (AppException)  // Catching known exceptions
    {
        throw;
    }
    catch (Exception e)  // Catching unexpected exceptions
    {
        Log.Error($"Error Fetching Registration by Participant and Course: {e.Message}");
        throw new InternalServerException($"Error Fetching Registration by Participant and Course: {e.Message}");
    }
}


    public async Task<List<RegistrationDto>> GetRegistrationList(int page)
    {
        try 
        {
            var registrations = await _registrationRepository.GetRegistrationList(page);
            if (registrations == null || !registrations.Any())
            {
                Log.Warning("No registrations found for the given page.");
                throw new NotFoundException("No registrations found for the given page.");
            }

            var registrationDtos = new List<RegistrationDto>();

            foreach (var registration in registrations)
            {
                var course = await _courseRepository.GetCourseByReference(registration.CourseReference);
                if (course == null)
                {
                    Log.Error($"No course found with reference: {registration.CourseReference}");
                    throw new NotFoundException($"No course found with reference: {registration.CourseReference}");
                }

                var facilitator = await _facilitatorRepository.GetFacilitatorByReference(course.FacilitatorReference);
                if (facilitator == null)
                {
                    Log.Error($"No facilitator found with reference: {course.FacilitatorReference}");
                    throw new NotFoundException($"No facilitator found with reference: {course.FacilitatorReference}");
                }

                var participant = await _participantRepository.GetParticipantByReference(registration.ParticipantReference);
                if (participant == null)
                {
                    Log.Error($"No participant found with reference: {registration.ParticipantReference}");
                    throw new NotFoundException($"No participant found with reference: {registration.ParticipantReference}");
                }

                registrationDtos.Add(new RegistrationDto(registration, course, facilitator, participant));
            }

            return registrationDtos;
        } 
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }

        catch (Exception e)  // Catching unexpected exceptions
        {
            Log.Error($"Error Fetching Registration List: {e.Message}");
            throw new InternalServerException($"Error Fetching Registration List: {e.Message}");
        }
    }

    public async Task<List<RegistrationDto>> SearchRegistrationList(int page, string courseReference)
    {
        try 
        {
            var registrations = await _registrationRepository.SearchRegistrationList(page, courseReference);
            if (registrations == null || !registrations.Any())
            {
                Log.Warning($"No registrations found with course reference: {courseReference} for the given page.");
                throw new NotFoundException($"No registrations found with course reference: {courseReference} for the given page.");
            }

            var registrationDtos = new List<RegistrationDto>();

            foreach (var registration in registrations)
            {
                var course = await _courseRepository.GetCourseByReference(registration.CourseReference);
                if (course == null)
                {
                    Log.Error($"No course found with reference: {registration.CourseReference}");
                    throw new NotFoundException($"No course found with reference: {registration.CourseReference}");
                }

                var facilitator = await _facilitatorRepository.GetFacilitatorByReference(course.FacilitatorReference);
                if (facilitator == null)
                {
                    Log.Error($"No facilitator found with reference: {course.FacilitatorReference}");
                    throw new NotFoundException($"No facilitator found with reference: {course.FacilitatorReference}");
                }

                var participant = await _participantRepository.GetParticipantByReference(registration.ParticipantReference);
                if (participant == null)
                {
                    Log.Error($"No participant found with reference: {registration.ParticipantReference}");
                    throw new NotFoundException($"No participant found with reference: {registration.ParticipantReference}");
                }

                registrationDtos.Add(new RegistrationDto(registration, course, facilitator, participant));
            }

            return registrationDtos;
        } 
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            Log.Error($"Error Searching Registration List: {e.Message}");
            throw new InternalServerException($"Error Searching Registration List: {e.Message}");
        }
    }

}

