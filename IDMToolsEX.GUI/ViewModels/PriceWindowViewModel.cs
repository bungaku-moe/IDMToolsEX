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
    [ObservableProperty] private bool _byBarcode;

    [ObservableProperty] private bool _byPlu;

    [ObservableProperty] private ObservableCollection<ItemPrice> _itemPricesList = [];
    [ObservableProperty] private string _searchValue;

    public PriceWindowViewModel(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _databaseService = databaseService;
    }

    [RelayCommand]
    private async Task GetItemPrice()
    {
        if (string.IsNullOrWhiteSpace(SearchValue))
        {
            _mainWindowViewModel.AppendLog("Masukkan PLU/Barcode!");
            return;
        }

        if (ByBarcode)
        {
            var itemDetails = await _databaseService.GetItemPriceByBarcodeAsync(SearchValue);
            _mainWindowViewModel.AppendLog($"Hasil pencarian: {itemDetails}");

            if (ItemPricesList.All(item => item.Plu != SearchValue))
                ItemPricesList.Add(new ItemPrice
                {
                    Description = itemDetails.Description ?? "TIDAK ADA DESKRIPSI",
                    Packaging = itemDetails.Packaging ?? "TIDAK ADA TIPE KEMASAN",
                    Price = FormatCurrency(itemDetails.Price),
                    Plu = itemDetails.Plu ?? "TIDAK ADA PLU",
                    Barcode = SearchValue
                });
            else
                _mainWindowViewModel.AppendLog($"Item {SearchValue} sudah ada di daftar.");
        }
        else if (ByPlu)
        {
            var itemDetails = await _databaseService.GetItemDetailsByPluAsync(SearchValue);

            if (ItemPricesList.All(item => item.Plu != SearchValue))
            {
                _mainWindowViewModel.AppendLog($"Hasil pencarian harga: {itemDetails}");
                ItemPricesList.Add(new ItemPrice
                {
                    Description = itemDetails.Description ?? "TIDAK ADA DESKRIPSI",
                    Packaging = itemDetails.Packaging ?? "TIDAK ADA TIPE KEMASAN",
                    Price = FormatCurrency(itemDetails.Price),
                    Plu = SearchValue,
                    Barcode = string.Join(", ", itemDetails.Barcodes)
                });
            }
            else
                _mainWindowViewModel.AppendLog($"Item {SearchValue} sudah ada di daftar.");
        }
        else
        {
            _mainWindowViewModel.AppendLog("Tidak ada hasil ditemukan.");
        }
    }

    private string FormatCurrency(decimal? value)
    {
        return (value ?? 0).ToString("C0", _idrCulture); // IDR format, no decimal places
    }
}

public partial class ItemPrice : ObservableObject
{
    [ObservableProperty] private string _barcode = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private string _packaging = string.Empty;
    [ObservableProperty] private string _plu = string.Empty;
    [ObservableProperty] private string _price = string.Empty;
}
