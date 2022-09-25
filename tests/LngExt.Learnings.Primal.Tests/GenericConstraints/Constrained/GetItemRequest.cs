using FluentValidation;
using MediatR;

namespace LngExt.Learnings.Primal.Tests.GenericConstraints.Constrained;

public record GetItemRequest(string Id) : IRequest<GetItemResponse>;

#region 1-Creating `BaseRequestHandler`

public abstract record BaseRequestHandler<TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest> _validator;

    protected BaseRequestHandler(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    protected abstract Task<TResponse> ExecuteAsync(TRequest request, CancellationToken token);

    protected virtual async Task ValidateAsync(TRequest request, CancellationToken token)
    {
        var validationResult = await _validator.ValidateAsync(request, token);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);
        return await ExecuteAsync(request, cancellationToken);
    }
}

public record GetItemRequestHandler : BaseRequestHandler<GetItemRequest, GetItemResponse>
{
    public GetItemRequestHandler(IValidator<GetItemRequest> validator) : base(validator) { }

    protected override async Task<GetItemResponse> ExecuteAsync(
        GetItemRequest request,
        CancellationToken token
    )
    {
        await Task.Delay(TimeSpan.FromSeconds(1), token);
        return new GetItemResponse(request.Id, "todo", "todo");
    }
}

// 1. G - The request handlers don't need to do perform validations anymore.
// 2. B - Cannot enforce the implementation of a validator for TRequest.

#endregion

#region 2-Enforce validation on the `BaseRequestHandler`

// public abstract record BaseRequestHandler<TRequest, TResponse, TValidator>
//     : IRequestHandler<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>
//     where TValidator : IValidator<TRequest>
// {
//     private readonly IValidator<TRequest> _validator;
//
//     protected BaseRequestHandler(IValidator<TRequest> validator) => _validator = validator;
//
//     public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
//     {
//         await ValidateAsync(request, cancellationToken);
//         return await ExecuteAsync(request, cancellationToken);
//     }
//
//     protected abstract Task<TResponse> ExecuteAsync(TRequest request, CancellationToken token);
//
//     protected virtual async Task ValidateAsync(TRequest request, CancellationToken token)
//     {
//         var validationResult = await _validator.ValidateAsync(request, token);
//         if (!validationResult.IsValid)
//             throw new ValidationException(validationResult.Errors);
//     }
// }
//
// public record GetItemRequestHandler
//     : BaseRequestHandler<GetItemRequest, GetItemResponse, GetItemRequestValidator>
// {
//     public GetItemRequestHandler(IValidator<GetItemRequest> validator) : base(validator) { }
//
//     protected override async Task<GetItemResponse> ExecuteAsync(
//         GetItemRequest request,
//         CancellationToken token
//     )
//     {
//         await Task.Delay(TimeSpan.FromSeconds(1), token);
//         return new GetItemResponse(request.Id, "todo", "todo");
//     }
// }
//
// // 1. B - Compiler warning of `TValidator` has not been used.
// // 2. B - Also, handler is not really the correct place to enforce this. The `TValidator` is more associated with the `TRequest`.
// // 3. B - If we enforce the `TValidator` on the constructor, we lose the ability to inject a more generic `IValidator<TRequest>`.

#endregion

#region 3-Enforce validation on the `TRequest`

// public abstract record BaseMediatorRequest<TRequest, TResponse, TValidator>
//     where TRequest : BaseMediatorRequest<TRequest, TResponse, TValidator>, IRequest<TResponse>
//     where TValidator : IValidator<TRequest>;
//
// public record SomeResponse(string Id);
//
// public record GetItemRequest(string Id)
//     : BaseMediatorRequest<GetItemRequest, GetItemResponse, GetItemRequestValidator>,
//         IRequest<GetItemResponse>;
//
// public abstract record BaseRequestHandler<TRequest, TResponse, TValidator>
//     : IRequestHandler<TRequest, TResponse>
//     where TRequest : BaseMediatorRequest<TRequest, TResponse, TValidator>, IRequest<TResponse>
//     where TValidator : IValidator<TRequest>
// {
//     private readonly IValidator<TRequest> _validator;
//
//     protected BaseRequestHandler(IValidator<TRequest> validator)
//     {
//         _validator = validator;
//     }
//
//     public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
//     {
//         await ValidateAsync(request, cancellationToken);
//         return await ExecuteAsync(request, cancellationToken);
//     }
//
//     protected abstract Task<TResponse> ExecuteAsync(TRequest request, CancellationToken token);
//
//     protected virtual async Task ValidateAsync(TRequest request, CancellationToken token)
//     {
//         var validationResult = await _validator.ValidateAsync(request, token);
//         if (!validationResult.IsValid)
//             throw new ValidationException(validationResult.Errors);
//     }
// }
//
// public record GetItemRequestHandler
//     : BaseRequestHandler<GetItemRequest, GetItemResponse, GetItemRequestValidator>
// {
//     public GetItemRequestHandler(IValidator<GetItemRequest> validator) : base(validator) { }
//
//     protected override async Task<GetItemResponse> ExecuteAsync(
//         GetItemRequest request,
//         CancellationToken token
//     )
//     {
//         await Task.Delay(TimeSpan.FromSeconds(1));
//         return new GetItemResponse(request.Id, "todo", "todo");
//     }
// }
//
// // 1. G - Using the generic base classes, you cannot miss any of the required classes or implementations.
// // 2. B - The "framework / ground work" could be harder to read and understand (subjective).
// // 3. G - From a user's point of view, just inherit from the classes, no need to understand the inner gymnastics.

#endregion


