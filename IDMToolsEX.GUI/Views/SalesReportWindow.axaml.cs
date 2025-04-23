using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;
using IDMToolsEX.Models;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX.Views;

public partial class SalesReportWindow : Window
{
    public SalesReportWindow(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        InitializeComponent();
        DataContext = new SalesReportWindowViewModel(mainWindowViewModel, databaseService);
        Loaded += async (_, _) =>
        {
            if (DataContext is not SalesReportWindowViewModel viewModel) return;
            await viewModel.Initialize();
        };
    }

    public SalesReportWindowViewModel ViewModel => (SalesReportWindowViewModel)DataContext;

    [RelayCommand]
    private void DeleteItem(GroupedSaleItem group)
    {
        // foreach (var item in group.Items?.ToList())
        // {
            ViewModel.GroupedItemsList.Remove(group);
        // }

        // ViewModel.RegroupItems(); // re-apply grouping
    }


    [RelayCommand]
    private void ClearAllItems()
    {
        ViewModel.GroupedItemsList.Clear();
        ViewModel.AppendLog("Daftar item-item dihapus");
    }

    private void OnShiftDateChanged(object? sender, DatePickerSelectedValueChangedEventArgs e)
    {
        ViewModel.LoadItems();
    }

    private void OnShiftChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        ViewModel.LoadItems();
    }
}
