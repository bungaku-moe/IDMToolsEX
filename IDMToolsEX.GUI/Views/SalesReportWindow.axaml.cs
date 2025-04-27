using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;
using IDMToolsEX.Models;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX.Views;

public partial class SalesReportWindow : Window
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    private int _lastTabIndex;

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

    private SalesReportWindowViewModel ViewModel => (SalesReportWindowViewModel)DataContext;

    [RelayCommand]
    private void DeleteItem(GroupedSaleItem group)
    {
        var (items, pluList) = ViewModel.SelectedTabIndex switch
        {
            0 => (ViewModel.HematItems, _mainWindowViewModel.Settings.HematPluList),
            1 => (ViewModel.MurahItems, _mainWindowViewModel.Settings.MurahPluList),
            2 => (ViewModel.HebohItems, _mainWindowViewModel.Settings.HebohPluList),
            _ => (null, null)
        };

        if (items == null || pluList == null) return;

        items.Remove(group);
        pluList.Remove(group.Plu);

        _mainWindowViewModel.SettingsLoader.SaveSettings(_mainWindowViewModel.Settings);
        ViewModel.AppendLog($"Plu {group.Plu} dihapus.");
    }

    [RelayCommand]
    private void ClearAllItems()
    {
        var (items, pluList) = ViewModel.SelectedTabIndex switch
        {
            0 => (ViewModel.HematItems, _mainWindowViewModel.Settings.HematPluList),
            1 => (ViewModel.MurahItems, _mainWindowViewModel.Settings.MurahPluList),
            2 => (ViewModel.HebohItems, _mainWindowViewModel.Settings.HebohPluList),
            _ => (null, null)
        };

        if (items == null || pluList == null) return;

        items.Clear();
        pluList.Clear();

        _mainWindowViewModel.SettingsLoader.SaveSettings(_mainWindowViewModel.Settings);
        ViewModel.AppendLog("Daftar item-item dihapus");
    }

    private void OnShiftDateChanged(object? sender, DatePickerSelectedValueChangedEventArgs e)
    {
        LoadAllItems();
    }

    private void OnShiftChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        LoadAllItems();
    }

    private void LoadAllItems()
    {
        _lastTabIndex = ViewModel.SelectedTabIndex;
        ViewModel.LoadAllItems();
        ViewModel.SelectedTabIndex = _lastTabIndex;
    }

    private void NewPluTextBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            ViewModel.AddItem();
    }
}
