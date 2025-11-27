#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit.Tests                                          *
*            Filename: ObservableRangeCollectionTests.cs                                 *
*              Author: Stanley Omoregie                                                  *
*        Created Date: 20.11.2025                                                        *
*       Modified Date: 21.11.2025                                                        *
*          Created By: Stanley Omoregie                                                  *
*    Last Modified By: Stanley Omoregie                                                  *
*           CopyRight: copyright © 2025 Omotech Digital Solutions                        *
*                  .oooO  Oooo.                                                          *
*                  (   )  (   )                                                          *
* ------------------\ (----) /---------------------------------------------------------- *
*                    \_)  (_/                                                            *
*****************************************************************************************/

#endregion copyright

using DotNetTools.Wpfkit.MvvM;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DotNetTools.Wpfkit.Tests.MvvM;

/// <summary>
/// Unit tests for the ObservableRangeCollection class.
/// </summary>
public class ObservableRangeCollectionTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_Default_ShouldCreateEmptyCollection()
    {
        // Arrange & Act
        var collection = new ObservableRangeCollection<int>();

        // Assert
        collection.Should().BeEmpty();
        collection.Count.Should().Be(0);
    }

    [Fact]
    public void Constructor_WithEnumerable_ShouldPopulateCollection()
    {
        // Arrange
        var items = new[] { 1, 2, 3, 4, 5 };

        // Act
        var collection = new ObservableRangeCollection<int>(items);

        // Assert
        collection.Should().HaveCount(5);
        collection.Should().Equal(items);
    }

    [Fact]
    public void Constructor_WithNullEnumerable_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Action act = () => new ObservableRangeCollection<int>(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region AddRange Tests

    [Fact]
    public void AddRange_WithValidCollection_ShouldAddAllItems()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
        var newItems = new[] { 4, 5, 6 };

        // Act
        collection.AddRange(newItems);

        // Assert
        collection.Should().HaveCount(6);
        collection.Should().Equal(1, 2, 3, 4, 5, 6);
    }

    [Fact]
    public void AddRange_WithNullCollection_ShouldThrowArgumentNullException()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int>();

        // Act & Assert
        Action act = () => collection.AddRange(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddRange_WithEmptyCollection_ShouldNotModify()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
        var newItems = Array.Empty<int>();

        // Act
        collection.AddRange(newItems);

        // Assert
        collection.Should().HaveCount(3);
        collection.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void AddRange_WithAddMode_ShouldRaiseCollectionChangedWithAdd()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
        var newItems = new[] { 4, 5 };
        NotifyCollectionChangedEventArgs? eventArgs = null;

        collection.CollectionChanged += (sender, args) => eventArgs = args;

        // Act
        collection.AddRange(newItems, NotifyCollectionChangedAction.Add);

        // Assert
        eventArgs.Should().NotBeNull();
        eventArgs!.Action.Should().Be(NotifyCollectionChangedAction.Add);
        eventArgs.NewItems.Should().NotBeNull();
        eventArgs.NewItems!.Cast<int>().Should().Equal(4, 5);
        eventArgs.NewStartingIndex.Should().Be(3);
    }

    [Fact]
    public void AddRange_WithResetMode_ShouldRaiseCollectionChangedWithReset()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
        var newItems = new[] { 4, 5 };
        NotifyCollectionChangedEventArgs? eventArgs = null;

        collection.CollectionChanged += (sender, args) => eventArgs = args;

        // Act
        collection.AddRange(newItems, NotifyCollectionChangedAction.Reset);

        // Assert
        eventArgs.Should().NotBeNull();
        eventArgs!.Action.Should().Be(NotifyCollectionChangedAction.Reset);
    }

    [Fact]
    public void AddRange_WithInvalidMode_ShouldThrowArgumentException()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int>();
        var newItems = new[] { 1, 2, 3 };

        // Act & Assert
        Action act = () => collection.AddRange(newItems, NotifyCollectionChangedAction.Remove);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddRange_ShouldRaisePropertyChangedForCount()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
        var newItems = new[] { 4, 5 };
        var propertyChangedRaised = false;

        ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == "Count")
                propertyChangedRaised = true;
        };

        // Act
        collection.AddRange(newItems);

        // Assert
        propertyChangedRaised.Should().BeTrue();
    }

    #endregion

    #region RemoveRange Tests

    [Fact]
    public void RemoveRange_WithValidCollection_ShouldRemoveAllItems()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3, 4, 5 };
        var itemsToRemove = new[] { 2, 4 };

        // Act
        collection.RemoveRange(itemsToRemove);

        // Assert
        collection.Should().HaveCount(3);
        collection.Should().Equal(1, 3, 5);
    }

    [Fact]
    public void RemoveRange_WithNullCollection_ShouldThrowArgumentNullException()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };

        // Act & Assert
        Action act = () => collection.RemoveRange(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void RemoveRange_WithNonExistentItems_ShouldNotModify()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
        var itemsToRemove = new[] { 4, 5 };

        // Act
        collection.RemoveRange(itemsToRemove);

        // Assert
        collection.Should().HaveCount(3);
        collection.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void RemoveRange_WithResetMode_ShouldRaiseCollectionChangedWithReset()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3, 4, 5 };
        var itemsToRemove = new[] { 2, 4 };
        NotifyCollectionChangedEventArgs? eventArgs = null;

        collection.CollectionChanged += (sender, args) => eventArgs = args;

        // Act
        collection.RemoveRange(itemsToRemove, NotifyCollectionChangedAction.Reset);

        // Assert
        eventArgs.Should().NotBeNull();
        eventArgs!.Action.Should().Be(NotifyCollectionChangedAction.Reset);
    }

    [Fact]
    public void RemoveRange_WithRemoveMode_ShouldRaiseCollectionChangedWithRemove()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3, 4, 5 };
        var itemsToRemove = new[] { 2, 4 };
        NotifyCollectionChangedEventArgs? eventArgs = null;

        collection.CollectionChanged += (sender, args) => eventArgs = args;

        // Act
        collection.RemoveRange(itemsToRemove, NotifyCollectionChangedAction.Remove);

        // Assert
        eventArgs.Should().NotBeNull();
        eventArgs!.Action.Should().Be(NotifyCollectionChangedAction.Remove);
        eventArgs.OldItems.Should().NotBeNull();
    }

    [Fact]
    public void RemoveRange_WithInvalidMode_ShouldThrowArgumentException()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
        var itemsToRemove = new[] { 1, 2 };

        // Act & Assert
        Action act = () => collection.RemoveRange(itemsToRemove, NotifyCollectionChangedAction.Add);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void RemoveRange_ShouldRaisePropertyChangedForCount()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3, 4, 5 };
        var itemsToRemove = new[] { 2, 4 };
        var propertyChangedRaised = false;

        ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == "Count")
                propertyChangedRaised = true;
        };

        // Act
        collection.RemoveRange(itemsToRemove);

        // Assert
        propertyChangedRaised.Should().BeTrue();
    }

    #endregion

    #region Replace Tests

    [Fact]
    public void Replace_WithSingleItem_ShouldClearAndAddItem()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };

        // Act
        collection.Replace(99);

        // Assert
        collection.Should().HaveCount(1);
        collection.Should().Equal(99);
    }

    [Fact]
    public void Replace_ShouldRaiseCollectionChangedWithReset()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
        NotifyCollectionChangedEventArgs? eventArgs = null;

        collection.CollectionChanged += (sender, args) => eventArgs = args;

        // Act
        collection.Replace(99);

        // Assert
        eventArgs.Should().NotBeNull();
        eventArgs!.Action.Should().Be(NotifyCollectionChangedAction.Reset);
    }

    #endregion

    #region ReplaceRange Tests

    [Fact]
    public void ReplaceRange_WithValidCollection_ShouldReplaceAllItems()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
        var newItems = new[] { 7, 8, 9 };

        // Act
        collection.ReplaceRange(newItems);

        // Assert
        collection.Should().HaveCount(3);
        collection.Should().Equal(7, 8, 9);
    }

    [Fact]
    public void ReplaceRange_WithNullCollection_ShouldThrowArgumentNullException()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };

        // Act & Assert
        Action act = () => collection.ReplaceRange(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ReplaceRange_WithEmptyCollection_ShouldClearOriginal()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
        var newItems = Array.Empty<int>();

        // Act
        collection.ReplaceRange(newItems);

        // Assert
        collection.Should().BeEmpty();
    }

    [Fact]
    public void ReplaceRange_ShouldRaiseCollectionChangedWithReset()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
        var newItems = new[] { 7, 8, 9 };
        NotifyCollectionChangedEventArgs? eventArgs = null;

        collection.CollectionChanged += (sender, args) => eventArgs = args;

        // Act
        collection.ReplaceRange(newItems);

        // Assert
        eventArgs.Should().NotBeNull();
        eventArgs!.Action.Should().Be(NotifyCollectionChangedAction.Reset);
    }

    [Fact]
    public void ReplaceRange_WhenBothEmpty_ShouldNotRaiseEvent()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int>();
        var newItems = Array.Empty<int>();
        var eventRaised = false;

        collection.CollectionChanged += (sender, args) => eventRaised = true;

        // Act
        collection.ReplaceRange(newItems);

        // Assert
        eventRaised.Should().BeFalse();
    }

    [Fact]
    public void ReplaceRange_ShouldRaisePropertyChangedForCount()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
        var newItems = new[] { 7, 8 };
        var propertyChangedRaised = false;

        ((INotifyPropertyChanged)collection).PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == "Count")
                propertyChangedRaised = true;
        };

        // Act
        collection.ReplaceRange(newItems);

        // Assert
        propertyChangedRaised.Should().BeTrue();
    }

    #endregion

    #region Performance Tests

    [Fact]
    public void AddRange_WithLargeCollection_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var items = Enumerable.Range(1, 1000).ToArray();
        var collection = new ObservableRangeCollection<int>();

        // Act
        var sw = System.Diagnostics.Stopwatch.StartNew();
        collection.AddRange(items);
        sw.Stop();

        // Assert
        collection.Should().HaveCount(1000);
        collection.Should().Contain(1);
        collection.Should().Contain(1000);
        sw.ElapsedMilliseconds.Should().BeLessThan(1000); // Should complete in less than 1 second
    }

    [Fact]
    public void AddRange_ShouldRaiseFewerEventsThanMultipleAdds()
    {
        // Arrange
        var items = Enumerable.Range(1, 100).ToArray();
        
        // Test AddRange - should raise 1 or 2 events (PropertyChanged + CollectionChanged)
        var collection1 = new ObservableRangeCollection<int>();
        var addRangeEventCount = 0;
        collection1.CollectionChanged += (s, e) => addRangeEventCount++;
        
        // Test Multiple Adds - should raise 100 events (1 per Add)
        var collection2 = new ObservableRangeCollection<int>();
        var multipleAddsEventCount = 0;
        collection2.CollectionChanged += (s, e) => multipleAddsEventCount++;

        // Act
        collection1.AddRange(items, NotifyCollectionChangedAction.Add);
        
        foreach (var item in items)
            collection2.Add(item);

        // Assert
        collection1.Should().HaveCount(100);
        collection2.Should().HaveCount(100);
        addRangeEventCount.Should().Be(1); // AddRange raises single event
        multipleAddsEventCount.Should().Be(100); // Multiple adds raise 100 events
        addRangeEventCount.Should().BeLessThan(multipleAddsEventCount); // AddRange is more efficient
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void ObservableRangeCollection_ComplexScenario_ShouldWorkCorrectly()
    {
        // Arrange
        var collection = new ObservableRangeCollection<string> { "A", "B", "C" };

        // Act & Assert - AddRange
        collection.AddRange(new[] { "D", "E" });
        collection.Should().Equal("A", "B", "C", "D", "E");

        // Act & Assert - RemoveRange
        collection.RemoveRange(new[] { "B", "D" });
        collection.Should().Equal("A", "C", "E");

        // Act & Assert - Replace
        collection.Replace("X");
        collection.Should().Equal("X");

        // Act & Assert - ReplaceRange
        collection.ReplaceRange(new[] { "P", "Q", "R" });
        collection.Should().Equal("P", "Q", "R");
    }

    [Fact]
    public void ObservableRangeCollection_WithComplexTypes_ShouldWorkCorrectly()
    {
        // Arrange
        var person1 = new { Name = "John", Age = 30 };
        var person2 = new { Name = "Jane", Age = 25 };
        var person3 = new { Name = "Bob", Age = 35 };

        var collection = new ObservableRangeCollection<object> { person1 };

        // Act
        collection.AddRange(new object[] { person2, person3 });

        // Assert
        collection.Should().HaveCount(3);
        collection.Should().Contain(person1);
        collection.Should().Contain(person2);
        collection.Should().Contain(person3);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void AddRange_ToEmptyCollection_ShouldWork()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int>();
        var items = new[] { 1, 2, 3 };

        // Act
        collection.AddRange(items);

        // Assert
        collection.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void RemoveRange_FromEmptyCollection_ShouldNotThrow()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int>();
        var items = new[] { 1, 2, 3 };

        // Act
        Action act = () => collection.RemoveRange(items);

        // Assert
        act.Should().NotThrow();
        collection.Should().BeEmpty();
    }

    [Fact]
    public void ReplaceRange_EmptyWithEmpty_ShouldNotRaiseEvents()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int>();
        var eventRaised = false;

        collection.CollectionChanged += (sender, args) => eventRaised = true;

        // Act
        collection.ReplaceRange(Array.Empty<int>());

        // Assert
        eventRaised.Should().BeFalse();
    }

    #endregion

    #region Thread Safety Tests

    [Fact]
    public async Task ObservableRangeCollection_WithConcurrentOperations_ShouldHandleGracefully()
    {
        // Arrange
        var collection = new ObservableRangeCollection<int>();
        var exceptions = new List<Exception>();
        if (exceptions == null)
        {
            throw new ArgumentNullException(nameof(exceptions));
        }

        var tasks = new List<Task>();

        // Act
        for (int i = 0; i < 5; i++)
        {
            var index = i;
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    var items = Enumerable.Range(index * 100, 10).ToArray();
                    collection.AddRange(items);
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            }));
        }

        await Task.WhenAll(tasks);

        // Assert - Should complete without deadlocks (exceptions are expected due to concurrent access)
        tasks.Should().AllSatisfy(t => t.IsCompleted.Should().BeTrue());
    }

    #endregion
}
