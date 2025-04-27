using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX.Views;

public partial class PriceWindow : Window
{
    public PriceWindow(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        DataContext = new PriceWindowViewModel(mainWindowViewModel, databaseService);
        InitializeComponent();
    }

    public PriceWindowViewModel ViewModel => (PriceWindowViewModel)DataContext;

    [RelayCommand]
    private void ClearAllItems()
    {
        ViewModel.ItemPricesList.Clear();
        ViewModel.AppendLog("Daftar harga dibersihkan.");
    }

    [RelayCommand]
    private void DeleteItem(ItemPrice item)
    {
        ViewModel.ItemPricesList.Remove(item);
        ViewModel.AppendLog($"Item {item.Plu} dihapus dari daftar.");
    }

    private void SearchTextBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            ViewModel.GetItemPrice();
    }
}
