using JoinExtensions.Enumerable;

namespace JoinExtensions.Tests;

public class EnumerableFullOuterJoinExtensionsTests
{
    [Fact]
    public void FullOuterJoin_WithMatchingElements_ShouldReturnAllElements()
    {
        // Arrange
        var left = new[] { 1, 2, 3 };
        var right = new[] { 2, 3, 4 };

        // Act
        var result = EnumerableFullOuterJoinExtensions.FullOuterJoinExtEnumerable(
                left,
                right,
                l => l,
                r => r,
                (l, r) => new { Left = l, Right = r })
            .OrderBy(x => x.Left == 0 ? int.MaxValue : x.Left).ThenBy(x => x.Right)
            .ToList();

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Equal(1, result[0].Left);
        Assert.Equal(0, result[0].Right);
        Assert.Equal(2, result[1].Left);
        Assert.Equal(2, result[1].Right);
        Assert.Equal(3, result[2].Left);
        Assert.Equal(3, result[2].Right);
        Assert.Equal(0, result[3].Left);
        Assert.Equal(4, result[3].Right);
    }

    [Fact]
    public void FullOuterJoin_WithEmptyLeftCollection_ShouldReturnAllRightElements()
    {
        // Arrange
        var left = Array.Empty<int>();
        var right = new[] { 1, 2, 3 };

        // Act
        var result = EnumerableFullOuterJoinExtensions.FullOuterJoinExtEnumerable(
                left,
                right,
                l => l,
                r => r,
                (l, r) => new { Left = l, Right = r })
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.All(result, item => Assert.Equal(0, item.Left));
        Assert.Equal(1, result[0].Right);
        Assert.Equal(2, result[1].Right);
        Assert.Equal(3, result[2].Right);
    }

    [Fact]
    public void FullOuterJoin_WithEmptyRightCollection_ShouldReturnAllLeftElements()
    {
        // Arrange
        var left = new[] { 1, 2, 3 };
        var right = Array.Empty<int>();

        // Act
        var result = EnumerableFullOuterJoinExtensions.FullOuterJoinExtEnumerable(
                left,
                right,
                l => l,
                r => r,
                (l, r) => new { Left = l, Right = r })
            .ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.All(result, item => Assert.Equal(0, item.Right));
        Assert.Equal(1, result[0].Left);
        Assert.Equal(2, result[1].Left);
        Assert.Equal(3, result[2].Left);
    }

    [Fact]
    public void FullOuterJoin_WithComplexObjects_ShouldJoinOnSpecifiedKeys()
    {
        // Arrange
        var leftPersons = new[]
        {
            new { Id = 1, Name = "Alice" },
            new { Id = 2, Name = "Bob" },
            new { Id = 3, Name = "Charlie" }
        };

        var rightOrders = new[]
        {
            new { PersonId = 2, Order = "Book" },
            new { PersonId = 3, Order = "Laptop" },
            new { PersonId = 4, Order = "Phone" }
        };

        // Act
        var result = EnumerableFullOuterJoinExtensions.FullOuterJoinExtEnumerable(
                leftPersons,
                rightOrders,
                l => l.Id,
                r => r.PersonId,
                (person, order) => new { Person = person, Order = order })
            .OrderBy(x => x.Person?.Id ?? x.Order?.PersonId)
            .ToList();

        // Assert
        Assert.Equal(4, result.Count);

        Assert.NotNull(result[0].Person);
        Assert.Null(result[0].Order);
        Assert.Equal("Alice", result[0].Person.Name);

        Assert.NotNull(result[1].Person);
        Assert.NotNull(result[1].Order);
        Assert.Equal("Bob", result[1].Person.Name);
        Assert.Equal("Book", result[1].Order.Order);

        Assert.NotNull(result[2].Person);
        Assert.NotNull(result[2].Order);
        Assert.Equal("Charlie", result[2].Person.Name);
        Assert.Equal("Laptop", result[2].Order.Order);

        Assert.Null(result[3].Person);
        Assert.NotNull(result[3].Order);
        Assert.Equal("Phone", result[3].Order.Order);
    }
}