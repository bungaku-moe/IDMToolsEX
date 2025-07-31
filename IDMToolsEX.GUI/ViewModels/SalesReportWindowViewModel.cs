using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;
using IDMToolsEX.Models;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace IDMToolsEX.ViewModels;

public partial class SalesReportWindowViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService;
    private readonly MainWindowViewModel _mainWindowViewModel;

    [ObservableProperty] private ObservableCollection<GroupedSaleItem> _hematItems = [];
    [ObservableProperty] private ObservableCollection<GroupedSaleItem> _murahItems = [];
    [ObservableProperty] private ObservableCollection<GroupedSaleItem> _hebohItems = [];

    [ObservableProperty] private DateTimeOffset _date = DateTimeOffset.Now;
    [ObservableProperty] private int _selectedTabIndex;
    [ObservableProperty] private int _shift = 1;
    [ObservableProperty] private string? _searchValue;

    [ObservableProperty] private bool _isLoading;
    // private readonly CultureInfo _idrCulture = new("id-ID"

    public SalesReportWindowViewModel(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _databaseService = databaseService;
    }

    public async Task Initialize()
    {
        LoadAllItems();
    }

    [RelayCommand]
    public async Task AddItem(Window window)
    {
        if (string.IsNullOrWhiteSpace(SearchValue) || string.IsNullOrEmpty(SearchValue))
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Tambah Item", "Masukkan nomor PLU!");
            var result = await box.ShowWindowDialogAsync(window);
            if (result is ButtonResult.Ok or ButtonResult.None) return;
        }

        var targetList = SelectedTabIndex switch
        {
            0 => HematItems,
            1 => MurahItems,
            2 => HebohItems,
            _ => null
        };

        if (targetList == null || targetList.Any(item => item.Plu == SearchValue))
        {
            _mainWindowViewModel.AppendLog("[Laporan Sales] PLU sudah ada di daftar!");
            var box = MessageBoxManager
                .GetMessageBoxStandard("Tambah Item", $"PLU: {SearchValue} sudah ada di daftar!");
            var result = await box.ShowWindowDialogAsync(window);
            if (result is ButtonResult.Ok or ButtonResult.None) return;
        }

        var itemDetails = await _databaseService.GetItemDetailsByPluAsync(SearchValue);
        if (string.IsNullOrEmpty(itemDetails.Description))
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard($"Tambah Item", $"Tidak ada item dengan PLU: {SearchValue}! Tetap tambahkan?", ButtonEnum.YesNo);
            var result = await box.ShowWindowDialogAsync(window);
            if (result is ButtonResult.No or ButtonResult.None) return;
        }

        var transactionDetails = await _databaseService.GetTransactionDetailsAsync(SearchValue, Date, Shift);
        var purchaseHistory = new ObservableCollection<SaleItem>(
            transactionDetails.Select(transaction => new SaleItem
            {
                Plu = SearchValue,
                Name = itemDetails.Description,
                Qty = transaction.Qty,
                Time = transaction.Time
            })
        );

        // foreach (var item in targetList)
        // {
        //     var newItemName = itemDetails.Description.Split(',')[0].Trim();
        //     if (item.Name = newItemName)
        // }

        targetList.Add(new GroupedSaleItem
        {
            Plu = SearchValue,
            Name = itemDetails.Description,
            Items = purchaseHistory
        });

        SortItems();

        var settingsList = SelectedTabIndex switch
        {
            0 => _mainWindowViewModel.Settings.HematPluList,
            1 => _mainWindowViewModel.Settings.MurahPluList,
            2 => _mainWindowViewModel.Settings.HebohPluList,
            _ => null
        };

        settingsList?.Add(SearchValue);

        _mainWindowViewModel.SettingsLoader.SaveSettings(_mainWindowViewModel.Settings);
        _mainWindowViewModel.AppendLog($"[Laporan Sales] PLU {SearchValue} ditambahkan.");
        SearchValue = string.Empty;
    }

    public async Task LoadAllItems()
    {
        IsLoading = true;

        for (var i = 0; i < 3; i++)
        {
            SelectedTabIndex = i;
            await LoadItems();
        }

        SortItems();
        SelectedTabIndex = 0;
        IsLoading = false;
    }

    private async Task LoadItems()
    {
        _mainWindowViewModel.AppendLog("[Laporan Sales] Memuat PLU yang disimpan...");

        var targetList = SelectedTabIndex switch
        {
            0 => HematItems,
            1 => MurahItems,
            2 => HebohItems,
            _ => null
        };

        var settingsList = SelectedTabIndex switch
        {
            0 => _mainWindowViewModel.Settings.HematPluList,
            1 => _mainWindowViewModel.Settings.MurahPluList,
            2 => _mainWindowViewModel.Settings.HebohPluList,
            _ => null
        };

        if (targetList == null || settingsList == null) return;

        targetList.Clear();

        foreach (var plu in settingsList)
        {
            var itemDetails = await _databaseService.GetItemDetailsByPluAsync(plu);
            var transactionDetails = await _databaseService.GetTransactionDetailsAsync(plu, Date, Shift);

            var itemList = new ObservableCollection<SaleItem>(
                transactionDetails.Select(transaction => new SaleItem
                {
                    Plu = plu,
                    Name = itemDetails.Description,
                    Qty = transaction.Qty,
                    Time = transaction.Time
                })
            );

            targetList.Add(new GroupedSaleItem
            {
                Plu = plu,
                Name = itemDetails.Description,
                Items = itemList,
                TotalQty = itemList.Sum(i => i.Qty)
            });
        }
    }

    [RelayCommand]
    public void SortItems()
    {
        void SortList(ObservableCollection<GroupedSaleItem> list)
        {
            var sorted = list.OrderBy(x => x.Name).ToList();
            list.Clear();
            foreach (var item in sorted)
                list.Add(item);
        }

        SortList(HematItems);
        SortList(MurahItems);
        SortList(HebohItems);
    }

    // private string FormatCurrency(decimal? value)
    // {
    //     return (value ?? 0).ToString("C0", _idrCulture);
    // }
}
