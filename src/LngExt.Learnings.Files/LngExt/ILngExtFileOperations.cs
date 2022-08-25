namespace LngExt.Learnings.Files.LngExt;

public interface ILngExtFileOperations
{
    Aff<string> ReadContentAsync(string filePath);
    Aff<Unit> WriteContentAsync(string filePath, string content);
}

public class LngExtFileOperations : ILngExtFileOperations
{
    public Aff<string> ReadContentAsync(string filePath) =>
        AffMaybe<string>(async () => await File.ReadAllTextAsync(filePath))
            .MapFail(error => Error.New(500, "error when reading file", error.ToException()));

    public Aff<Unit> WriteContentAsync(string filePath, string content) =>
        AffMaybe<Unit>(async () =>
            {
                await File.WriteAllTextAsync(filePath, content);
                return unit;
            })
            .MapFail(error => Error.New(501, "error when writing content", error));
}
