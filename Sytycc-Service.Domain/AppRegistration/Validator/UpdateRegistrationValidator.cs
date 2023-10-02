using FluentValidation;
using FluentValidation.Results;

namespace Sytycc_Service.Domain;

public class UpdateRegistrationValidator : AbstractValidator<UpdateRegistrationDto>
{
    public UpdateRegistrationValidator()
    {
          
            RuleFor(registration => registration.CourseReference)
                .NotEmpty().WithMessage("Course Reference must not be empty.")
                .Must(IsGuid).WithMessage("Course Reference must be a valid GUID.");

            RuleFor(registration => registration.ParticipantReference)
                .NotEmpty().WithMessage("Participant Reference must not be empty.")
                .Must(IsGuid).WithMessage("Participant Reference must be a valid GUID.");


           
          
           
    }
    private bool IsGuid(string guid)
        {
            return Guid.TryParse(guid, out _);
        }
        public override ValidationResult Validate(ValidationContext<UpdateRegistrationDto> context)
    {
        return context.InstanceToValidate == null
            ? new ValidationResult(new[] { new ValidationFailure(nameof(UpdateRegistrationDto), 
            "Parameters must be in the required format and must not be null. Please stand advised.") })
            : base.Validate(context);
    }
}  