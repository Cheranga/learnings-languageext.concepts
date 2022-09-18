using FluentAssertions;

namespace LngExt.Learnings.Primal.Tests.DiscriminatedUnions.Either;

public static class ProductLifeCycleTests
{
    [Fact]
    public static void ValidProductEventStream()
    {
        var emptyProduct = new BaseProduct.EmptyProduct();
        var correlationId = Guid.NewGuid();
        var productEvents = new Queue<BaseProductEvent>(
            new BaseProductEvent[]
            {
                new BaseProductEvent.ProductAdded(correlationId, "666", "Key Board", 5, 300),
                new BaseProductEvent.PriceChanged(correlationId, 250),
                new BaseProductEvent.StockInHandUpdated(correlationId, 10),
                new BaseProductEvent.StockInHandUpdated(correlationId, -10),
                new BaseProductEvent.PriceChanged(correlationId, 200),
            }
        );

        var updatedProduct = ProductLifeCycle.Evolve(emptyProduct, productEvents);
        updatedProduct
            .Should()
            .BeEquivalentTo(new BaseProduct.ActiveProduct("666", "Key Board", 5, 200));
        updatedProduct
            .GetProductInfo()
            .Should()
            .BeEquivalentTo((nameof(BaseProduct.ActiveProduct), "666", "Key Board"));
    }

    [Fact]
    public static void InvalidProductEventStream()
    {
        var correlationId = Guid.NewGuid();
        var productEvents = new Queue<BaseProductEvent>(
            new BaseProductEvent[]
            {
                new BaseProductEvent.ProductAdded(correlationId, "666", "Mouse", 100, 25.90m),
                new BaseProductEvent.ProductRemoved(correlationId, "666"),
                new BaseProductEvent.StockInHandUpdated(correlationId, 10)
            }
        );

        var updatedProduct = ProductLifeCycle.Evolve(new BaseProduct.EmptyProduct(), productEvents);
        updatedProduct.Should().BeEquivalentTo(new BaseProduct.DeletedProduct("666"));
        updatedProduct
            .GetProductInfo()
            .Should()
            .BeEquivalentTo((nameof(BaseProduct.DeletedProduct), "666", "666"));
    }
}