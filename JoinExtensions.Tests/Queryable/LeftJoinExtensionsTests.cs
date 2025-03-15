using Shouldly;
using JoinExtensions.Queryable;

namespace JoinExtensions.Tests.Queryable;

public class LeftJoinExtensionsTests
{
    [Fact]
    public void LeftJoin_ReturnsAllLeftItems_WhenRightIsEmpty()
    {
        // Arrange
        var left = new[]
        {
            new { Id = 1, Value = "A" },
            new { Id = 2, Value = "B" }
        };
        var right = new object[0]; // empty right sequence

        // Act
        var result = LeftJoinExtensions.LeftJoinExt(
            left.AsQueryable(),
            right.AsQueryable(),
            l => l.Id,
            r => default
        ).ToList();

        // Assert
        result.Count.ShouldBe(2);
        result.ForEach(item =>
        {
            item.Left.ShouldNotBeNull();
            item.Right.ShouldBeNull();
        });
    }

    [Fact]
    public void LeftJoin_ReturnsMatchingPair_WhenAllLeftItemsHaveMatches()
    {
        // Arrange
        var left = new[]
        {
            new { Id = 1, Value = "A" },
            new { Id = 2, Value = "B" }
        };
        var right = new[]
        {
            new { Id = 1, Value = "X" },
            new { Id = 2, Value = "Y" }
        };

        // Act
        var result = LeftJoinExtensions.LeftJoinExt(
            left.AsQueryable(),
            right.AsQueryable(),
            l => l.Id,
            r => r.Id
        ).ToList();

        // Assert
        result.Count.ShouldBe(2);
        result.ForEach(item =>
        {
            item.Left.ShouldNotBeNull();
            item.Right.ShouldNotBeNull();
            item.Left.Id.ShouldBe(item.Right.Id);
        });
    }

    [Fact]
    public void LeftJoin_ReturnsLeftItemsWithNullRight_WhenNoMatchFound()
    {
        // Arrange
        var left = new[]
        {
            new { Id = 1, Value = "A" },
            new { Id = 2, Value = "B" },
            new { Id = 3, Value = "C" }
        };
        var right = new[]
        {
            new { Id = 2, Value = "Y" }
        };

        // Act
        var result = LeftJoinExtensions.LeftJoinExt(
            left.AsQueryable(),
            right.AsQueryable(),
            l => l.Id,
            r => r.Id
        ).ToList();

        // Assert
        result.Count.ShouldBe(3);
        result.ForEach(item =>
        {
            item.Left.ShouldNotBeNull();
            if (item.Left.Id == 2)
            {
                item.Right.ShouldNotBeNull();
                item.Right.Id.ShouldBe(2);
            }
            else
            {
                item.Right.ShouldBeNull();
            }
        });
    }

    [Fact]
    public void LeftJoin_ReturnsMultipleRows_ForMultipleRightMatches()
    {
        // Arrange
        var left = new[]
        {
            new { Id = 1, Value = "A" },
            new { Id = 2, Value = "B" }
        };
        var right = new[]
        {
            new { Id = 1, Value = "X" },
            new { Id = 1, Value = "Y" }
        };

        // Act
        var result = LeftJoinExtensions.LeftJoinExt(
            left.AsQueryable(),
            right.AsQueryable(),
            l => l.Id,
            r => r.Id
        ).ToList();

        // Assert
        // Left with Id = 1 has 2 matches; left with Id = 2 has no match.
        result.Count.ShouldBe(3);
        var left1Items = result.Where(item => item.Left.Id == 1).ToList();
        left1Items.Count.ShouldBe(2);
        left1Items.ForEach(item =>
        {
            item.Left.ShouldNotBeNull();
            item.Right.ShouldNotBeNull();
            item.Right.Id.ShouldBe(1);
        });

        var left2Items = result.Where(item => item.Left.Id == 2).ToList();
        left2Items.Count.ShouldBe(1);
        left2Items.ForEach(item => item.Right.ShouldBeNull());
    }
}