using FluentValidation;
using SocialMedia.Domain.Aggregates.PostAggregates;

namespace SocialMedia.Domain.Validators.PostValidators;
public class PostValidator : AbstractValidator<Post>
{
    public PostValidator()
    {
            RuleFor(x => x.TextContent)
            .NotNull().WithMessage("Post text content should not be null")
            .NotEmpty().WithMessage("Post text content should not be empty")
            .MaximumLength(1000)
            .MinimumLength(1);
    }
}
