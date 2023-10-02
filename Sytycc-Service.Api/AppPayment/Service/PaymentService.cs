
using System.Globalization;
using Serilog;
using Stripe;
using Sytycc_Service.Domain;


namespace Sytycc_Service.Api;

public class PaymentService : IPaymentService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IRegistrationRepository _registrationRepository;

    public PaymentService(ICourseRepository courseRepository, IRegistrationRepository registrationRepository)
    {
        StripeConfiguration.ApiKey = Service.StripeSecretKey;
        _courseRepository = courseRepository;
        _registrationRepository = registrationRepository;

    }

    public async Task<PaymentIntent> InitiatePayment(string courseReference, string participantReference, PaymentMethodDto paymentMethodDto)
    {
        try
        {
            var service = new PaymentIntentService();
            var course = await _courseRepository.GetCourseByReference(courseReference);
            
            if (course == null)
            {
                Log.Error($"No course found by the given reference: {courseReference}.");
                throw new NotFoundException($"Course not found by the given reference: {courseReference}.");
            }

             // Check if the course has already ended
            DateTime courseEndDate = DateTime.ParseExact(course.EndDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            if (DateTime.Now >= courseEndDate)
            {
                Log.Error("Attempt to register for a course that has already ended.");
                throw new BadRequestException("You cannot register for a course that has already ended.");
            }

            var registration = await _registrationRepository.GetRegistrationByParticipantCourse(courseReference,participantReference);
            if (registration != null)
            {
                Log.Error($"Sorry, you have already registered for this course: {courseReference} with participant reference: {participantReference}");
                throw new ConflictException($"Sorry, you have already registered for this course: {courseReference} with participant reference: {participantReference}");
            }

            if (paymentMethodDto.PaymentMethodId == null || paymentMethodDto.PaymentMethodId ==string.Empty)
            {
                Log.Error($"No Payment Method ID found.");
                throw new NotFoundException($"No payment method ID found.");
            }

            var options = new PaymentIntentCreateOptions
            {
                Amount = course.Price * 100,
                Currency = course.Currency,
                Description = $"Payment for {course.Title}",
                PaymentMethodTypes = new List<string> { "card" },
                Confirm = true,
                ConfirmationMethod = "manual",
                UseStripeSdk = true,
                PaymentMethod = paymentMethodDto.PaymentMethodId,
            };

            return await service.CreateAsync(options);
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
        catch (StripeException e)
        {
            throw PaymentExceptionHandler.HandleException(e);
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            Log.Error($"Error initiating payment: {e.Message}");
            throw new InternalServerException($"Error initiating payment: {e.Message}");
        }
    }

    public async Task<PaymentIntent> ConfirmPayment(string paymentIntentId)
    {
        try
        {
            var service = new PaymentIntentService();
            var confirmOptions = new PaymentIntentConfirmOptions { };

            if (paymentIntentId== null || paymentIntentId ==string.Empty)
            {
                Log.Error($"No Payment Intent ID found.");
                throw new NotFoundException($"No payment intent ID found.");
            }
            
            return await service.ConfirmAsync(paymentIntentId, confirmOptions);
        }
        catch (StripeException e)
        {
            throw PaymentExceptionHandler.HandleException(e);
        }
        catch (AppException)  // Catching known exceptions
        {
            throw;
        }
        catch (Exception e)  // Catching unexpected exceptions
        {
            Log.Error($"Error confirming payment: {e.Message}");
            throw new InternalServerException($"Error confirming payment: {e.Message}");
        }
    }
}

