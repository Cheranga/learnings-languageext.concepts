using FluentValidation;
using MediatR;

namespace LngExt.Learnings.Primal.Tests.GenericConstraints;

public record SomeResponse(string ReferenceId);

public record SomeRequest(string Id, string Name) : IRequest<SomeResponse>;

public class SomeRequestValidator : AbstractValidator<SomeRequest>
{
    public SomeRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("id is required");
        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("name is required");
    }
}

public record SomeRequestHandler : IRequestHandler<SomeRequest, SomeResponse>
{
    private readonly IValidator<SomeRequest> _validator;

    public SomeRequestHandler(IValidator<SomeRequest> validator)
    {
        _validator = validator;
    }

    public async Task<SomeResponse> Handle(SomeRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new Exception("invalid request");
        }

        // do something
        return new SomeResponse($"{request.Id}-{request.Name}");
    }
}

public abstract class BaseRecord<TRequest, TValidator>
    where TRequest : BaseRecord<TRequest, TValidator>
    where TValidator : AbstractValidator<TRequest> { }

public abstract record BaseRequestHandler<TRequest, TResponse, TValidator>
    where TRequest : BaseRecord<TRequest, TValidator>
    where TValidator : AbstractValidator<TRequest>
{
    public abstract Task<TResponse> HandleAsync(TRequest request);
}

public class OtherRequestValidator : AbstractValidator<OtherRequest>
{
    
}
public class OtherRequest : BaseRecord<OtherRequest, OtherRequestValidator>{}

public record OtherRequestHandler
    : BaseRequestHandler<OtherRequest, SomeResponse, OtherRequestValidator>
{
    public override Task<SomeResponse> HandleAsync(OtherRequest request) => throw new NotImplementedException();
}
