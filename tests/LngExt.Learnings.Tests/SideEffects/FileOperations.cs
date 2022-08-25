namespace LngExt.Learnings.Tests.SideEffects;

public static class FileOperations
{
    public static IO<string> ReadAllText(string filePath) => () => File.ReadAllText(filePath);

    public static IO<Unit> WriteAllText(string filePath, string content) =>
        () =>
        {
            File.WriteAllText(filePath, content);
            return unit;
        };
}