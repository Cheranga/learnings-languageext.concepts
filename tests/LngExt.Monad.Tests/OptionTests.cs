using FluentAssertions;
using LanguageExt;
using static LanguageExt.Prelude;

namespace LngExt.Monad.Tests;

public record Student(string Id, string Name);

public record Employee(string Id, string Name, string Address);

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
            employee.Name.Should().Be("Cheranga");
            employee.Address.Should().Be("Melbourne");
        });
    }
}
