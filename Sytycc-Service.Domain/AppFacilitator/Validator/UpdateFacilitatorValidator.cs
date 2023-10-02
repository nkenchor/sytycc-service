using FluentValidation;
using FluentValidation.Results;

namespace Sytycc_Service.Domain;

public class UpdateFacilitatorValidator : AbstractValidator<UpdateFacilitatorDto>
{
    public UpdateFacilitatorValidator()
    {
          
            RuleFor(facilitator => facilitator.FirstName)
                .NotEmpty().WithMessage("FirstName must not be empty.")
                .Matches("^[a-zA-Z]+$").WithMessage("First Name can only contain letters.");

            RuleFor(facilitator => facilitator.LastName)
                .NotEmpty().WithMessage("LastName must not be empty.")
                .Matches("^[a-zA-Z]+$").WithMessage("Last Name can only contain letters.");

            RuleFor(facilitator => facilitator.Bio)
                .NotEmpty().WithMessage("Bio must not be empty.")
                .Length(10, 500).WithMessage("Bio should be between 10 and 500 characters.");

            RuleFor(facilitator => facilitator.Email)
                .NotEmpty().WithMessage("Email must not be empty.")
                .EmailAddress().WithMessage("Email must be in valid format.");

            RuleFor(facilitator => facilitator.Phone)
                .NotEmpty().WithMessage("Phone must not be empty.")
                .Matches("^\\+?[1-9]\\d{1,14}$").WithMessage("Phone number must be in a valid international format.");

            // RuleFor(facilitator => facilitator.LinkedIn)
            //     .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _) && uri.StartsWith("https://www.linkedin.com/"))
            //     .When(f => !string.IsNullOrWhiteSpace(f.LinkedIn))
            //     .WithMessage("LinkedIn URL must be a valid LinkedIn URL.");

            // RuleFor(facilitator => facilitator.Instagram)
            //     .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _) && uri.StartsWith("https://www.instagram.com/"))
            //     .When(f => !string.IsNullOrWhiteSpace(f.Instagram))
            //     .WithMessage("Instagram URL must be a valid Instagram URL.");

            // RuleFor(facilitator => facilitator.Twitter)
            //     .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _) && uri.StartsWith("https://twitter.com/"))
            //     .When(f => !string.IsNullOrWhiteSpace(f.Twitter))
            //     .WithMessage("Twitter URL must be a valid Twitter URL.");
        
    }
        public override ValidationResult Validate(ValidationContext<UpdateFacilitatorDto> context)
    {
        return context.InstanceToValidate == null
            ? new ValidationResult(new[] { new ValidationFailure(nameof(UpdateFacilitatorDto), 
            "Parameters must be in the required format and must not be null. Please stand advised.") })
            : base.Validate(context);
    }
}  