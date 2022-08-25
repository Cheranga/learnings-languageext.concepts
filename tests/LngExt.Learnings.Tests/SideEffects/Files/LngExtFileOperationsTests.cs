using LngExt.Learnings.Files.LngExt;

namespace LngExt.Learnings.Tests.SideEffects.Files;

public class LngExtFileOperationsTests
{
    [Fact]
    public async Task ErrorWhenReadingUnExistingFile()
    {
        var service = new LngExtFileOperations();
        var operation = await (
            from content in service.ReadContentAsync("blah.json")
            select content
        ).Run();

        operation.IsFail.Should().BeTrue();
        operation.IfFail(error =>
        {
            error.ToException().Should().BeOfType<FileNotFoundException>();
            error.Code.Should().Be(500);
            error.Message.Should().Be("error when reading file");
        });
    }

    [Fact]
    public async Task ReadFromExistingFile()
    {
        var service = new LngExtFileOperations();
        var operation = await (
            from content in service.ReadContentAsync("TestData/input.csv")
            select content
        ).Run();
        operation.IsSucc.Should().BeTrue();
        operation.IfSucc(s => s.Should().NotBeNullOrEmpty());
    }

    [Fact]
    public async Task WriteToFile()
    {
        var service = new LngExtFileOperations();
        var operation = await (
            from _ in service.WriteContentAsync("TestData/output.csv", "some content")
            select _
        ).Run();
        operation.IsSucc.Should().BeTrue();
    }

    [Fact]
    public async Task CombineReadAndWriteWithExistingFiles()
    {
        var service = new LngExtFileOperations();
        var input = "TestData/input.csv";
        var output = "TestData/randomoutput.csv";

        var operation = await (
            from a in service.ReadContentAsync(input)
            from _ in service.WriteContentAsync(output, a)
            select unit
        ).Run();

        operation.IsSucc.Should().BeTrue();
    }

    [Fact]
    public async Task CombineReadAndWriteWithUnExistingFiles()
    {
        var service = new LngExtFileOperations();
        var input = "TestData/blah.csv";
        var output = "TestData/output.csv";

        var operation = await (
            from a in service.ReadContentAsync(input)
            from _ in service.WriteContentAsync(output, a)
            select unit
        ).Run();

        operation.IsFail.Should().BeTrue();
        operation.IfFail(error => error.ToException().Should().BeOfType<FileNotFoundException>());
    }

    [Fact]
    public async Task GetCharacterCount()
    {
        var service = new LngExtFileOperations();
        var input = "TestData/input.csv";
        var output = "TestData/random.csv";

        var operation = await (
            from i in service.ReadContentAsync(input)
            from _ in service.WriteContentAsync(output, i)
            from o in service.ReadContentAsync(output)
            select o
        )
            .Map(s => s.Length)
            .Run();

        operation.IsSucc.Should().BeTrue();
        operation.IfSucc(i => i.Should().BeGreaterThan(1));
    }
}
