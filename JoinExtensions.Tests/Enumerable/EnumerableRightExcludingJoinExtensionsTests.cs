using Shouldly;
using JoinExtensions.Enumerable;

namespace JoinExtensions.Tests;

public class EnumerableRightExcludingJoinExtensionsTests
{
    [Fact]
    public void RightExcludingJoin_ReturnsAllRight_WhenLeftIsEmpty()
    {
        var left = new[] { new { Id = 0, Value = "" } }.Where(x => false);
        var right = new[]
        {
            new { Id = 1, Value = "x" },
            new { Id = 2, Value = "y" }
        };

        var result = EnumerableRightExcludingJoinExtensions.RightExcludingJoinExtEnumerable(
            left,
            right,
            l => l.Id,
            r => r.Id,
            (l, r) => r.Value
        ).ToList();

        result.Count.ShouldBe(2);
        result.ShouldContain("x");
        result.ShouldContain("y");
    }

    [Fact]
    public void RightExcludingJoin_ReturnsEmpty_WhenAllRightMatch()
    {
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" }
        };

        var right = new[]
        {
            new { Id = 1, Value = "x" },
            new { Id = 2, Value = "y" }
        };

        var result = EnumerableRightExcludingJoinExtensions.RightExcludingJoinExtEnumerable(
            left,
            right,
            l => l.Id,
            r => r.Id,
            (l, r) => r.Value
        ).ToList();

        result.ShouldBeEmpty();
    }

    [Fact]
    public void RightExcludingJoin_ReturnsOnlyNonMatchingRight()
    {
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" }
        };

        var right = new[]
        {
            new { Id = 1, Value = "x" },
            new { Id = 3, Value = "c" }
        };

        var result = EnumerableRightExcludingJoinExtensions.RightExcludingJoinExtEnumerable(
            left,
            right,
            l => l.Id,
            r => r.Id,
            (l, r) => r.Value
        ).ToList();

        result.Count.ShouldBe(1);
        result.ShouldContain("c");
    }
}