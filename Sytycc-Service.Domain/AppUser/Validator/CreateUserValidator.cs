using FluentValidation;
using FluentValidation.Results;

namespace Sytycc_Service.Domain;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
public CreateUserValidator()
{
    RuleFor(user => user.FirstName)
        .NotEmpty().WithMessage("FirstName must not be empty.")
        .Matches("^[a-zA-Z]+$").WithMessage("First Name can only contain letters.");

    RuleFor(user => user.LastName)
        .NotEmpty().WithMessage("LastName must not be empty.")
        .Matches("^[a-zA-Z]+$").WithMessage("Last Name can only contain letters.");

    RuleFor(user => user.Bio)
        .NotEmpty().WithMessage("Bio must not be empty.")
        .Length(10, 500).WithMessage("Bio should be between 10 and 500 characters.");

    RuleFor(user => user.Email)
        .NotEmpty().WithMessage("Email must not be empty.")
        .EmailAddress().WithMessage("Email must be in valid format.");

    RuleFor(user => user.Phone)
        .NotEmpty().WithMessage("Phone must not be empty.")
        .Matches("^\\+?[1-9]\\d{1,14}$").WithMessage("Phone number must be in a valid international format.");

    RuleFor(user => user.UserName)
        .NotEmpty().WithMessage("UserName must not be empty.")
        .Length(6, 10).WithMessage("UserName should be between 6 and 10 characters.");

    RuleFor(user => user.Password)
        .NotEmpty().WithMessage("Password must not be empty.")
        .Length(6, 8).WithMessage("Password should be between 6 and 8 characters.")
        .Matches("^(?=.*[A-Za-z])(?=.*\\d).{6,8}$").WithMessage("Password must be a mix of letters and numbers.");

    RuleFor(user => user.Role)
        .NotEmpty().WithMessage("Role must not be empty.")
        .Must(role => role == "Administrator" || role == "Participant" || role == "Facilitator")
        .WithMessage("Role must be either admin, participant, or facilitator.");
}

    public override ValidationResult Validate(ValidationContext<CreateUserDto> context)
    {
        return context.InstanceToValidate == null
            ? new ValidationResult(new[] { new ValidationFailure(nameof(CreateUserDto), 
            "Parameters must be in the required format and must not be null. Please stand advised.") })
            : base.Validate(context);
    }
}  
