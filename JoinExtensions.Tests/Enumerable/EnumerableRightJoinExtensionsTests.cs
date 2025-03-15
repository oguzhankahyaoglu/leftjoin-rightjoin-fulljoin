namespace JoinExtensions.Tests;

using System.Linq;
using Shouldly;
using Xunit;
using JoinExtensions.Enumerable;

public class EnumerableRightJoinExtensionsTests
{
    [Fact]
    public void RightJoinExtEnumerable_ReturnsAllRightItems_WithNoMatches()
    {
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" }
        };

        var right = new[]
        {
            new { Id = 3, Value = "c" },
            new { Id = 4, Value = "d" }
        };

        var result = EnumerableRightJoinExtensions.RightJoinExtEnumerable(
            left,
            right,
            l => l.Id,
            r => r.Id,
            (l, r) => new { Left = l, Right = r }
        ).ToList();

        result.Count.ShouldBe(2);
        result.ForEach(item =>
        {
            item.Left.ShouldBeNull();
            item.Right.ShouldNotBeNull();
        });
    }

    [Fact]
    public void RightJoinExtEnumerable_ReturnsMatchedAndUnmatchedRightPairs()
    {
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" }
        };

        var right = new[]
        {
            new { Id = 1, Value = "x" },
            new { Id = 3, Value = "y" }
        };

        var result = EnumerableRightJoinExtensions.RightJoinExtEnumerable(
            left,
            right,
            l => l.Id,
            r => r.Id,
            (l, r) => new { Left = l, Right = r }
        ).ToList();

        result.Count.ShouldBe(2);

        var matched = result.First(x => x.Right.Id == 1);
        matched.Left.ShouldNotBeNull();
        matched.Left.Value.ShouldBe("a");

        var unmatched = result.First(x => x.Right.Id == 3);
        unmatched.Left.ShouldBeNull();
    }

    [Fact]
    public void RightJoinExtEnumerable_WithResultSelector_ReturnsTransformedResult()
    {
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" }
        };

        var right = new[]
        {
            new { Id = 1, Value = "x" },
            new { Id = 2, Value = "y" },
            new { Id = 3, Value = "z" }
        };

        var result = EnumerableRightJoinExtensions.RightJoinExtEnumerable(
            left,
            right,
            l => l.Id,
            r => r.Id,
            (l, r) => (l != null ? l.Value : "_") + r.Value
        ).ToList();

        result.Count.ShouldBe(3);
        result.ShouldContain("ax");
        result.ShouldContain("by");
        result.ShouldContain("_z");
    }
}