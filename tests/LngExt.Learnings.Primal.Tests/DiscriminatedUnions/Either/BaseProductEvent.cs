namespace LngExt.Learnings.Primal.Tests.DiscriminatedUnions.Either;

public abstract record BaseProductEvent
{
    private BaseProductEvent() { }

    public record ProductAdded(
        Guid CorrelationId,
        string ProductId,
        string Name,
        int StockInHand,
        decimal Price
    ) : BaseProductEvent;

    public record StockInHandUpdated(Guid CorrelationId, int Amount) : BaseProductEvent;

    public record PriceChanged(Guid CorrelationId, decimal Price) : BaseProductEvent;

    public record ProductRemoved(Guid CorrelationId, string Id) : BaseProductEvent;
}
