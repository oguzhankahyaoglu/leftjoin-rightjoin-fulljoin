namespace JoinExtensions.Tests;

using JoinExtensions.Enumerable;
using System.Linq;
using Xunit;

public class EnumerableInnerJoinExtensionsTests
{
    [Fact]
    public void InnerJoinExtEnumerable_ReturnsEmpty_WhenNoMatches()
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

        var result = EnumerableInnerJoinExtensions.InnerJoinExtEnumerable(
            left, right, l => l.Id, r => r.Id).ToList();
        Assert.Empty(result);
    }

    [Fact]
    public void InnerJoinExtEnumerable_ReturnsMatches_WithJoinItem()
    {
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" }
        };

        var right = new[]
        {
            new { Id = 1, Value = "c" },
            new { Id = 3, Value = "d" }
        };

        var result = EnumerableInnerJoinExtensions.InnerJoinExtEnumerable(
            left, right, l => l.Id, r => r.Id).ToList();
        Assert.Single(result);
        Assert.Equal(1, result[0].Left.Id);
        Assert.Equal("a", result[0].Left.Value);
        Assert.Equal(1, result[0].Right.Id);
        Assert.Equal("c", result[0].Right.Value);
    }

    [Fact]
    public void InnerJoinExtEnumerable_ReturnsMatches_WithResultFunc()
    {
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" }
        };

        var right = new[]
        {
            new { Id = 1, Value = "c" },
            new { Id = 2, Value = "d" },
            new { Id = 3, Value = "e" }
        };

        var result = EnumerableInnerJoinExtensions.InnerJoinExtEnumerable(
            left, right,
            l => l.Id,
            r => r.Id,
            (l, r) => l.Value + r.Value
        ).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains("ac", result);
        Assert.Contains("bd", result);
    }
}