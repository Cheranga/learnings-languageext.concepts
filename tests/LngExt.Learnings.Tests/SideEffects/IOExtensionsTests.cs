namespace LngExt.Learnings.Tests.SideEffects;

public class IOExtensionsTests
{
    [Fact]
    public void ReadingAnUnExistingFile()
    {
        var input = "TestData/blah.json";

        var operation = (from content in FileOperations.ReadAllText(input) select content).Run();

        operation.IsLeft.Should().BeTrue();
        operation.IfLeft(error =>
        {
            error.ToException().Should().BeOfType<FileNotFoundException>();
            error.Code.Should().Be(500);
            error.Message.Should().Be("error when running the IO operation");
        });
    }

    [Fact]
    public void ReadingAnExistingFile()
    {
        var input = "TestData/input.csv";

        var operation = (from content in FileOperations.ReadAllText(input) select content).Run();

        operation.IsRight.Should().BeTrue();
        operation.IfRight(content => content.Should().NotBeNullOrEmpty());
    }

    [Fact]
    public void CombineReadAndWriteWithExistingFiles()
    {
        var input = "TestData/input.csv";
        var output = "TestData/output.csv";

        var operation = (
            from a in FileOperations.ReadAllText(input)
            from _ in FileOperations.WriteAllText(output, a)
            select unit
        ).Run();

        operation.IsRight.Should().BeTrue();
    }

    [Fact]
    public void CombineReadAndWriteWithUnExistingFiles()
    {
        var input = "TestData/blah.csv";
        var output = "TestData/output.csv";

        var operation = (
            from a in FileOperations.ReadAllText(input)
            from _ in FileOperations.WriteAllText(output, a)
            select unit
        ).Run();

        operation.IsLeft.Should().BeTrue();
        operation.IfLeft(error => error.ToException().Should().BeOfType<FileNotFoundException>());
    }

    [Fact]
    public void GetCharacterCount()
    {
        var input = "TestData/input.csv";
        var output = "TestData/output.csv";

        var operation = (
            from i in FileOperations.ReadAllText(input)
            from _ in FileOperations.WriteAllText(output, i)
            from o in FileOperations.ReadAllText(output)
            select o
        )
            .Map(s => s.Length)
            .Run();

        operation.IsRight.Should().BeTrue();
        operation.IfRight(i => i.Should().BeGreaterThan(1));
    }
}
