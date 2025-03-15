using Shouldly;
using JoinExtensions.Queryable;

namespace JoinExtensions.Tests.Queryable;

public class RightExcludingJoinExtensionsTests
{
    [Fact]
    public void RightExcludingJoin_ReturnsAllRightItems_WhenLeftIsEmpty()
    {
        // Arrange
        var left = new object[0];
        var right = new[]
        {
            new { Id = 1, Value = "A" },
            new { Id = 2, Value = "B" }
        };

        // Act
        var result = RightExcludingJoinExtensions.RightExcludingJoinExt(
            left.AsQueryable(),
            right.AsQueryable(),
            l => 0, // Using a dummy key selector as left is empty
            r => r.Id
        ).ToList();

        // Assert
        result.Count.ShouldBe(2);
        result.ForEach(item =>
        {
            item.Left.ShouldBeNull();
            item.Right.ShouldNotBeNull();
        });
    }

    [Fact]
    public void RightExcludingJoin_ReturnsOnlyNonMatchingRightItems()
    {
        // Arrange
        var left = new[]
        {
            new { Id = 1, Value = "L1" },
            new { Id = 3, Value = "L3" }
        };
        var right = new[]
        {
            new { Id = 1, Value = "R1" },
            new { Id = 2, Value = "R2" },
            new { Id = 3, Value = "R3" }
        };

        // Act
        var result = RightExcludingJoinExtensions.RightExcludingJoinExt(
            left.AsQueryable(),
            right.AsQueryable(),
            l => l.Id,
            r => r.Id
        ).ToList();

        // Assert
        // Only right item with Id = 2 does not have a match in left.
        result.Count.ShouldBe(1);
        result[0].Left.ShouldBeNull();
        result[0].Right.Id.ShouldBe(2);
    }

    [Fact]
    public void RightExcludingJoin_ReturnsEmpty_WhenAllRightItemsHaveMatches()
    {
        // Arrange
        var left = new[]
        {
            new { Id = 1, Value = "L1" },
            new { Id = 2, Value = "L2" }
        };
        var right = new[]
        {
            new { Id = 1, Value = "R1" },
            new { Id = 2, Value = "R2" }
        };

        // Act
        var result = RightExcludingJoinExtensions.RightExcludingJoinExt(
            left.AsQueryable(),
            right.AsQueryable(),
            l => l.Id,
            r => r.Id
        ).ToList();

        // Assert
        result.Count.ShouldBe(0);
    }
}