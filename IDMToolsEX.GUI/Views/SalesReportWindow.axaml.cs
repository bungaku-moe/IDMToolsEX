using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;
using IDMToolsEX.Models;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX.Views;

public partial class SalesReportWindow : Window
{
    private MainWindowViewModel _mainWindowViewModel;
    private SalesReportWindowViewModel ViewModel => (SalesReportWindowViewModel)DataContext;

    public SalesReportWindow(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        DataContext = new SalesReportWindowViewModel(mainWindowViewModel, databaseService);
        InitializeComponent();
        Loaded += async (_, _) =>
        {
            if (DataContext is not SalesReportWindowViewModel viewModel) return;
            await viewModel.Initialize();
        };
    }

    [RelayCommand]
    private void DeleteItem(GroupedSaleItem group)
    {
        ViewModel.GroupedItemsList.Remove(group);
        _mainWindowViewModel.Settings.SalesReportPluList.Remove(group.Plu);
        _mainWindowViewModel.SettingsLoader.SaveSettings(_mainWindowViewModel.Settings);
        ViewModel.AppendLog($"Plu {group.Plu} dihapus.");
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
