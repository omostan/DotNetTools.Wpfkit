#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <               DotNet WPF Tool Kit             |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: DotNetTools.Wpfkit                                                *
*            Filename: BaseViewModel.cs                                                  *
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

namespace DotNetTools.Wpfkit.MvvM;

public class BaseViewModel : ObservableObject
{
    string _title = string.Empty;

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    string _subtitle = string.Empty;

    /// <summary>
    /// Gets or sets the subtitle.
    /// </summary>
    /// <value>The subtitle.</value>
    public string Subtitle
    {
        get => _subtitle;
        set => SetProperty(ref _subtitle, value);
    }

    string _icon = string.Empty;

    /// <summary>
    /// Gets or sets the icon.
    /// </summary>
    /// <value>The icon.</value>
    public string Icon
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }

    bool _isBusy;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is busy.
    /// </summary>
    /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
                IsNotBusy = !_isBusy;
        }
    }

    bool _isNotBusy = true;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is not busy.
    /// </summary>
    /// <value><c>true</c> if this instance is not busy; otherwise, <c>false</c>.</value>
    public bool IsNotBusy
    {
        get => _isNotBusy;
        set
        {
            if (SetProperty(ref _isNotBusy, value))
                IsBusy = !_isNotBusy;
        }
    }

    bool _canLoadMore = true;

    /// <summary>
    /// Gets or sets a value indicating whether this instance can load more.
    /// </summary>
    /// <value><c>true</c> if this instance can load more; otherwise, <c>false</c>.</value>
    public bool CanLoadMore
    {
        get => _canLoadMore;
        set => SetProperty(ref _canLoadMore, value);
    }


    string _header = string.Empty;

    /// <summary>
    /// Gets or sets the header.
    /// </summary>
    /// <value>The header.</value>
    public string Header
    {
        get => _header;
        set => SetProperty(ref _header, value);
    }

    string _footer = string.Empty;

    /// <summary>
    /// Gets or sets the footer.
    /// </summary>
    /// <value>The footer.</value>
    public string Footer
    {
        get => _footer;
        set => SetProperty(ref _footer, value);
    }
}
