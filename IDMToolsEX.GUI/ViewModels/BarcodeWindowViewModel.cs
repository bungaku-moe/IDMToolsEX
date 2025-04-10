using System;
using System.Collections.ObjectModel;
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

    [ObservableProperty] private string _pluList = string.Empty;
    public ObservableCollection<Barcode> BarcodeList { get; set; } = [];

    public ObservableCollection<string> RakOptions { get; set; } = [];
    [ObservableProperty] private string _selectedRakFirst;
    [ObservableProperty] private string _selectedRakLast;

    public ObservableCollection<string> ModisOptions { get; set; } = [];
    [ObservableProperty] private string _selectedModisFirst;
    [ObservableProperty] private string _selectedModisLast;

    public BarcodeWindowViewModel(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _databaseService = databaseService;
    }

    [RelayCommand]
    private async Task GenerateBarcode()
    {
        BarcodeList.Clear();
        var pluArray = PluList.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        foreach (var plu in pluArray)
        {
            var details = await _databaseService.GetDescriptionAsync(plu);
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
