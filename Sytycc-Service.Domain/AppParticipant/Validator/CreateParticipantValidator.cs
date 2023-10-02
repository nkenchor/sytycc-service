using FluentValidation;
using FluentValidation.Results;

namespace Sytycc_Service.Domain;

public class CreateParticipantValidator : AbstractValidator<CreateParticipantDto>
{
    public CreateParticipantValidator()
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

        
        
           
        
    }
        public override ValidationResult Validate(ValidationContext<CreateParticipantDto> context)
    {
        return context.InstanceToValidate == null
            ? new ValidationResult(new[] { new ValidationFailure(nameof(CreateParticipantDto), 
            "Parameters must be in the required format and must not be null. Please stand advised.") })
            : base.Validate(context);
    }
}  
