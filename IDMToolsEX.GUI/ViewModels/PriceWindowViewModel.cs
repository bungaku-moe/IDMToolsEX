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
        {
            _mainWindowViewModel.AppendLog("Masukkan PLU/Barcode!");
            return;
        }

        if (ByBarcode)
        {
            var itemDetails = await _databaseService.GetItemPriceByBarcodeAsync(SearchValue);
            var promoPrice = await _databaseService.GetPromotionByBarcodeAsync(SearchValue);

            if (ItemPricesList.All(item => item.Plu != SearchValue))
            {
                _mainWindowViewModel.AppendLog($"Hasil pencarian: {itemDetails}");
                if (promoPrice.HasValue)
                    ItemPricesList.Add(new ItemPrice
                    {
                        Description = itemDetails.Description ?? "TIDAK ADA DESKRIPSI",
                        Price = FormatCurrency(itemDetails.Price),
                        PricePromo = FormatCurrency(promoPrice.Value.Price - promoPrice.Value.Promo),
                        Start = promoPrice.Value.Start.HasValue
                            ? DateOnly.FromDateTime(promoPrice.Value.Start.Value.DateTime)
                            : null,
                        End = promoPrice.Value.End.HasValue
                            ? DateOnly.FromDateTime(promoPrice.Value.End.Value.DateTime)
                            : null,
                        Plu = itemDetails.Plu ?? "TIDAK ADA PLU",
                        Barcode = SearchValue
                    });
                else
                    _mainWindowViewModel.AppendLog("Promo price data is not available.");
            }
            else
            {
                _mainWindowViewModel.AppendLog($"Item {SearchValue} sudah ada di daftar.");
            }
        }
        else if (ByPlu)
        {
            var itemDetails = await _databaseService.GetItemDetailsByPluAsync(SearchValue);
            var promoPrice = await _databaseService.GetPromotionByPluAsync(SearchValue);

            if (ItemPricesList.All(item => item.Plu != SearchValue))
            {
                _mainWindowViewModel.AppendLog($"Hasil pencarian harga: {itemDetails}");
                ItemPricesList.Add(new ItemPrice
                {
                    Description = itemDetails.Description ?? "TIDAK ADA DESKRIPSI",
                    Price = FormatCurrency(itemDetails.Price),
                    PricePromo = FormatCurrency(promoPrice.Value.Price - promoPrice.Value.Promo),
                    Start = promoPrice.Value.Start.HasValue
                        ? DateOnly.FromDateTime(promoPrice.Value.Start.Value.DateTime)
                        : null,
                    End = promoPrice.Value.End.HasValue
                        ? DateOnly.FromDateTime(promoPrice.Value.End.Value.DateTime)
                        : null,
                    Plu = SearchValue,
                    Barcode = string.Join(", ", itemDetails.Barcodes)
                });
            }
            else
            {
                _mainWindowViewModel.AppendLog($"Item {SearchValue} sudah ada di daftar.");
            }
        }
        else
        {
            _mainWindowViewModel.AppendLog("Tidak ada hasil ditemukan.");
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
    [ObservableProperty] private string _barcode = "TIDAK ADA BARCODE";
    [ObservableProperty] private string _description = "TIDAK ADA DESKRIPSI";
    [ObservableProperty] private DateOnly? _end;
    [ObservableProperty] private string _plu = "TIDAK ADA PLU";
    [ObservableProperty] private string? _price = "0";
    [ObservableProperty] private string? _pricePromo = "0";
    [ObservableProperty] private DateOnly? _start;
    public string PromotionColor => Start != null ? "Yellow" : "Transparent";
}
