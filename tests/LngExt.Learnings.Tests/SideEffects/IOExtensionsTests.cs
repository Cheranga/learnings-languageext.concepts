namespace LngExt.Learnings.Tests.SideEffects;

public class IOExtensionsTests
{
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
    public void CombineReadAndWriteWithUnexistingFiles()
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
            from a in FileOperations.ReadAllText(input)
            from _ in FileOperations.WriteAllText(output, a)
            select a
        )
            .Select(s => s.Length)
            .Run();

        operation.IsRight.Should().BeTrue();
        operation.IfRight(i => i.Should().BeGreaterThan(1));
    }
}
