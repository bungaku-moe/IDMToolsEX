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
    [ObservableProperty] private ObservableCollection<(string, string)> _modisOptions = [];
    [ObservableProperty] private ObservableCollection<string> _modisShelfNames = [];
    [ObservableProperty] private string _pluList = string.Empty;

    [ObservableProperty] private ObservableCollection<string> _rakOptions = [];
    [ObservableProperty] private string _selectedModis;
    [ObservableProperty] private string _selectedModisDescription;
    [ObservableProperty] private string _selectedShelfFrom;
    [ObservableProperty] private string _selectedShelfTo;

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
        SelectedShelfFrom = RakOptions[0];
        SelectedShelfTo = RakOptions[0];
    }

    [RelayCommand]
    private async Task GenerateBarcode()
    {
        BarcodeList.Clear();
        var pluList = PluList.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        foreach (var plu in pluList) await AddBarcodesToListAsync(plu);
    }

    [RelayCommand]
    private async Task GenerateBarcodeFromModis()
    {
        BarcodeList.Clear();
        var pluList = await _databaseService.GetShelfPluAsync(SelectedModis, SelectedShelfFrom, SelectedShelfTo);

        foreach (var plu in pluList) await AddBarcodesToListAsync(plu);
    }

    private async Task AddBarcodesToListAsync(string plu)
    {
        var details = await _databaseService.GetProductDescriptionAsync(plu);
        var barcodes = await _databaseService.GetBarcodesAsync(plu);

        foreach (var barcode in barcodes)
        {
            var barcodeImage = await BarcodeGenerator.GenerateBarcodeImageAsync(barcode);
            BarcodeList.Add(new Barcode
            {
                Plu = plu,
                Abbreviation = details.Abbreviation ?? "NO ABBREVIATION",
                Description = details.Description ?? "NO DESCRIPTION",
                BarcodeText = barcode,
                BarcodeImage = barcodeImage
            });
        }
    }

    public async Task GetModisShelfsAsync(string selectedItem)
    {
        var shelfNumbers = await _databaseService.GetShelfNumbersAsync(selectedItem);
        RakOptions = new ObservableCollection<string>(shelfNumbers);

        // Update SelectedModisDescription when SelectedModis changes
        var selectedModisDetails = ModisOptions.FirstOrDefault(modis => modis.Item1 == selectedItem);
        SelectedModisDescription = selectedModisDetails.Item2;
    }
}

public partial class Barcode : ObservableObject
{
    [ObservableProperty] private string _abbreviation = string.Empty;
    [ObservableProperty] private Bitmap? _barcodeImage;
    [ObservableProperty] private string _barcodeText = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private string _plu = string.Empty;
}
