using FluentValidation;

namespace LngExt.Learnings.Primal.Tests.GenericConstraints.Normal;

public class GetItemRequestValidator : AbstractValidator<GetItemRequest>
{
    public GetItemRequestValidator() =>
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("id is required");
}