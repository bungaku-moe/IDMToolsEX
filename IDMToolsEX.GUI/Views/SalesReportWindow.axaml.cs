using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;
using IDMToolsEX.Models;
using IDMToolsEX.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

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
    private async void DeleteItem(GroupedSaleItem group)
    {
        var (items, pluList) = ViewModel.SelectedTabIndex switch
        {
            0 => (ViewModel.HematItems, _mainWindowViewModel.Settings.HematPluList),
            1 => (ViewModel.MurahItems, _mainWindowViewModel.Settings.MurahPluList),
            2 => (ViewModel.HebohItems, _mainWindowViewModel.Settings.HebohPluList),
            _ => (null, null)
        };

        if (items == null || pluList == null) return;

        var box = MessageBoxManager
            .GetMessageBoxStandard($"Hapus Item", $"Hapus {group.Name}?", ButtonEnum.YesNo);
        var result = await box.ShowWindowDialogAsync(this);
        if (result is ButtonResult.No or ButtonResult.None) return;

        items.Remove(group);
        pluList.Remove(group.Plu);

        _mainWindowViewModel.SettingsLoader.SaveSettings(_mainWindowViewModel.Settings);
        ViewModel.AppendLog($"[Laporan Sales] PLU {group.Plu} dihapus.");
    }

    [RelayCommand]
    private async void ClearAllItems()
    {
        var (items, pluList) = ViewModel.SelectedTabIndex switch
        {
            0 => (ViewModel.HematItems, _mainWindowViewModel.Settings.HematPluList),
            1 => (ViewModel.MurahItems, _mainWindowViewModel.Settings.MurahPluList),
            2 => (ViewModel.HebohItems, _mainWindowViewModel.Settings.HebohPluList),
            _ => (null, null)
        };

        if (items?.Count <= 0 || pluList?.Count <= 0) return;
        var tabName = ViewModel.SelectedTabIndex switch
        {
            0 => "Hemat Minggu Ini", 1 => "Paling Murah", 2 => "Tebus Heboh", _ => "Tab sekarang"
        };
        var box = MessageBoxManager
            .GetMessageBoxStandard($"Hapus Item", $"Hapus semua item di {tabName}?", ButtonEnum.YesNo);
        var result = await box.ShowWindowDialogAsync(this);
        if (result is ButtonResult.No or ButtonResult.None) return;

        items.Clear();
        pluList.Clear();

        _mainWindowViewModel.SettingsLoader.SaveSettings(_mainWindowViewModel.Settings);
        ViewModel.AppendLog("[Laporan Sales] Daftar item-item dihapus");
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
            ViewModel.AddItem(this);
    }
}
