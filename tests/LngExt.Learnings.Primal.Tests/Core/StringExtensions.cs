namespace LngExt.Learnings.Primal.Tests.Core;

public static class StringExtensions
{
    public static bool CompareWithoutCase(this string @this, string target) =>
        @this.Compare(target, StringComparison.OrdinalIgnoreCase);
    
    private static bool Compare(this string @this, string target, StringComparison comparison) =>
        string.Equals(@this, target, comparison);
}