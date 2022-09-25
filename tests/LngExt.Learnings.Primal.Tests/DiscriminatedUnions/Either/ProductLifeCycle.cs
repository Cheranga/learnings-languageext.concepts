namespace LngExt.Learnings.Primal.Tests.DiscriminatedUnions.Either;

public sealed record ProductLifeCycle
{
    public static BaseProduct Evolve(BaseProduct product, Queue<BaseProductEvent> events)
    {
        if (!events.Any())
        {
            return product;
        }

        var updatedProduct = Evolve(product, events.Dequeue());
        return Evolve(updatedProduct, events);
    }

    private static BaseProduct Evolve(BaseProduct product, BaseProductEvent productEvent) =>
        productEvent switch
        {
            BaseProductEvent.ProductAdded op
                => product is BaseProduct.EmptyProduct
                    ? ActivateProduct(
                        op.CorrelationId,
                        op.ProductId,
                        op.Name,
                        op.StockInHand,
                        op.Price
                    )
                    : product,
            BaseProductEvent.StockInHandUpdated op
                => product switch
                {
                    BaseProduct.ActiveProduct ap
                        => UpdateStockInHand(op.CorrelationId, ap, op.Amount),
                    BaseProduct.RestockRequiredProduct ap
                        => UpdateStockInHand(op.CorrelationId, ap, op.Amount),
                    _ => product
                },
            BaseProductEvent.PriceChanged op
                => product switch
                {
                    BaseProduct.ActiveProduct ap
                        => UpdateProductPrice(op.CorrelationId, ap, op.Price),
                    _ => product
                },
            BaseProductEvent.ProductRemoved op
                => product switch
                {
                    BaseProduct.ActiveProduct _ => RemoveProduct(op.CorrelationId, op.Id),
                    _ => product
                },
            _ => throw new NotSupportedException(),
        };

    private static BaseProduct.ActiveProduct ActivateProduct(
        Guid correlationId,
        string id,
        string name,
        int stockInHand,
        decimal price
    )
    {
        Console.WriteLine($"{correlationId} adding product");
        return new BaseProduct.ActiveProduct(id, name, stockInHand, price);
    }

    private static BaseProduct.ActiveProduct UpdateProductPrice(
        Guid correlationId,
        BaseProduct.ActiveProduct ap,
        decimal price
    )
    {
        Console.WriteLine($"{correlationId} updated the stock price {price}");
        return ap with { Price = price };
    }

    private static BaseProduct UpdateStockInHand(
        Guid correlationId,
        BaseProduct.ActiveProduct ap,
        int amount
    )
    {
        Console.WriteLine($"{correlationId} updated product with amount {amount}");
        var updatedStockInHand = ap.StockInHand + amount;
        if (updatedStockInHand <= 0)
        {
            return new BaseProduct.RestockRequiredProduct(ap.Id, ap.Name, ap.Price);
        }

        return ap with
        {
            StockInHand = updatedStockInHand
        };
    }

    private static BaseProduct UpdateStockInHand(
        Guid correlationId,
        BaseProduct.RestockRequiredProduct ap,
        int amount
    )
    {
        if (amount <= 0)
        {
            return ap;
        }

        Console.WriteLine(
            $"{correlationId} sufficient stock received, this product is active to trade now"
        );
        
        return new BaseProduct.ActiveProduct(ap.Id, ap.Name, amount, ap.Price);
    }

    private static BaseProduct RemoveProduct(Guid correlationId, string productId)
    {
        Console.WriteLine($"{correlationId} deleted product {productId}");
        return new BaseProduct.DeletedProduct(productId);
    }
}