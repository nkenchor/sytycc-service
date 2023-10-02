using FluentValidation;
using FluentValidation.Results;

namespace Sytycc_Service.Domain;

public class UpdateParticipantValidator : AbstractValidator<UpdateParticipantDto>
{
    public UpdateParticipantValidator()
    {
          
           RuleFor(participant => participant.FirstName)
                .NotEmpty().WithMessage("FirstName must not be empty.")
                .Matches("^[a-zA-Z]+$").WithMessage("First Name can only contain letters.");

            RuleFor(participant => participant.LastName)
                .NotEmpty().WithMessage("LastName must not be empty.")
                .Matches("^[a-zA-Z]+$").WithMessage("Last Name can only contain letters.");

            RuleFor(participant => participant.Bio)
                .NotEmpty().WithMessage("Bio must not be empty.")
                .Length(10, 500).WithMessage("Bio should be between 10 and 500 characters.");

            RuleFor(participant => participant.Email)
                .NotEmpty().WithMessage("Email must not be empty.")
                .EmailAddress().WithMessage("Email must be in valid format.");

            RuleFor(participant => participant.Phone)
                .NotEmpty().WithMessage("Phone must not be empty.")
                .Matches("^\\+?[1-9]\\d{1,14}$").WithMessage("Phone number must be in a valid international format.");

            // RuleFor(participant => participant.LinkedIn)
            //     .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _) && uri.StartsWith("https://www.linkedin.com/"))
            //     .When(f => !string.IsNullOrWhiteSpace(f.LinkedIn))
            //     .WithMessage("LinkedIn URL must be a valid LinkedIn URL.");

            // RuleFor(participant => participant.Instagram)
            //     .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _) && uri.StartsWith("https://www.instagram.com/"))
            //     .When(f => !string.IsNullOrWhiteSpace(f.Instagram))
            //     .WithMessage("Instagram URL must be a valid Instagram URL.");

            // RuleFor(participant => participant.Twitter)
            //     .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _) && uri.StartsWith("https://twitter.com/"))
            //     .When(f => !string.IsNullOrWhiteSpace(f.Twitter))
            //     .WithMessage("Twitter URL must be a valid Twitter URL.");
        
           
    }
        public override ValidationResult Validate(ValidationContext<UpdateParticipantDto> context)
    {
        return context.InstanceToValidate == null
            ? new ValidationResult(new[] { new ValidationFailure(nameof(UpdateParticipantDto), 
            "Parameters must be in the required format and must not be null. Please stand advised.") })
            : base.Validate(context);
    }
}  