using FluentAssertions;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.TypeClasses;
using LngExt.Monad.Tests.Models;

namespace LngExt.Monad.Tests;

public static class EitherTests
{
    [Fact]
    public static void Test()
    {
        var successOperation = Either<Error, Student>.Right(new Student("666", "Cheranga"));
        var errorOperation = Either<Error, Student>.Left(Error.New("customer not found"));

        successOperation
            .Map(x => new Employee(x.Id, x.Name.ToUpper(), "Melbourne"))
            .IfRight(employee => employee.Name.Should().Be("cheranga".ToUpper()));

        errorOperation
            .MapLeft(error => error)
            .IfLeft(error => error.Message.Should().Be("customer not found"));
    }
}
