namespace LngExt.Learnings.Tests.SideEffects;

public class FileOperationsTests
{
    [Fact]
    public void FileDoesNotExist()
    {
        Assert.Throws<FileNotFoundException>(() => FileOperations.ReadAllText("blah.json")());


        // operation.IsLeft.Should().BeTrue();
        // operation.IfLeft(error =>
        // {
        //     error.Should().NotBeNull("error must not be null");
        //     error.Code.Should().Be(500, "error code must be 500");
        //     error.Exception.IsSome.Should().BeTrue("there must be an exception");
        //     error.Exception.IfSome(
        //         e =>
        //             e.Should()
        //                 .BeOfType<FileNotFoundException>(
        //                     $"exception must be of type {nameof(FileNotFoundException)}"
        //                 )
        //     );
        // });
    }
}