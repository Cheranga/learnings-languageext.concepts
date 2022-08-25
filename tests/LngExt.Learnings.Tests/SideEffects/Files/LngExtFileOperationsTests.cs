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
}
