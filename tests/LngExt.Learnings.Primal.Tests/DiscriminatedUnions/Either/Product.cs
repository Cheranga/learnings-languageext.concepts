namespace LngExt.Learnings.Primal.Tests.DiscriminatedUnions.Either;

// TODO: Add a stock in hand type, which can be (value + measurement)

public record Product(string Id, string Name, int StockInHand, decimal Price);

public abstract record BaseProduct
{
    private BaseProduct() { }

    public record EmptyProduct : BaseProduct;

    public record ActiveProduct(string Id, string Name, int StockInHand, decimal Price)
        : BaseProduct;

    public record RestockRequiredProduct(string Id, string Name, decimal Price) : BaseProduct;

    public record DeletedProduct(string Id) : BaseProduct;

    public (string, string, string) GetProductInfo() =>
        this switch
        {
            EmptyProduct p => ("Unassigned", string.Empty, string.Empty),
            ActiveProduct p => (nameof(ActiveProduct), p.Id, p.Name),
            DeletedProduct p => (nameof(DeletedProduct), p.Id, p.Id),
            _ => throw new ArgumentOutOfRangeException()
        };
}

public abstract record BaseProductOperationType
{
    private BaseProductOperationType() { }

    private TMap Map<TMap>(
        Func<AddProductOperation, TMap> addMapper,
        Func<UpdateProductOperation, TMap> updateMapper,
        Func<DeleteProductOperation, TMap> deleteMapper
    ) =>
        this switch
        {
            AddProductOperation op => addMapper(op),
            UpdateProductOperation op => updateMapper(op),
            DeleteProductOperation op => deleteMapper(op),
            _ => throw new NotSupportedException()
        };

    public sealed record AddProductOperation(string Id, string Name, decimal Price)
        : BaseProductOperationType;

    public sealed record UpdateProductOperation(string Id, string Name, decimal Price)
        : BaseProductOperationType;

    public sealed record DeleteProductOperation(string Id, string Reason)
        : BaseProductOperationType;
}
