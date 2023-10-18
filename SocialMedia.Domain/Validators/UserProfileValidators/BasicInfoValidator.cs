using FluentValidation;
using SocialMedia.Domain.Aggregates.UserProfileAggregate;

namespace SocialMedia.Domain.Validators.UserProfileValidator;
public class BasicInfoValidator : AbstractValidator<BasicInfo>
{
    public BasicInfoValidator()
    {
        RuleFor(info => info.FirstName)
            .NotNull().WithMessage("First name is required.")
            .MinimumLength(3).WithMessage("First name length must 3 characters long.")
            .MaximumLength(50).WithMessage("First name can contain at most 50 characters long.");

        RuleFor(info => info.LastName)
            .NotNull().WithMessage("Last Name is required.")
            .MinimumLength(3).WithMessage("Last Name length must 3 characters long.")
            .MaximumLength(50).WithMessage("Last Name can contain at most 50 characters long.");

        RuleFor(info => info.EmailAddress)
            .NotNull().WithMessage("Emai is required.")
            .EmailAddress().WithMessage("Provided email is not a valid email.");

        RuleFor(info => info.DateOfBirth)
            .NotNull().WithMessage("Date of birth is required.")
            .InclusiveBetween(new DateTime(DateTime.Now.AddYears(-100).Ticks), 
                new DateTime(DateTime.Now.AddYears(-18).Ticks))
            .WithMessage("Age need to be between 18-100");
    }
}
