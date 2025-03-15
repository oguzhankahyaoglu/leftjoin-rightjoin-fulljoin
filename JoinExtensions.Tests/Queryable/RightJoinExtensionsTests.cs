using Shouldly;
using JoinExtensions.Queryable;

namespace JoinExtensions.Tests.Queryable;

public class RightJoinExtensionsTests
{
    [Fact]
    public void RightJoin_ReturnsAllRightItems_WhenLeftIsEmpty()
    {
        // Arrange: left for RightJoin is empty and right is not empty.
        var left = new object[0];
        var right = new[]
        {
            new { Id = 1, Value = "R1" },
            new { Id = 2, Value = "R2" }
        }.AsQueryable();

        // Act: Call RightJoinExt without using extension syntax.
        var result = RightJoinExtensions.RightJoinExt(
            left.AsQueryable(), 
            right, 
            l => 0, // dummy selector since left is empty
            r => r.Id
        ).ToList();

        // Assert: every right item appears with a null match (mapped to Right property).
        result.Count.ShouldBe(2);
        result.ForEach(item =>
        {
            item.Left.ShouldBeNull();
            item.Right.ShouldNotBeNull();
        });
    }

    [Fact]
    public void RightJoin_ReturnsEmpty_WhenRightIsEmpty()
    {
        // Arrange: right sequence is empty.
        var left = new[]
        {
            new { Id = 1, Value = "L1" },
            new { Id = 2, Value = "L2" }
        }.AsQueryable();
        var right = new object[0].AsQueryable();

        // Act
        var result = RightJoinExtensions.RightJoinExt(left, right, 
                l => l.Id, r => default)
            .ToList();

        // Assert: no item is returned because the outer (right) sequence is empty.
        result.Count.ShouldBe(0);
    }

    [Fact]
    public void RightJoin_ReturnsMatchingPairs_WithUnmatchedRightItems()
    {
        // Arrange: mix of matching and non-matching items.
        var left = new[]
        {
            new { Id = 1, Value = "L1" }
        }.AsQueryable();

        var right = new[]
        {
            new { Id = 1, Value = "R1" },
            new { Id = 2, Value = "R2" }
        }.AsQueryable();

        // Act: perform right join in non-extension method syntax.
        var result = RightJoinExtensions.RightJoinExt(left, right, l => l.Id, r => r.Id)
            .ToList();

        // Assert:
        // For Id = 1, there should be a matching pair.
        // For Id = 2, right item appears with a null match.
        result.Count.ShouldBe(2);
        var match = result.FirstOrDefault(x => x.Left.Id == 1);
        match.ShouldNotBeNull();
        match.Right.ShouldNotBeNull();
        match.Right.Id.ShouldBe(1);

        var nonMatch = result.FirstOrDefault(x => x.Left?.Id == 2);
        nonMatch.ShouldBeNull();
    }

    [Fact]
    public void RightJoin_ReturnsMultipleRows_ForMultipleMatches()
    {
        // Arrange: multiple matches among duplicated keys.
        var left = new[]
        {
            new { Id = 1, Value = "L1-A" },
            new { Id = 1, Value = "L1-B" }
        }.AsQueryable();

        var right = new[]
        {
            new { Id = 1, Value = "R1-X" },
            new { Id = 1, Value = "R1-Y" }
        }.AsQueryable();

        // Act: right join call without extension method syntax.
        var result = RightJoinExtensions.RightJoinExt(left, right, l => l.Id, r => r.Id)
            .ToList();

        // Assert: For each right item, join with every matching left item.
        // There are 2 right items each matching 2 left items; total = 4.
        result.Count.ShouldBe(4);
        result.ForEach(item =>
        {
            item.Left.ShouldNotBeNull();
            // Each right item from the outer sequence, therefore if no matching left, Right would be null;
            // here both right items match so Right is never null.
            item.Right.ShouldNotBeNull();
            item.Left.Id.ShouldBe(item.Right.Id);
        });
    }
}