using Avalonia;
using Avalonia.Controls;
using IDMToolsEX.Lib;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX.Views;

public partial class BarcodeWindow : Window
{
    // private MainWindowViewModel _mainWindowViewModel;
    private readonly BarcodeWindowViewModel _barcodeViewModel;

    public BarcodeWindow(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        // _mainWindowViewModel = mainWindowViewModel;
        _barcodeViewModel = new BarcodeWindowViewModel(mainWindowViewModel, databaseService);
        DataContext = _barcodeViewModel;
        Loaded += async (_, _) =>
        {
            await _barcodeViewModel.InitializeAsync();
            InitializeComponent();
        };
        Closed += (sender, args) => { _barcodeViewModel.Cleanup(); };
    }

    private async void OnModisSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count <= 0) return;
        var selectedItem = e.AddedItems[0]?.ToString();
        await _barcodeViewModel.GetModisShelfsAsync(selectedItem);
    }

    private async void OnShelfSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        await _barcodeViewModel.GenerateBarcodeFromModis();
    }

    private void OnPluListChanged(object? sender, TextChangedEventArgs e)
    {
        _barcodeViewModel.UpdatePluListCount();
    }
}
