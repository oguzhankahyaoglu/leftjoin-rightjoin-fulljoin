namespace JoinExtensions.Tests;

using System.Linq;
using Xunit;
using JoinExtensions.Enumerable;

public class EnumerableLeftJoinExtensionsTests
{
    [Fact]
    public void LeftJoinExtEnumerable_ReturnsAllLeftItems_WithNoMatches()
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

        var result = EnumerableLeftJoinExtensions.LeftJoinExtEnumerable(
            left,
            right,
            l => l.Id,
            r => r.Id
        ).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, item => Assert.Null(item.Right));
    }

    [Fact]
    public void LeftJoinExtEnumerable_ReturnsMatchedAndUnmatchedPairs()
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

        var result = EnumerableLeftJoinExtensions.LeftJoinExtEnumerable(
            left,
            right,
            l => l.Id,
            r => r.Id
        ).ToList();

        Assert.Equal(2, result.Count);
        // For left Id=1, right should be non-null, for left Id=2, right should be null.
        var match = result.First(item => item.Left.Id == 1);
        Assert.NotNull(match.Right);
        Assert.Equal("x", match.Right.Value);

        var noMatch = result.First(item => item.Left.Id == 2);
        Assert.Null(noMatch.Right);
    }

    [Fact]
    public void LeftJoinExtEnumerable_WithResultSelector_ReturnsTransformedResult()
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

        var result = EnumerableLeftJoinExtensions.LeftJoinExtEnumerable(
            left,
            right,
            l => l.Id,
            r => r.Id,
            (l, r) => l.Value + (r != null ? r.Value : "_")
        ).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains("ax", result);
        Assert.Contains("by", result);
    }
}