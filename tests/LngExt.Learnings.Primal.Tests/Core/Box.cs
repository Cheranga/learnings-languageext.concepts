namespace LngExt.Learnings.Primal.Tests.Core;

public class Box<T>
{
    public Box(T data) => Data = data;

    public Box()
    {
    }

    public T Data { get; init; }
}