using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;
using IDMToolsEX.Models;

namespace IDMToolsEX.ViewModels;

public partial class SalesReportWindowViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService;

    // private readonly CultureInfo _idrCulture = new("id-ID");
    private readonly MainWindowViewModel _mainWindowViewModel;

    [ObservableProperty] private ObservableCollection<GroupedSaleItem> _groupedItemsList = [];

    // [ObservableProperty] private ObservableCollection<SaleItem> _itemsList = [];
    [ObservableProperty] private DateTimeOffset _date = DateTimeOffset.Now;
    [ObservableProperty] private int _shift = 1;
    [ObservableProperty] private string? _searchValue;

    public SalesReportWindowViewModel(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _databaseService = databaseService;
    }

    public async Task Initialize()
    {
        // await LoadItems();
    }

    [RelayCommand]
    private async Task AddItem()
    {
        if (string.IsNullOrWhiteSpace(SearchValue))
        {
            _mainWindowViewModel.AppendLog("Masukkan PLU!");
            return;
        }

        if (GroupedItemsList.Any(item => item.Plu == SearchValue))
        {
            _mainWindowViewModel.AppendLog("PLU sudah ada di daftar!");
            return;
        }

        var itemDetails = await _databaseService.GetItemDetailsByPluAsync(SearchValue);
        var transactionDetails = await _databaseService.GetTransactionDetailsAsync(SearchValue, Date, Shift);
        ObservableCollection<SaleItem> itemList = [];

        foreach (var transaction in transactionDetails)
        {
            itemList.Add(
                new SaleItem
                {
                    Plu = SearchValue,
                    Name = itemDetails.Description,
                    Qty = transaction.Qty,
                    // Price = transaction?.Price ?? 0,
                    Time = transaction.Time
                    // Rtype = transaction?.Rtype
                }
            );
        }

        GroupedItemsList.Add(
            new GroupedSaleItem
            {
                Plu = SearchValue,
                Name = itemDetails.Description,
                Items = itemList
            }
        );

        _mainWindowViewModel.Settings.SalesReportPluList.Add(SearchValue);
        _mainWindowViewModel.SettingsLoader.SaveSettings(_mainWindowViewModel.Settings);
        _mainWindowViewModel.AppendLog($"PLU {SearchValue} ditambahkan.");
        SearchValue = string.Empty;
    }

    public async Task LoadItems()
    {
        _mainWindowViewModel.AppendLog("Memuat PLU yang disimpan...!");

        GroupedItemsList.Clear();
        foreach (var plu in _mainWindowViewModel.Settings.SalesReportPluList)
        {
            var itemDetails = await _databaseService.GetItemDetailsByPluAsync(plu);
            var transactionDetails = await _databaseService.GetTransactionDetailsAsync(plu, Date, Shift);
            ObservableCollection<SaleItem> itemList = [];

            foreach (var transaction in transactionDetails)
            {
                itemList.Add(
                    new SaleItem
                    {
                        Plu = SearchValue,
                        Name = itemDetails.Description,
                        Qty = transaction.Qty,
                        // Price = transaction?.Price ?? 0,
                        Time = transaction.Time
                        // Rtype = transaction?.Rtype
                    }
                );
            }

            GroupedItemsList.Add(
                new GroupedSaleItem
                {
                    Plu = plu,
                    Name = itemDetails.Description,
                    Items = itemList,
                    ItemsTotalQty = itemList.Sum(i => i.Qty)
                });
        }
    }

    // private string FormatCurrency(decimal? value)
    // {
    //     return (value ?? 0).ToString("C0", _idrCulture);
    // }
}

public partial class GroupedSaleItem : ObservableObject
{
    [ObservableProperty] private string? _plu;
    [ObservableProperty] private string? _name;
    [ObservableProperty] private ObservableCollection<SaleItem>? _items;
    [ObservableProperty] private int? _itemsTotalQty;
}
