using LanguageExt;
using LanguageExt.Common;

namespace LngExt.Learnings.Files;

public static class FileEnvOperations
{
    public static IOEnv<Env, string> ReadFileContent<Env>(string filePath) where Env : IFileWorld =>
        env => env.ReadContent(filePath);

    public static IOEnv<Env, Unit> WriteToFile<Env>(string filePath, string content)
        where Env : IFileWorld => env => env.WriteContent(filePath, content);
}
