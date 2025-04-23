using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;
using IDMToolsEX.Utility;

namespace IDMToolsEX.ViewModels;

public partial class BarcodeWindowViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService;
    private readonly MainWindowViewModel _mainWindowViewModel;
    [ObservableProperty] private ObservableCollection<Barcode> _barcodeList = [];
    private int _index;

    [ObservableProperty] private bool _isOneBarcode = true;
    private string _lastPlu = string.Empty;
    [ObservableProperty] private ObservableCollection<(string, string)> _modisOptions = [];
    [ObservableProperty] private ObservableCollection<string> _modisShelfNames = [];
    [ObservableProperty] private string _pluList = string.Empty;
    [ObservableProperty] private string _selectedModis;
    [ObservableProperty] private string _selectedModisDescription;
    [ObservableProperty] private string _selectedShelfFrom;
    [ObservableProperty] private string _selectedShelfTo;

    [ObservableProperty] private ObservableCollection<string> _shelfOptions = [];

    public BarcodeWindowViewModel(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _databaseService = databaseService;
    }

    public async Task InitializeAsync()
    {
        ModisOptions = new ObservableCollection<(string, string)>(await _databaseService.GetModisAsync());
        SelectedModis = ModisOptions[0].Item1;
        SelectedModisDescription = ModisOptions[0].Item2;
        ModisShelfNames = new ObservableCollection<string>(ModisOptions.Select(option => option.Item1));

        await GetModisShelfsAsync(SelectedModis);
        SelectedShelfFrom = ShelfOptions[0];
        SelectedShelfTo = ShelfOptions[^1];
    }

    [RelayCommand]
    private async Task GenerateBarcode()
    {
        _index = 0;
        _lastPlu = string.Empty;
        BarcodeList.Clear();
        var pluList = PluList.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        foreach (var plu in pluList) await AddBarcodesToListAsync(plu);
    }

    [RelayCommand]
    private async Task GenerateBarcodeFromModis()
    {
        _index = 0;
        BarcodeList.Clear();
        _lastPlu = string.Empty;
        var pluList = await _databaseService.GetShelfPluAsync(SelectedModis, SelectedShelfFrom, SelectedShelfTo);

        foreach (var plu in pluList) await AddBarcodesToListAsync(plu);
    }

    private async Task AddBarcodesToListAsync(string plu)
    {
        var details = await _databaseService.GetProductDescriptionAsync(plu);
        var barcodes = (await _databaseService.GetBarcodesAsync(plu)).ToArray();
        // var enumerable = barcodes as string[] ?? barcodes.ToArray();

        // Check if this is a new PLU
        if (_lastPlu != plu)
        {
            _index++;
            _lastPlu = plu;
        }

        if (IsOneBarcode && barcodes.Length != 0)
            foreach (var barcode in barcodes)
            {
                if (string.IsNullOrEmpty(barcode)) continue;

                var barcodeImage = await BarcodeGenerator.GenerateBarcodeImageAsync(barcode);
                BarcodeList.Add(new Barcode
                {
                    Index = _index,
                    Plu = plu,
                    Abbreviation = details.Abbreviation ?? "TIDAK ADA SINGKATAN",
                    Description = details.Description ?? "TIDAK ADA DESKRIPSI",
                    BarcodeText = barcode,
                    BarcodeImage = barcodeImage
                });
                break;
            }
        else
            foreach (var barcode in barcodes)
            {
                var barcodeImage = await BarcodeGenerator.GenerateBarcodeImageAsync(barcode);
                BarcodeList.Add(new Barcode
                {
                    Index = _index,
                    Plu = plu,
                    Abbreviation = details.Abbreviation ?? "TIDAK ADA SINGKATAN",
                    Description = details.Description ?? "TIDAK ADA DESKRIPSI",
                    BarcodeText = barcode,
                    BarcodeImage = barcodeImage
                });
            }
    }


    public async Task GetModisShelfsAsync(string selectedItem)
    {
        var shelfNumbers = await _databaseService.GetShelfNumbersAsync(selectedItem);
        ShelfOptions = new ObservableCollection<string>(shelfNumbers);

        // Update SelectedModisDescription when SelectedModis changes
        var selectedModisDetails = ModisOptions.FirstOrDefault(modis => modis.Item1 == selectedItem);
        SelectedModisDescription = selectedModisDetails.Item2;

        SelectedShelfFrom = ShelfOptions[0];
        SelectedShelfTo = ShelfOptions[^1];
    }
}

public partial class Barcode : ObservableObject
{
    [ObservableProperty] private string _abbreviation = string.Empty;
    [ObservableProperty] private Bitmap? _barcodeImage;
    [ObservableProperty] private string _barcodeText = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private int _index;
    [ObservableProperty] private string _plu = string.Empty;
    [ObservableProperty] private bool _isInvalid;
}
