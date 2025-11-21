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

public class ObservableObject : INotifyPropertyChanged
{
    /// <summary>
    /// Sets the property.
    /// </summary>
    /// <returns><c>true</c>, if property was set, <c>false</c> otherwise.</returns>
    /// <param name="backingStore">Backing store.</param>
    /// <param name="value">Value.</param>
    /// <param name="validateValue">Validates value.</param>
    /// <param name="propertyName">Property name.</param>
    /// <param name="onChanged">On changed.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
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


    /// <summary>
    /// Occurs when property changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the property changed event.
    /// </summary>
    /// <param name="propertyName">Property name.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

}
