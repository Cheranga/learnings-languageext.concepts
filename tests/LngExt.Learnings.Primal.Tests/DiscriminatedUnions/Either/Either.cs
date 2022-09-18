namespace LngExt.Learnings.Primal.Tests.DiscriminatedUnions.Either;

public class Either<TA, TB, TC>
    where TA : BaseProductOperationType
    where TB : BaseProductOperationType
    where TC : BaseProductOperationType
{
    private readonly BaseProductOperationType _operation;

    private Either(BaseProductOperationType operation) => _operation = operation;

    public static Either<TA, TB, TC> New(TA operation) => new(operation);

    public static Either<TA, TB, TC> New(TB operation) => new(operation);

    public static Either<TA, TB, TC> New(TC operation) => new(operation);

    private TD Map<TD>(Func<TA, TD> taMapper, Func<TB, TD> tbMapper, Func<TC, TD> tcMapper) =>
        _operation switch
        {
            TA a => taMapper(a),
            TB b => tbMapper(b),
            TC c => tcMapper(c),
            _ => throw new NotSupportedException()
        };
}