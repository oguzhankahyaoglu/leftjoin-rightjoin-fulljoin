using Shouldly;
using JoinExtensions.Queryable;

namespace JoinExtensions.Tests.Queryable;

public class FullOuterJoinExtensionsTests
{
    [Fact]
    public void FullOuterJoin_ReturnsAllItems_WhenNoMatches()
    {
        // Arrange
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

        // Act
        var result = FullOuterJoinExtensions.FullOuterJoinExt(
            left.AsQueryable(),
            right.AsQueryable(),
            l => l.Id,
            r => r.Id
        ).ToList();

        // Assert
        result.Count.ShouldBe(4);
        result.Count(x => x.Left != null).ShouldBe(2);
        result.Count(x => x.Right != null).ShouldBe(2);
    }

    [Fact]
    public void FullOuterJoin_ReturnsBothJoinPairs_WhenMatchesExist()
    {
        // Arrange
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

        // Act
        var result = FullOuterJoinExtensions.FullOuterJoinExt(
            left.AsQueryable(),
            right.AsQueryable(),
            l => l.Id,
            r => r.Id
        ).ToList();

        // Assert
        // For the matching key (Id = 1), two join pairs are produced (one from each join).
        result.Count.ShouldBe(4);
        result.Count(x => x.Left != null && x.Right != null).ShouldBe(2);
        result.Count(x => x.Left != null && x.Left.Id == 2).ShouldBe(1);
        result.Count(x => x.Right != null && x.Right.Id == 3).ShouldBe(1);
    }
}