using System.Globalization;
using FluentValidation;
using FluentValidation.Results;

namespace Sytycc_Service.Domain;

public class CreateCourseValidator : AbstractValidator<CreateCourseDto>
{
    private static readonly string[] ValidTrainingTypes = { "Self-paced", "Instructor-led", "Project-based" };
    private static readonly string[] ValidCourseLevels = { "Beginner", "Intermediate", "Advanced" };

    public CreateCourseValidator()
    {
            RuleFor(course => course.Title)
                .NotEmpty()
                .WithMessage("Title must not be empty. Please stand advised.")
                .Length(5, 200)
                .WithMessage("Title should be between 5 and 200 characters.");

            RuleFor(course => course.Description)
                .NotEmpty()
                .WithMessage("Description must not be empty. Please stand advised.")
                .Length(10, 2000)
                .WithMessage("Description should be between 10 and 2000 characters.");

            RuleFor(course => course.CourseOutLine)
                .NotNull()
                .WithMessage("Course outline must not be null. Please stand advised.")
                .Must(ol => ol.Length > 0)
                .WithMessage("Course outline must not be empty. Please stand advised.")
                .ForEach(rule => rule
                    .NotEmpty()
                    .WithMessage("Course outline item must not be empty.")
                    .Length(1, 500)
                    .WithMessage("Each course outline item should be between 1 and 500 characters."));
            RuleFor(course => course.FacilitatorReference)
                .NotEmpty()
                .WithMessage("Facilitator reference must not be empty. Please stand advised.")
                .Must(reference => Guid.TryParse(reference, out _))
                .WithMessage("Facilitator reference must be a valid GUID.");

            RuleFor(course => course.TrainingType)
                .NotEmpty()
                .WithMessage("Training Type must not be empty. Please stand advised.")
                .Must(type => ValidTrainingTypes.Contains(type, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Training Type must be either 'Self-paced' or 'Instructor-led'.");

            RuleFor(course => course.Level)
                .NotEmpty()
                .WithMessage("Level must not be empty. Please stand advised.")
                .Must(level => ValidCourseLevels.Contains(level, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Level must be either 'Beginner', 'Intermediate', or 'Advanced'.");

            RuleFor(course => course.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");

            RuleFor(course => course.Discount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Discount must not be negative.");

              RuleFor(course => course.Currency)
                .Equal("GBP", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Currency must be GBP.");

            RuleFor(course => course.TimeZone)
                .Equal("BST", StringComparer.OrdinalIgnoreCase)
                .WithMessage("Timezone must be BST.");

              RuleFor(course => course.StartDate)
                .Must(BeValidDate)
                .WithMessage("Start Date must be in the format DD-MM-YYYY.");

            RuleFor(course => course.EndDate)
                .Must(BeValidDate)
                .WithMessage("End Date must be in the format DD-MM-YYYY.");

            RuleFor(course => course.StartTime)
                .Must(BeValidTime)
                .WithMessage("Start Time must be in the format h:mm AM/PM.");

            RuleFor(course => course.EndTime)
                .Must(BeValidTime)
                .WithMessage("End Time must be in the format h:mm AM/PM.");

            RuleFor(course => course.DiscountEndDate)
                .Must(BeValidDate)
                .WithMessage("Discount Date must be in the format DD-MM-YYYY.");

           
            RuleFor(course => course)
            .Must(course => BeDateBefore(course.StartDate, course.EndDate))
            .WithMessage("Start date must be before end date.");

            RuleFor(course => course)
            .Must(course => BeTimeBeforeOnSameDay(course.StartDate, course.EndDate, course.StartTime, course.EndTime))
            .WithMessage("On the same day, the start time must be before the end time.");

            RuleFor(course => course.DiscountEndDate)
            .Must(BeFutureDate)
            .When(course => course.Discount > 0)
            .WithMessage("Discount end date should be in the future.");
        
    }


// Utility functions for the validators
    private bool BeDateBefore(string startDate, string endDate)
    {

        bool isValidStart = DateTime.TryParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDateTime);
        bool isValidEnd = DateTime.TryParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDateTime);

        return isValidStart && isValidEnd && startDateTime < endDateTime;
    }

    private bool BeTimeBeforeOnSameDay(string startDate, string endDate, string startTime, string endTime)
    {

        bool isValidStartDate = DateTime.TryParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDateTime);
        bool isValidEndDate = DateTime.TryParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDateTime);
        bool isValidStartTime = DateTime.TryParseExact(startTime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDateTimeTime);
        bool isValidEndTime = DateTime.TryParseExact(endTime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDateTimeTime);

        if (!isValidStartDate || !isValidEndDate || !isValidStartTime || !isValidEndTime)
            return false;

        // If the dates are different, we don't need to check the times.
        if (startDateTime != endDateTime) 
            return true;

        return startDateTimeTime.TimeOfDay < endDateTimeTime.TimeOfDay;
    }

    private bool BeFutureDate(string date)
    {
        bool isValid = DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);
        return isValid && dateTime > DateTime.Today;
    }

        private bool BeValidDate(string date)
        {
            return DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        private bool BeValidTime(string time)
        {
            return DateTime.TryParseExact(time, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
        public override ValidationResult Validate(ValidationContext<CreateCourseDto> context)
    {
        return context.InstanceToValidate == null
            ? new ValidationResult(new[] { new ValidationFailure(nameof(CreateCourseDto), 
            "Parameters must be in the required format and must not be null. Please stand advised.") })
            : base.Validate(context);
    }
}  
