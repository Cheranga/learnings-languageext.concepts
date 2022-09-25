using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LngExt.Learnings.Primal.Tests.GenericConstraints.Constrained;

public static class GetItemRequestHandlerTests
{
    [Fact]
    public static async Task Test()
    {
        var services = new ServiceCollection();
        services.AddValidatorsFromAssembly(typeof(GetItemRequestHandlerTests).Assembly);
        services.AddMediatR(typeof(GetItemRequestHandlerTests).Assembly);

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        var request = new GetItemRequest("666");
        var response = await mediator.Send(request);

        response.Should().NotBeNull();
        response.Id.Should().Be(request.Id);
    }
}
