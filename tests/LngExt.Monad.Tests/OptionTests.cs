using FluentAssertions;
using LanguageExt;
using LngExt.Monad.Tests.Models;
using static LanguageExt.Prelude;

namespace LngExt.Monad.Tests;

public class OptionTests
{
    [Fact]
    public void NullObjectMustBeNone()
    {
        Func<Student, Option<Employee>> toEmployee = s =>
            Optional(new Employee(s.Id, s.Name, "Melbourne"));

        Student student = null;

        var operation = from a in Optional(student) from b in toEmployee(a) select b;

        operation.IsNone.Should().BeTrue();
    }

    [Fact]
    public void NonNullObjectMustBeSome()
    {
        Func<Student, Option<Employee>> toEmployee = s =>
            Optional(new Employee(s.Id, s.Name, "Melbourne"));

        var student = new Student("666", "Cheranga");

        var operation = from a in Optional(student) from b in toEmployee(a) select b;

        operation.IsSome.Should().BeTrue();
        operation.IfSome(employee =>
        {
            employee.Id.Should().Be("666");
            employee.Name.Should().Be("Cheranga");
            employee.Address.Should().Be("Melbourne");
        });
    }
}
