using MediatR;

namespace LngExt.Learnings.Primal.Tests.GenericConstraints.Normal;

public record GetItemRequest(string Id) : IRequest<GetItemResponse>;