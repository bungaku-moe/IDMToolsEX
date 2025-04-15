using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX.Views;

public partial class PriceWindow : Window
{
    public PriceWindowViewModel ViewModel => (PriceWindowViewModel)DataContext;

    public PriceWindow(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        DataContext = new PriceWindowViewModel(mainWindowViewModel, databaseService);
        InitializeComponent();
    }

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
}
