namespace LngExt.Learnings.Tests.SideEffects;

public delegate Either<Error, A> IO<A>();
public static class IOOperations
{
    public static IO<string> ReadAllText(string filePath) =>
        () =>
            Try(() => File.ReadAllText(filePath))
                .ToEither(exception => Error.New(500, "error when reading file", exception));

    public static IO<Unit> WriteAllText(string filePath, string content) =>
        () =>
            Try(() =>
                {
                    File.WriteAllText(filePath, content);
                    return unit;
                })
                .ToEither(exception => Error.New(500, "error when writing file", exception));
}

public class IOOperationTests
{
    [Fact]
    public void FileDoesNotExist()
    {
        var operation = IOOperations.ReadAllText("blah.json")();
        operation.IsLeft.Should().BeTrue();
        operation.IfLeft(error =>
        {
            error.Should().NotBeNull("error must not be null");
            error.Code.Should().Be(500, "error code must be 500");
            error.Exception.IsSome.Should().BeTrue("there must be an exception");
            error.Exception.IfSome(e => e.Should().BeOfType<FileNotFoundException>($"exception must be of type {nameof(FileNotFoundException)}"));
        });
    }
}
