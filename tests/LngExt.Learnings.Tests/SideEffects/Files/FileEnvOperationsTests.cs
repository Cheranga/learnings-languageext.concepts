using LngExt.Learnings.Files;

namespace LngExt.Learnings.Tests.SideEffects.Files;

public class TestFileEnvironment : IFileWorld
{
    public string ReadContent(string filePath) => File.ReadAllText(filePath);

    public Either<Error, Unit> WriteContent(string filePath, string content) =>
        Try(() =>
            {
                File.WriteAllText(filePath, content);
                return unit;
            })
            .ToEither(exception => Error.New(500, "error when writing to file", exception));
}

public class FileEnvOperationsTests
{
    [Fact]
    public void ReadFromUnExistingFile()
    {
        var operation = (
            from content in FileEnvOperations.ReadFileContent<IFileWorld>("blah.json")
            select content
        ).Run(new TestFileEnvironment());
        operation.IsLeft.Should().BeTrue();
        operation.IfLeft(error => error.ToException().Should().BeOfType<FileNotFoundException>());
    }

    [Fact]
    public void ReadFromExistingFile()
    {
        var operation = (
            from content in FileEnvOperations.ReadFileContent<IFileWorld>("TestData/input.csv")
            select content
        ).Run(new TestFileEnvironment());
        operation.IsRight.Should().BeTrue();
        operation.IfRight(s => s.Should().NotBeNullOrEmpty());
    }

    [Fact]
    public void WriteToFile()
    {
        var operation = (
            from _ in FileEnvOperations.WriteToFile<IFileWorld>("TestData/output.csv", "some content")
            select _
        ).Run(new TestFileEnvironment());
        operation.IsRight.Should().BeTrue();
    }

    [Fact]
    public void CombineReadAndWriteWithExistingFiles()
    {
        var input = "TestData/input.csv";
        var output = "TestData/output.csv";

        var operation = (
            from a in FileEnvOperations.ReadFileContent<IFileWorld>(input)
            from _ in FileEnvOperations.WriteToFile<IFileWorld>(output, a)
            select unit
        ).Run(new TestFileEnvironment());

        operation.IsRight.Should().BeTrue();
    }

    [Fact]
    public void CombineReadAndWriteWithUnExistingFiles()
    {
        var input = "TestData/blah.csv";
        var output = "TestData/output.csv";

        var operation = (
            from a in FileEnvOperations.ReadFileContent<IFileWorld>(input)
            from _ in FileEnvOperations.WriteToFile<IFileWorld>(output, a)
            select unit
        ).Run(new TestFileEnvironment());

        operation.IsLeft.Should().BeTrue();
        operation.IfLeft(error => error.ToException().Should().BeOfType<FileNotFoundException>());
    }

    [Fact]
    public void GetCharacterCount()
    {
        var input = "TestData/input.csv";
        var output = "TestData/output.csv";

        var operation = (
            from i in FileEnvOperations.ReadFileContent<IFileWorld>(input)
            from _ in FileEnvOperations.WriteToFile<IFileWorld>(output, i)
            from o in FileEnvOperations.ReadFileContent<IFileWorld>(output)
            select o
        )
            .Map(s => s.Length)
            .Run(new TestFileEnvironment());

        operation.IsRight.Should().BeTrue();
        operation.IfRight(i => i.Should().BeGreaterThan(1));
    }
}
