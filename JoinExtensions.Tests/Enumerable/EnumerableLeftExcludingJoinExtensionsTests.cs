using JoinExtensions.Enumerable;
using Shouldly;

namespace JoinExtensions.Tests;

public class EnumerableLeftExcludingJoinExtensionsTests
{
    [Fact]
    public void LeftExcludingJoinExtEnumerable_ReturnsAllLeft_WhenRightIsEmpty()
    {
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" }
        };

        var right = left.Where(x => false);

        var result = EnumerableLeftExcludingJoinExtensions.LeftExcludingJoinExtEnumerable(
            left,
            right,
            l => l.Id,
            r => r.Id,
            (l, r) => l.Value
        ).ToList();

        result.Count.ShouldBe(2);
        result.ShouldContain("a");
        result.ShouldContain("b");
    }

    [Fact]
    public void LeftExcludingJoinExtEnumerable_ReturnsEmpty_WhenAllLeftMatch()
    {
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" }
        };

        var right = new[]
        {
            new { Id = 1, Value = "c" },
            new { Id = 2, Value = "d" }
        };

        var result = EnumerableLeftExcludingJoinExtensions.LeftExcludingJoinExtEnumerable(
            left,
            right,
            l => l.Id,
            r => r.Id,
            (l, r) => l.Value
        ).ToList();

        result.ShouldBeEmpty();
    }

    [Fact]
    public void LeftExcludingJoinExtEnumerable_ReturnsOnlyNonMatchingLeft()
    {
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" },
            new { Id = 3, Value = "c" }
        };

        var right = new[]
        {
            new { Id = 1, Value = "x" },
            new { Id = 4, Value = "y" }
        };

        var result = EnumerableLeftExcludingJoinExtensions.LeftExcludingJoinExtEnumerable(
            left,
            right,
            l => l.Id,
            r => r.Id,
            (l, r) => l.Value
        ).ToList();

        result.Count.ShouldBe(2);
        result.ShouldContain("b");
        result.ShouldContain("c");
    }
}