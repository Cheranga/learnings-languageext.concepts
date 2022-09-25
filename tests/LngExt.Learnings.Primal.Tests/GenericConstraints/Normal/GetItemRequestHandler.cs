using FluentValidation;
using MediatR;

namespace LngExt.Learnings.Primal.Tests.GenericConstraints.Normal;

public record GetItemRequestHandler : IRequestHandler<GetItemRequest, GetItemResponse>
{
    private readonly IValidator<GetItemRequest> _validator;

    public GetItemRequestHandler(IValidator<GetItemRequest> validator) => _validator = validator;

    public async Task<GetItemResponse> Handle(
        GetItemRequest request,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        return new GetItemResponse(request.Id, "todo", "todo");
    }
}
