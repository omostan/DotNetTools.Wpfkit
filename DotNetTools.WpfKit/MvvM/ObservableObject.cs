#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit                                                *
*            Filename: ObservableObject.cs                                               *
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

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DotNetTools.Wpfkit.MvvM;

/// <summary>
/// Provides a base implementation of <see cref="INotifyPropertyChanged"/> for observable objects in MVVM patterns.
/// This class simplifies property change notification and validation in WPF applications.
/// </summary>
public class ObservableObject : INotifyPropertyChanged
{
    #region SetProperty

    /// <summary>
    /// Sets the value of a property and raises the <see cref="PropertyChanged"/> event if the value has changed.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="backingStore">Reference to the backing field for the property.</param>
    /// <param name="value">The new value to set.</param>
    /// <param name="propertyName">The name of the property. This is automatically populated by the compiler.</param>
    /// <param name="onChanged">Optional action to invoke after the property value has been changed and the event has been raised.</param>
    /// <param name="validateValue">Optional validation function that takes the old and new values and returns <c>true</c> if the change is valid.</param>
    /// <returns>
    /// <c>true</c> if the property value was changed and the <see cref="PropertyChanged"/> event was raised; 
    /// <c>false</c> if the value did not change or validation failed.
    /// </returns>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    /// <item>Checks if the new value is equal to the current value using <see cref="EqualityComparer{T}"/>.</item>
    /// <item>If a validation function is provided, validates the value change.</item>
    /// <item>Updates the backing store with the new value.</item>
    /// <item>Invokes the optional <paramref name="onChanged"/> action.</item>
    /// <item>Raises the <see cref="PropertyChanged"/> event.</item>
    /// </list>
    /// </remarks>
    protected virtual bool SetProperty<T>(
        ref T backingStore, T value,
        [CallerMemberName] string propertyName = "",
        Action? onChanged = null,
        Func<T, T, bool>? validateValue = null)
    {
        //if value didn't change
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        //if value changed but didn't validate
        if (validateValue != null && !validateValue(backingStore, value))
            return false;

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion


    #region PropertyChanged

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    /// <remarks>
    /// This event is part of the <see cref="INotifyPropertyChanged"/> interface and is used by 
    /// WPF data binding to detect changes in property values and update the UI accordingly.
    /// </remarks>
    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region OnPropertyChanged

    /// <summary>
    /// Raises the property changed event.
    /// </summary>
    /// <param name="propertyName">Property name.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    #endregion

}
