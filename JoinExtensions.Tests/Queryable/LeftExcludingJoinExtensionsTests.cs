using Shouldly;
using JoinExtensions.Queryable;

namespace JoinExtensions.Tests.Queryable;

public class LeftExcludingJoinExtensionsTests
{
    [Fact]
    public void LeftExcludingJoin_ReturnsAllLeftItems_WhenRightIsEmpty()
    {
        // Arrange
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" },
            new { Id = 3, Value = "c" }
        };
        var right = new object[0];

        // Act
        var result = LeftExcludingJoinExtensions.LeftExcludingJoinExt(
            left.AsQueryable(),
            right.AsQueryable(),
            l => l.Id,
            r => default
        ).ToList();

        // Assert
        result.Count.ShouldBe(3);
        result.ForEach(item =>
        {
            item.Left.ShouldNotBeNull();
            item.Right.ShouldBeNull();
        });
    }

    [Fact]
    public void LeftExcludingJoin_ReturnsOnlyNonMatchingLeftItems()
    {
        // Arrange
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" },
            new { Id = 3, Value = "c" }
        };
        var right = new[]
        {
            new { Id = 2, Value = "x" }
        };

        // Act
        var result = LeftExcludingJoinExtensions.LeftExcludingJoinExt(
            left.AsQueryable(),
            right.AsQueryable(),
            l => l.Id,
            r => r.Id
        ).ToList();

        // Assert
        // Expecting left items with Id = 1 and Id = 3 only.
        result.Count.ShouldBe(2);
        result.Any(i => ((dynamic)i.Left).Id == 1).ShouldBeTrue();
        result.Any(i => ((dynamic)i.Left).Id == 3).ShouldBeTrue();
        result.ForEach(item =>
        {
            item.Left.ShouldNotBeNull();
            item.Right.ShouldBeNull();
        });
    }

    [Fact]
    public void LeftExcludingJoin_ReturnsEmpty_WhenAllLeftItemsHaveMatches()
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
            new { Id = 2, Value = "y" }
        };

        // Act
        var result = LeftExcludingJoinExtensions.LeftExcludingJoinExt(
            left.AsQueryable(),
            right.AsQueryable(),
            l => l.Id,
            r => r.Id
        ).ToList();

        // Assert
        result.Count.ShouldBe(0);
    }
}