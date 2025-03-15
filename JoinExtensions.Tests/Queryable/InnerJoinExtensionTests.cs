using Shouldly;
using JoinExtensions.Queryable;

namespace JoinExtensions.Tests.Queryable;

public class InnerJoinExtensionsTests
{
    [Fact]
    public void InnerJoin_ReturnsOnlyMatchingPairs()
    {
        // Arrange
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" },
            new { Id = 3, Value = "c" }
        }.AsQueryable();

        var right = new[]
        {
            new { Id = 1, Info = "x" },
            new { Id = 3, Info = "y" }
        }.AsQueryable();

        // Act
        var result = InnerJoinExtensions.InnerJoinExt(left, right, l => l.Id, r => r.Id).ToList();

        // Assert
        // Expecting two join items: for Id = 1 and Id = 3.
        result.Count.ShouldBe(2);
        result.ForEach(item =>
        {
            item.Left.ShouldNotBeNull();
            item.Right.ShouldNotBeNull();
            (item.Left.Id == item.Right.Id).ShouldBeTrue();
        });
    }

    [Fact]
    public void InnerJoin_ReturnsEmpty_WhenNoKeysMatch()
    {
        // Arrange
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 2, Value = "b" }
        }.AsQueryable();

        var right = new[]
        {
            new { Id = 3, Info = "x" },
            new { Id = 4, Info = "y" }
        }.AsQueryable();

        // Act
        var result = InnerJoinExtensions.InnerJoinExt(left, right, l => l.Id, r => r.Id).ToList();

        // Assert
        result.Count.ShouldBe(0);
    }

    [Fact]
    public void InnerJoin_ReturnsMultipleMatches()
    {
        // Arrange
        var left = new[]
        {
            new { Id = 1, Value = "a" },
            new { Id = 1, Value = "b" },
            new { Id = 2, Value = "c" }
        }.AsQueryable();

        var right = new[]
        {
            new { Id = 1, Info = "x" },
            new { Id = 2, Info = "y" },
            new { Id = 2, Info = "z" }
        }.AsQueryable();

        // Act
        var result = InnerJoinExtensions.InnerJoinExt(left, right, l => l.Id, r=> r.Id).ToList();

        // Assert
        // Expecting:
        // For Id = 1: two join items (each left matches one right) if joined by multiple combinations
        // For Id = 2: two join items (one left combining with two right items)
        // Actual behavior depends on LeftJoinExt implementation, but InnerJoin filters out non-matching pairs.
        // Here we assume that LeftJoinExt creates one join item per matching pair.
        var expectedCount = (left.Count(item => item.Id == 1) * right.Count(item => item.Id == 1)) +
                            (left.Count(item => item.Id == 2) * right.Count(item => item.Id == 2));
        result.Count.ShouldBe(expectedCount);

        result.ForEach(item =>
        {
            item.Left.ShouldNotBeNull();
            item.Right.ShouldNotBeNull();
            (item.Left.Id == item.Right.Id).ShouldBeTrue();
        });
    }
}