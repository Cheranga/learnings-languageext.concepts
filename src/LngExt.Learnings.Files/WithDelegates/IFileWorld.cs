namespace LngExt.Learnings.Files.WithDelegates;

public interface IFileWorld
{
    string ReadContent(string filePath);
    Either<Error, Unit> WriteContent(string filePath, string content);
}