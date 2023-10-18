using FluentValidation;
using SocialMedia.Domain.Aggregates.PostAggregate;

namespace SocialMedia.Domain.Validators.PostValidators;
public class PostCommentValidator : AbstractValidator<PostComment>
{
    public PostCommentValidator()
    {
           RuleFor(x => x.Text)
            .NotNull().WithMessage("Commment should not be null")
            .NotEmpty().WithMessage("Commment should not be empty")
            .MaximumLength(1000)
            .MinimumLength(1);
    }
}
