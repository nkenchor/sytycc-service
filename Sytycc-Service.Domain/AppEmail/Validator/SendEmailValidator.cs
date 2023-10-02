using FluentValidation;
using Sytycc_Service.Domain;

public class EmailValidator : AbstractValidator<EmailDto>
{
    public EmailValidator()
    {
        RuleFor(x => x.Recipient)
            .NotEmpty().WithMessage("Recipient is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required.")
            .MaximumLength(255).WithMessage("Subject must be less than 256 characters.");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Body is required.");
    }
}
