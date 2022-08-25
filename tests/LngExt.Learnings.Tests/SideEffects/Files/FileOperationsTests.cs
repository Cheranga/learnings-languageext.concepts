using LngExt.Learnings.Files;

namespace LngExt.Learnings.Tests.SideEffects.Files;

public class FileOperationsTests
{
    [Fact]
    public void ReadFromUnExistingFile()
    {
        var operation = (
            from content in FileOperations.ReadAllText("blah.json")
            select content
        ).Run();
        operation.IsLeft.Should().BeTrue();
        operation.IfLeft(error => error.ToException().Should().BeOfType<FileNotFoundException>());
    }

    [Fact]
    public void ReadFromExistingFile()
    {
        var operation = (
            from content in FileOperations.ReadAllText("TestData/input.csv")
            select content
        ).Run();
        operation.IsRight.Should().BeTrue();
        operation.IfRight(s => s.Should().NotBeNullOrEmpty());
    }

    [Fact]
    public void WriteToFile()
    {
        var operation = (
            from _ in FileOperations.WriteAllText("TestData/output.csv", "some content")
            select _
        ).Run();
        operation.IsRight.Should().BeTrue();
    }

    [Fact]
    public void CombineReadAndWriteWithExistingFiles()
    {
        var input = "TestData/input.csv";
        var output = "TestData/randomoutput.csv";

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
        var output = "TestData/random.csv";

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
