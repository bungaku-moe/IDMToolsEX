using Avalonia.Controls;
using IDMToolsEX.Lib;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX.Views;

public partial class BarcodeWindow : Window
{
    public BarcodeWindow(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        DataContext = new BarcodeWindowViewModel(mainWindowViewModel, databaseService);
        Loaded += async (_, _) =>
        {
            if (DataContext is not BarcodeWindowViewModel viewModel) return;
            await viewModel.InitializeAsync();
            InitializeComponent();
        };
    }

    private async void OnModisSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count <= 0) return;
        var selectedItem = e.AddedItems[0]?.ToString();
        if (DataContext is not BarcodeWindowViewModel viewModel) return;
        await viewModel.GetModisShelfsAsync(selectedItem);
    }
}
