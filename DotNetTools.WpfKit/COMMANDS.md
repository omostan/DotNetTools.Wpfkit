# Commands Documentation - DotNetTools.Wpfkit

## 📑 Table of Contents
- [Overview](#overview)
- [Quick Start](#quick-start)
- [Command Types](#command-types)
- [Usage Examples](#usage-examples)
- [Best Practices](#best-practices)
- [Advanced Scenarios](#advanced-scenarios)
- [Troubleshooting](#troubleshooting)

---

## 📋 Overview

The Commands infrastructure in DotNetTools.Wpfkit provides a complete set of implementations for the ICommand interface, enabling clean MVVM pattern implementation in WPF applications.

### Key Features

✅ **Multiple Command Types** - Sync and async operations  
✅ **Automatic State Management** - Built-in execution state tracking  
✅ **Exception Handling** - Centralized error handling for async commands  
✅ **Concurrent Execution Prevention** - No double-clicking issues  
✅ **Parameter Support** - Strongly-typed parameter passing  
✅ **CanExecute Logic** - Conditional command execution  
✅ **Logging Integration** - Built-in TraceTool logging  

### When to Use Each Command Type

| Scenario | Recommended Command |
|----------|---------------------|
| Simple button click | `ActionCommand` |
| Loading data from API | `AsyncRelayCommand` |
| Custom command logic | `CommandBase` |
| Complex async operations | `AsyncCommandBase` |
| Form validation | `ActionCommand` with predicate |
| Long-running tasks | `AsyncRelayCommand` |

---

## 🚀 Quick Start

### 1. Basic Synchronous Command

```csharp
using DotNetTools.Wpfkit.Commands;
using DotNetTools.Wpfkit.MvvM;
using System.Windows.Input;

public class MainViewModel : BaseViewModel
{
    public ICommand ClickCommand { get; }
    
    public MainViewModel()
    {
        ClickCommand = new ActionCommand(
            action: param => OnClick(),
            predicate: param => true
        );
    }
    
    private void OnClick()
    {
        System.Diagnostics.Debug.WriteLine("Button clicked!");
    }
}
```

### 2. Basic Asynchronous Command

```csharp
public class MainViewModel : BaseViewModel
{
    private readonly IDataService _dataService;
    
    public ICommand LoadCommand { get; }
    
    public MainViewModel(IDataService dataService)
    {
        _dataService = dataService;
        
        LoadCommand = new AsyncRelayCommand(
            callback: LoadDataAsync,
            onException: ex => ShowError(ex.Message)
        );
    }
    
    private async Task LoadDataAsync()
    {
        IsBusy = true;
        try
        {
            var data = await _dataService.GetDataAsync();
            // Process data
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    private void ShowError(string message)
    {
        // Display error to user
    }
}
```

### 3. XAML Binding

```xml
<Window x:Class="MyApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    
    <StackPanel>
        <Button Content="Click Me" Command="{Binding ClickCommand}"/>
        <Button Content="Load Data" Command="{Binding LoadCommand}"/>
        
        <ProgressBar IsIndeterminate="True"
                     Visibility="{Binding IsBusy, Converter={StaticResource BoolToVisibilityConverter}}"/>
    </StackPanel>
</Window>
```

---

## 🎯 Command Types

### 1. CommandBase

Abstract base class implementing `ICommand`.

#### Features
- ✅ Virtual `CanExecute` method
- ✅ Abstract `Execute` method
- ✅ `CanExecuteChanged` event
- ✅ Protected `OnCanExecuteChanged()` method

#### Usage

```csharp
public class CustomCommand : CommandBase
{
    private readonly MyViewModel _viewModel;
    
    public CustomCommand(MyViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (s, e) => 
        {
            if (e.PropertyName == nameof(_viewModel.CanSave))
            {
                OnCanExecuteChanged();
            }
        };
    }
    
    public override bool CanExecute(object? parameter)
    {
        return _viewModel.CanSave && base.CanExecute(parameter);
    }
    
    public override void Execute(object? parameter)
    {
        _viewModel.Save();
    }
}
```

#### When to Use
- Custom command logic with complex state management
- Need to respond to multiple property changes
- Implementing command-specific behavior

---

### 2. ActionCommand

Flexible command accepting delegates for execution and validation.

#### Constructor

```csharp
public ActionCommand(
    Action<object> action,
    Predicate<object>? predicate = null
)
```

#### Features
- ✅ Parameter support
- ✅ Predicate validation
- ✅ Automatic UI updates via CommandManager
- ✅ Null-checking with ArgumentNullException

#### Usage Examples

**Simple Action:**
```csharp
public ICommand SaveCommand => new ActionCommand(
    action: param => Save(),
    predicate: param => CanSave()
);
```

**With Parameters:**
```csharp
public ICommand DeleteCommand => new ActionCommand(
    action: param => Delete((int)param),
    predicate: param => param is int id && id > 0
);
```

**With Complex Validation:**
```csharp
public ICommand SubmitCommand => new ActionCommand(
    action: param => Submit((FormData)param),
    predicate: param => param is FormData data && 
                        ValidateForm(data) && 
                        !IsSubmitting
);
```

#### XAML Binding with Parameters

```xml
<!-- Simple binding -->
<Button Content="Save" Command="{Binding SaveCommand}"/>

<!-- With CommandParameter -->
<Button Content="Delete" 
        Command="{Binding DeleteCommand}" 
        CommandParameter="{Binding SelectedId}"/>

<!-- With ElementName binding -->
<TextBox x:Name="InputBox"/>
<Button Content="Submit" 
        Command="{Binding SubmitCommand}" 
        CommandParameter="{Binding Text, ElementName=InputBox}"/>
```

---

### 3. AsyncCommandBase

Abstract base for asynchronous operations with automatic state management.

#### Constructor

```csharp
public AsyncCommandBase(Action<Exception> onException)
```

#### Features
- ✅ Automatic `IsExecuting` state management
- ✅ Prevents concurrent execution
- ✅ Built-in exception handling
- ✅ TraceTool logging integration
- ✅ Automatic `CanExecute` updates

#### Usage

```csharp
public class SaveDataCommand : AsyncCommandBase
{
    private readonly IDataService _dataService;
    private readonly BaseViewModel _viewModel;
    
    public SaveDataCommand(
        IDataService dataService, 
        BaseViewModel viewModel,
        Action<Exception> onException) 
        : base(onException)
    {
        _dataService = dataService;
        _viewModel = viewModel;
    }
    
    protected override async Task ExecuteAsync(object parameter)
    {
        _viewModel.IsBusy = true;
        try
        {
            // Validation
            if (parameter is not DataModel data)
                throw new ArgumentException("Invalid data");
            
            // Long-running operation
            await _dataService.SaveAsync(data);
            
            // Success notification
            await _viewModel.ShowSuccessAsync("Data saved");
        }
        finally
        {
            _viewModel.IsBusy = false;
        }
    }
}

// In ViewModel
public ICommand SaveCommand { get; }

public MyViewModel(IDataService dataService)
{
    SaveCommand = new SaveDataCommand(
        dataService, 
        this,
        ex => ShowError($"Save failed: {ex.Message}")
    );
}
```

#### Execution Flow

```
User triggers command
    ↓
Check IsExecuting (false → continue, true → return)
    ↓
Set IsExecuting = true
    ↓
Update CanExecute (returns false)
    ↓
Execute async operation
    ↓
[On Exception]
    ├─→ Log with TraceTool
    └─→ Invoke exception handler
    ↓
Set IsExecuting = false
    ↓
Update CanExecute (returns true)
```

---

### 4. AsyncRelayCommand

Concrete async command for quick implementation.

#### Constructor

```csharp
public AsyncRelayCommand(
    Func<Task> callback,
    Action<Exception> onException
)
```

#### Features
- ✅ All AsyncCommandBase features
- ✅ Simple callback pattern
- ✅ No parameter support (use AsyncCommandBase for parameters)

#### Usage Examples

**Basic Async Operation:**
```csharp
public ICommand LoadCommand => new AsyncRelayCommand(
    callback: async () => await LoadDataAsync(),
    onException: ex => ShowError(ex.Message)
);
```

**With IsBusy:**
```csharp
public ICommand RefreshCommand => new AsyncRelayCommand(
    callback: async () =>
    {
        IsBusy = true;
        try
        {
            await _api.RefreshAsync();
        }
        finally
        {
            IsBusy = false;
        }
    },
    onException: ex => Logger.Error(ex, "Refresh failed")
);
```

**Multiple Operations:**
```csharp
public ICommand SaveAllCommand => new AsyncRelayCommand(
    callback: async () =>
    {
        await SaveLocalChangesAsync();
        await SyncWithServerAsync();
        await RefreshViewAsync();
    },
    onException: HandleSaveError
);

private void HandleSaveError(Exception ex)
{
    if (ex is NetworkException)
        ShowError("Network error. Changes saved locally.");
    else
        ShowError($"Save failed: {ex.Message}");
}
```

---

## 💡 Usage Examples

### Example 1: CRUD Operations

```csharp
using DotNetTools.Wpfkit.MvvM;
using DotNetTools.Wpfkit.Commands;
using System.Windows.Input;

public class ProductViewModel : BaseViewModel
{
    private readonly IProductService _productService;
    private readonly IDialogService _dialogService;
    
    private Product _selectedProduct;
    public Product SelectedProduct
    {
        get => _selectedProduct;
        set => SetProperty(ref _selectedProduct, value);
    }
    
    public ObservableRangeCollection<Product> Products { get; }
    
    // Commands
    public ICommand LoadProductsCommand { get; }
    public ICommand AddProductCommand { get; }
    public ICommand UpdateProductCommand { get; }
    public ICommand DeleteProductCommand { get; }
    
    public ProductViewModel(IProductService productService, IDialogService dialogService)
    {
        _productService = productService;
        _dialogService = dialogService;
        
        Products = new ObservableRangeCollection<Product>();
        
        // Load products - async
        LoadProductsCommand = new AsyncRelayCommand(
            callback: LoadProductsAsync,
            onException: HandleError
        );
        
        // Add new product - async
        AddProductCommand = new AsyncRelayCommand(
            callback: AddProductAsync,
            onException: HandleError
        );
        
        // Update selected product - async
        UpdateProductCommand = new AsyncRelayCommand(
            callback: UpdateProductAsync,
            onException: HandleError
        );
        
        // Delete selected product - sync with validation
        DeleteProductCommand = new ActionCommand(
            action: param => DeleteProduct(),
            predicate: param => SelectedProduct != null
        );
    }
    
    private async Task LoadProductsAsync()
    {
        IsBusy = true;
        Title = "Loading Products...";
        
        try
        {
            var products = await _productService.GetAllAsync();
            Products.ReplaceRange(products);
            Title = $"Products ({Products.Count})";
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    private async Task AddProductAsync()
    {
        var newProduct = await _dialogService.ShowProductDialogAsync();
        if (newProduct == null) return;
        
        IsBusy = true;
        try
        {
            await _productService.AddAsync(newProduct);
            Products.Add(newProduct);
            await _dialogService.ShowMessageAsync("Success", "Product added");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    private async Task UpdateProductAsync()
    {
        if (SelectedProduct == null) return;
        
        IsBusy = true;
        try
        {
            await _productService.UpdateAsync(SelectedProduct);
            await _dialogService.ShowMessageAsync("Success", "Product updated");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    private void DeleteProduct()
    {
        if (SelectedProduct == null) return;
        
        Products.Remove(SelectedProduct);
        SelectedProduct = null;
    }
    
    private void HandleError(Exception ex)
    {
        IsBusy = false;
        _dialogService.ShowError("Error", ex.Message);
    }
}
```

### Example 2: Form Validation

```csharp
public class RegistrationViewModel : BaseViewModel
{
    private string _username;
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value, onChanged: ValidateForm);
    }
    
    private string _email;
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value, onChanged: ValidateForm);
    }
    
    private string _password;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value, onChanged: ValidateForm);
    }
    
    private bool _isFormValid;
    public bool IsFormValid
    {
        get => _isFormValid;
        private set => SetProperty(ref _isFormValid, value);
    }
    
    public ICommand RegisterCommand { get; }
    public ICommand CancelCommand { get; }
    
    public RegistrationViewModel()
    {
        RegisterCommand = new AsyncRelayCommand(
            callback: RegisterAsync,
            onException: HandleRegistrationError
        );
        
        CancelCommand = new ActionCommand(
            action: param => Cancel(),
            predicate: param => !IsBusy
        );
    }
    
    private void ValidateForm()
    {
        IsFormValid = !string.IsNullOrWhiteSpace(Username) &&
                      !string.IsNullOrWhiteSpace(Email) &&
                      Email.Contains("@") &&
                      !string.IsNullOrWhiteSpace(Password) &&
                      Password.Length >= 8;
    }
    
    private async Task RegisterAsync()
    {
        if (!IsFormValid) return;
        
        IsBusy = true;
        Title = "Registering...";
        
        try
        {
            await _authService.RegisterAsync(Username, Email, Password);
            await ShowSuccessAsync("Registration successful!");
            NavigateToLogin();
        }
        finally
        {
            IsBusy = false;
            Title = "Registration";
        }
    }
    
    private void Cancel()
    {
        ClearForm();
        NavigateBack();
    }
    
    private void HandleRegistrationError(Exception ex)
    {
        IsBusy = false;
        
        if (ex is ValidationException valEx)
            ShowError(valEx.Message);
        else
            ShowError("Registration failed. Please try again.");
    }
}
```

### Example 3: Pagination and Search

```csharp
public class ProductListViewModel : BaseViewModel
{
    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }
    
    private int _currentPage = 1;
    private const int PageSize = 20;
    
    public ObservableRangeCollection<Product> Products { get; }
    
    public ICommand SearchCommand { get; }
    public ICommand LoadMoreCommand { get; }
    public ICommand RefreshCommand { get; }
    
    public ProductListViewModel()
    {
        Products = new ObservableRangeCollection<Product>();
        
        SearchCommand = new ActionCommand(
            action: param => Search((string)param),
            predicate: param => param is string text && 
                                text.Length >= 3 && 
                                !IsBusy
        );
        
        LoadMoreCommand = new AsyncRelayCommand(
            callback: LoadMoreAsync,
            onException: HandleError
        );
        
        RefreshCommand = new AsyncRelayCommand(
            callback: RefreshAsync,
            onException: HandleError
        );
    }
    
    private void Search(string searchText)
    {
        SearchText = searchText;
        _currentPage = 1;
        
        // Trigger async load
        _ = LoadProductsAsync(clearExisting: true);
    }
    
    private async Task LoadMoreAsync()
    {
        CanLoadMore = false;
        _currentPage++;
        await LoadProductsAsync(clearExisting: false);
    }
    
    private async Task RefreshAsync()
    {
        _currentPage = 1;
        await LoadProductsAsync(clearExisting: true);
    }
    
    private async Task LoadProductsAsync(bool clearExisting)
    {
        IsBusy = true;
        
        try
        {
            var products = await _productService.SearchAsync(
                searchText: SearchText,
                page: _currentPage,
                pageSize: PageSize
            );
            
            if (clearExisting)
                Products.ReplaceRange(products);
            else
                Products.AddRange(products);
            
            CanLoadMore = products.Count == PageSize;
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    private void HandleError(Exception ex)
    {
        IsBusy = false;
        CanLoadMore = false;
        ShowError($"Error loading products: {ex.Message}");
    }
}
```

---

## 🎯 Best Practices

### 1. Choose the Right Command Type

```csharp
// ✅ GOOD - Use AsyncRelayCommand for async operations
public ICommand LoadCommand => new AsyncRelayCommand(
    callback: LoadDataAsync,
    onException: HandleError
);

// ❌ BAD - Using ActionCommand for async (blocks UI)
public ICommand LoadCommand => new ActionCommand(
    action: param => LoadDataAsync().Wait() // DON'T DO THIS
);
```

### 2. Always Handle Exceptions

```csharp
// ✅ GOOD - Proper exception handling
public ICommand SaveCommand => new AsyncRelayCommand(
    callback: SaveAsync,
    onException: ex => 
    {
        Logger.Error(ex, "Save failed");
        ShowError($"Save failed: {ex.Message}");
    }
);

// ❌ BAD - No exception handler (exceptions swallowed)
public ICommand SaveCommand => new AsyncRelayCommand(
    callback: SaveAsync,
    onException: ex => { } // Silent failure
);
```

### 3. Use IsBusy for UI Feedback

```csharp
// ✅ GOOD - Proper state management
private async Task LoadDataAsync()
{
    IsBusy = true;
    try
    {
        var data = await _service.GetDataAsync();
        Items.ReplaceRange(data);
    }
    finally
    {
        IsBusy = false; // Always reset in finally
    }
}

// ❌ BAD - IsBusy not reset on error
private async Task LoadDataAsync()
{
    IsBusy = true;
    var data = await _service.GetDataAsync();
    Items.ReplaceRange(data);
    IsBusy = false; // Won't execute if exception thrown
}
```

### 4. Validate Parameters

```csharp
// ✅ GOOD - Type-safe validation
public ICommand DeleteCommand => new ActionCommand(
    action: param => Delete((int)param),
    predicate: param => param is int id && id > 0
);

// ❌ BAD - Unsafe casting
public ICommand DeleteCommand => new ActionCommand(
    action: param => Delete((int)param), // Could throw InvalidCastException
    predicate: param => true
);
```

### 5. Prevent Concurrent Execution

```csharp
// ✅ GOOD - AsyncRelayCommand handles this automatically
public ICommand SaveCommand => new AsyncRelayCommand(
    callback: SaveAsync,
    onException: HandleError
);

// ❌ BAD - Manual state management (unnecessary)
private bool _isSaving;
public ICommand SaveCommand => new AsyncRelayCommand(
    callback: async () =>
    {
        if (_isSaving) return; // AsyncCommandBase already does this
        _isSaving = true;
        try
        {
            await SaveAsync();
        }
        finally
        {
            _isSaving = false;
        }
    },
    onException: HandleError
);
```

### 6. Don't Capture Large Objects in Closures

```csharp
// ✅ GOOD - Pass parameters explicitly
public ICommand ProcessCommand => new ActionCommand(
    action: param => ProcessData((DataModel)param),
    predicate: param => param is DataModel
);

// ❌ BAD - Captures entire collection in closure
public ICommand ProcessCommand => new ActionCommand(
    action: param => 
    {
        foreach (var item in largeCollection) // Keeps collection in memory
        {
            // Process
        }
    }
);
```

### 7. Use Descriptive Command Names

```csharp
// ✅ GOOD - Clear intent
public ICommand SaveCustomerCommand { get; }
public ICommand LoadProductsCommand { get; }
public ICommand DeleteSelectedItemCommand { get; }

// ❌ BAD - Vague names
public ICommand Command1 { get; }
public ICommand DoStuff { get; }
public ICommand Execute { get; }
```

---

## 🔧 Advanced Scenarios

### Custom Async Command with Parameters

```csharp
public class LoadDataCommand : AsyncCommandBase
{
    private readonly IDataService _service;
    
    public LoadDataCommand(IDataService service, Action<Exception> onException)
        : base(onException)
    {
        _service = service;
    }
    
    protected override async Task ExecuteAsync(object parameter)
    {
        if (parameter is not LoadDataParameters params)
            throw new ArgumentException("Invalid parameters");
        
        var data = await _service.LoadAsync(
            startDate: params.StartDate,
            endDate: params.EndDate,
            category: params.Category
        );
        
        // Process data
    }
}

// Usage
public ICommand LoadCommand { get; }

public MyViewModel()
{
    LoadCommand = new LoadDataCommand(_service, HandleError);
}

// In XAML
<Button Command="{Binding LoadCommand}">
    <Button.CommandParameter>
        <local:LoadDataParameters StartDate="{Binding StartDate}" 
                                 EndDate="{Binding EndDate}"
                                 Category="{Binding SelectedCategory}"/>
    </Button.CommandParameter>
</Button>
```

### Command with Progress Reporting

```csharp
public class DownloadCommand : AsyncCommandBase
{
    private readonly IDownloadService _service;
    private readonly IProgress<int> _progress;
    
    public DownloadCommand(
        IDownloadService service,
        IProgress<int> progress,
        Action<Exception> onException)
        : base(onException)
    {
        _service = service;
        _progress = progress;
    }
    
    protected override async Task ExecuteAsync(object parameter)
    {
        var url = parameter as string;
        await _service.DownloadAsync(url, _progress);
    }
}

// In ViewModel
private int _downloadProgress;
public int DownloadProgress
{
    get => _downloadProgress;
    set => SetProperty(ref _downloadProgress, value);
}

public MyViewModel()
{
    var progress = new Progress<int>(p => DownloadProgress = p);
    
    DownloadCommand = new DownloadCommand(
        _downloadService,
        progress,
        HandleError
    );
}
```

### Composite Command

```csharp
public class CompositeCommand : CommandBase
{
    private readonly List<ICommand> _commands = new();
    
    public void RegisterCommand(ICommand command)
    {
        _commands.Add(command);
        command.CanExecuteChanged += (s, e) => OnCanExecuteChanged();
    }
    
    public override bool CanExecute(object? parameter)
    {
        return _commands.All(cmd => cmd.CanExecute(parameter));
    }
    
    public override void Execute(object? parameter)
    {
        foreach (var command in _commands)
        {
            if (command.CanExecute(parameter))
                command.Execute(parameter);
        }
    }
}

// Usage
public MyViewModel()
{
    var saveCommand = new ActionCommand(param => Save());
    var validateCommand = new ActionCommand(param => Validate());
    
    var compositeCommand = new CompositeCommand();
    compositeCommand.RegisterCommand(validateCommand);
    compositeCommand.RegisterCommand(saveCommand);
    
    SaveAllCommand = compositeCommand;
}
```

---

## 🐛 Troubleshooting

### Command Not Executing

**Problem:** Button click doesn't trigger command

**Solutions:**
1. Check if command is bound correctly in XAML
2. Verify `CanExecute` returns `true`
3. Ensure command is not null
4. Check for exceptions in exception handler

```csharp
// Debug CanExecute
public ICommand MyCommand => new ActionCommand(
    action: param => Execute(),
    predicate: param =>
    {
        var result = CanExecuteLogic();
        Debug.WriteLine($"CanExecute: {result}");
        return result;
    }
);
```

### Command Disabled After First Execution

**Problem:** Async command stays disabled

**Solution:** Ensure `IsBusy` is reset in `finally` block

```csharp
// ✅ CORRECT
private async Task LoadAsync()
{
    IsBusy = true;
    try
    {
        await _service.LoadAsync();
    }
    finally
    {
        IsBusy = false; // Always executed
    }
}
```

### Exception Not Caught

**Problem:** Exceptions not reaching exception handler

**Solution:** Don't catch exceptions inside async method if you want them handled by AsyncCommandBase

```csharp
// ❌ WRONG - Exception caught internally
private async Task LoadAsync()
{
    try
    {
        await _service.LoadAsync();
    }
    catch (Exception ex)
    {
        // Exception won't reach AsyncCommandBase
        Logger.Error(ex);
    }
}

// ✅ CORRECT - Let AsyncCommandBase handle it
private async Task LoadAsync()
{
    await _service.LoadAsync(); // Exception propagates to AsyncCommandBase
}
```

### Memory Leaks

**Problem:** Commands holding references preventing GC

**Solution:** Use weak event patterns or dispose properly

```csharp
// Consider implementing IDisposable for complex commands
public class MyViewModel : BaseViewModel, IDisposable
{
    private readonly CompositeDisposable _disposables = new();
    
    public MyViewModel()
    {
        // Store disposable subscriptions
        PropertyChanged += OnPropertyChanged;
        _disposables.Add(Disposable.Create(() => 
            PropertyChanged -= OnPropertyChanged));
    }
    
    public void Dispose()
    {
        _disposables.Dispose();
    }
}
```

---

## 📚 Additional Resources

- [WPF Commands Overview](https://docs.microsoft.com/dotnet/desktop/wpf/advanced/commanding-overview)
- [MVVM Pattern](https://docs.microsoft.com/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern)
- [Async/Await Best Practices](https://docs.microsoft.com/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)
- [DotNetTools.Wpfkit GitHub](https://github.com/omostan/DotNetTools.Wpfkit)

---

**Need Help?**  
- 📧 Email: stan@omotech.com  
- 🐛 Issues: https://github.com/omostan/DotNetTools.Wpfkit/issues  
- 📖 Full API Docs: See README.md

---

*Last Updated: November 24, 2025*  
*Version: 1.0.2*
