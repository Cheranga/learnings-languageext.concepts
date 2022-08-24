using System.Net;

namespace LngExt.Learnings.Tests.SideEffects;

public static class FileOperations
{
    public static IO<string> ReadAllText(string filePath) => () => File.ReadAllText(filePath);

    // () =>
    //     Try(() => File.ReadAllText(filePath))
    //         .ToEither(exception => Error.New(500, "error when reading file", exception));

    public static IO<Unit> WriteAllText(string filePath, string content) =>
        () =>
        {
            File.WriteAllText(filePath, content);
            return unit;
        };
    // () =>
    //     Try(() =>
    //         {
    //             File.WriteAllText(filePath, content);
    //             return unit;
    //         })
    //         .ToEither(exception => Error.New(500, "error when writing file", exception));
}
