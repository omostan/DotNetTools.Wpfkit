#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit                                                *
*            Filename: ObservableRangeCollection.cs                                      *
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

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DotNetTools.Wpfkit.MvvM;

/// <summary> 
/// Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed. 
/// </summary> 
/// <typeparam name="T"></typeparam> 
public class ObservableRangeCollection<T> : ObservableCollection<T>
{

    #region Constructors

    /// <summary> 
    /// Initializes a new instance of the System.Collections.ObjectModel.ObservableCollection(Of T) class. 
    /// </summary> 
    public ObservableRangeCollection() : base()
    {
    }

    /// <summary> 
    /// Initializes a new instance of the System.Collections.ObjectModel.ObservableCollection(Of T) class that contains elements copied from the specified collection.
    /// </summary> 
    /// <param name="collection">collection: The collection from which the elements are copied.</param> 
    /// <exception cref="System.ArgumentNullException">The collection parameter cannot be null.</exception>
    public ObservableRangeCollection(IEnumerable<T> collection) : base(collection)
    {
    }

    #endregion

    #region AddRange

    /// <summary> 
    /// Adds the elements of the specified collection to the end of the ObservableCollection(Of T). 
    /// </summary> 
    public void AddRange(IEnumerable<T> collection, NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Add)
    {
        if (notificationMode != NotifyCollectionChangedAction.Add && notificationMode != NotifyCollectionChangedAction.Reset)
            throw new ArgumentException("Mode must be either Add or Reset for AddRange.", nameof(notificationMode));
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        CheckReentrancy();

        int startIndex = Count;

        T[] enumerable = collection as T[] ?? collection.ToArray();
        bool itemsAdded = AddArrangeCore(enumerable);

        if (!itemsAdded)
            return;

        if (notificationMode == NotifyCollectionChangedAction.Reset)
        {
            RaiseChangeNotificationEvents(action: NotifyCollectionChangedAction.Reset);

            return;
        }

        List<T> changedItems = collection as List<T> ?? [.. enumerable];

        RaiseChangeNotificationEvents(
            action: NotifyCollectionChangedAction.Add,
            changedItems: changedItems,
            startingIndex: startIndex);
    }

    #endregion

    #region RemoveRange

    /// <summary> 
    /// Removes the first occurence of each item in the specified collection from ObservableCollection(Of T). NOTE: with notificationMode = Remove, removed items starting index is not set because items are not guaranteed to be consecutive.
    /// </summary> 
    public void RemoveRange(IEnumerable<T>? collection, NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Reset)
    {
        if (notificationMode != NotifyCollectionChangedAction.Remove && notificationMode != NotifyCollectionChangedAction.Reset)
            throw new ArgumentException("Mode must be either Remove or Reset for RemoveRange.", nameof(notificationMode));
        if (collection != null)
        {
            CheckReentrancy();

            if (notificationMode == NotifyCollectionChangedAction.Reset)
            {
                bool raiseEvents = false;
                foreach (T item in collection)
                {
                    Items.Remove(item);
                    raiseEvents = true;
                }

                if (raiseEvents)
                    RaiseChangeNotificationEvents(action: NotifyCollectionChangedAction.Reset);

                return;
            }

            List<T> changedItems = new List<T>(collection);
            for (int i = 0; i < changedItems.Count; i++)
            {
                if (Items.Remove(changedItems[i]))
                {
                    continue;
                }

                changedItems
                    .RemoveAt(i); //Can't use a foreach because changedItems is intended to be (carefully) modified
                i--;
            }

            if (changedItems.Count == 0)
                return;

            RaiseChangeNotificationEvents(
                action: NotifyCollectionChangedAction.Remove,
                changedItems: changedItems);
        }
        else
        {
            throw new ArgumentNullException(nameof(collection));
        }
    }

    #endregion

    #region Replace

    /// <summary> 
    /// Clears the current collection and replaces it with the specified item. 
    /// </summary> 
    public void Replace(T item) => ReplaceRange([item]);

    #endregion

    #region ReplaceRange

    /// <summary> 
    /// Clears the current collection and replaces it with the specified collection. 
    /// </summary> 
    public void ReplaceRange(IEnumerable<T>? collection)
    {
        if (collection != null)
        {
            CheckReentrancy();

            bool previouslyEmpty = Items.Count == 0;

            Items.Clear();

            AddArrangeCore(collection);

            bool currentlyEmpty = Items.Count == 0;

            if (previouslyEmpty && currentlyEmpty)
                return;

            RaiseChangeNotificationEvents(action: NotifyCollectionChangedAction.Reset);
        }
        else
        {
            throw new ArgumentNullException(nameof(collection));
        }
    }

    #endregion

    #region AddArrangeCore

    private bool AddArrangeCore(IEnumerable<T> collection)
    {
        bool itemAdded = false;
        foreach (T item in collection)
        {
            Items.Add(item);
            itemAdded = true;
        }
        return itemAdded;
    }

    #endregion

    #region RaiseChangeNotificationEvents

    private void RaiseChangeNotificationEvents(NotifyCollectionChangedAction action, List<T>? changedItems = null, int startingIndex = -1)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));

        OnCollectionChanged(changedItems is null
            ? new NotifyCollectionChangedEventArgs(action)
            : new NotifyCollectionChangedEventArgs(action, changedItems: changedItems, startingIndex: startingIndex));
    }

    #endregion
}

