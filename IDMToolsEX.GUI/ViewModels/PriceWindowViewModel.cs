using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;

namespace IDMToolsEX.ViewModels;

public partial class PriceWindowViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService;
    private readonly CultureInfo _idrCulture = new("id-ID");
    private readonly MainWindowViewModel _mainWindowViewModel;
    [ObservableProperty] private bool _byBarcode = true;
    [ObservableProperty] private bool _byPlu;

    [ObservableProperty] private ObservableCollection<ItemPrice> _itemPricesList = [];
    [ObservableProperty] private string _searchValue;

    public PriceWindowViewModel(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _databaseService = databaseService;
    }

    [RelayCommand]
    public async Task GetItemPrice()
    {
        if (string.IsNullOrWhiteSpace(SearchValue))
            return;

        if (ByBarcode)
        {
            var itemDetails = await _databaseService.GetItemPriceByBarcodeAsync(SearchValue);
            if (itemDetails.Description == null)
            {
                _mainWindowViewModel.AppendLog($"Tidak dapat menemukan item {SearchValue}!");
                SearchValue = string.Empty;
                return;
            }

            var promoPrice = await _databaseService.GetPromotionByBarcodeAsync(SearchValue);
            var expired = await _databaseService.GetExpiredByPluAsync(itemDetails.Plu);

            if (ItemPricesList.Any(item => item.Plu == SearchValue))
            {
                var existingItem = ItemPricesList.First(item => item.Plu == SearchValue);
                ItemPricesList.Remove(existingItem);
            }

            _mainWindowViewModel.AppendLog($"[PRICE TAG] Hasil pencarian: {itemDetails}");
            ItemPricesList.Add(new ItemPrice
            {
                Description = itemDetails.Description,
                Price = FormatCurrency(itemDetails.Price),
                PricePromo = FormatCurrency(promoPrice.Value.Price - promoPrice.Value.Promo),
                Start = promoPrice.Value.Start.HasValue
                    ? DateOnly.FromDateTime(promoPrice.Value.Start.Value.DateTime)
                    : null,
                End = promoPrice.Value.End.HasValue
                    ? DateOnly.FromDateTime(promoPrice.Value.End.Value.DateTime)
                    : null,
                Plu = itemDetails.Plu,
                Expired = expired,
                Barcode = SearchValue
            });
        }
        else if (ByPlu)
        {
            var itemDetails = await _databaseService.GetItemDetailsByPluAsync(SearchValue);
            if (itemDetails.Description == null)
            {
                _mainWindowViewModel.AppendLog($"Tidak dapat menemukan item {SearchValue}!");
                SearchValue = string.Empty;
                return;
            }

            var promoPrice = await _databaseService.GetPromotionByPluAsync(SearchValue);
            var expired = await _databaseService.GetExpiredByPluAsync(SearchValue);

            if (ItemPricesList.Any(item => item.Plu == SearchValue))
            {
                var existingItem = ItemPricesList.First(item => item.Plu == SearchValue);
                ItemPricesList.Remove(existingItem);
            }

            _mainWindowViewModel.AppendLog($"[PRICE TAG] Hasil pencarian: {itemDetails}");
            ItemPricesList.Add(new ItemPrice
            {
                Description = itemDetails.Description,
                Price = FormatCurrency(itemDetails.Price),
                PricePromo = FormatCurrency(promoPrice.Value.Price - promoPrice.Value.Promo),
                Start = promoPrice.Value.Start.HasValue
                    ? DateOnly.FromDateTime(promoPrice.Value.Start.Value.DateTime)
                    : null,
                End = promoPrice.Value.End.HasValue
                    ? DateOnly.FromDateTime(promoPrice.Value.End.Value.DateTime)
                    : null,
                Plu = SearchValue,
                Expired = expired,
                Barcode = string.Join(", ", itemDetails.Barcodes)
            });
        }

        SearchValue = string.Empty;
    }

    private string FormatCurrency(decimal? value)
    {
        return (value ?? 0).ToString("C0", _idrCulture);
    }
}

public partial class ItemPrice : ObservableObject
{
    [ObservableProperty] private string _barcode = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private DateOnly? _end;
    [ObservableProperty] private string _plu = string.Empty;
    [ObservableProperty] private string? _price = "0";
    [ObservableProperty] private string? _pricePromo = "0";
    [ObservableProperty] private DateOnly? _start;
    [ObservableProperty] private string _expired = string.Empty;
    public string PromotionColor => Start != null ? "Yellow" : "Transparent";
}
