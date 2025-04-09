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
    private readonly MainWindowViewModel _mainWindowViewModel;
    private readonly DatabaseService _databaseService;

    [ObservableProperty] private string _pluList = string.Empty;
    public ObservableCollection<Barcode> BarcodeList { get; set; } = new();

    public BarcodeWindowViewModel(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _databaseService = databaseService;
    }

    [RelayCommand]
    private async Task GenerateBarcodeCommand()
    {
        var pluArray = PluList.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string plu in pluArray)
        {
            var barcodeText = plu; // Use PLU as the barcode text
            // var barcodeImage =
            //     BarcodeGenerator.GenerateBarcodeImage(barcodeText); // Generate barcode image using ZXing.Net

            BarcodeList.Add(new Barcode
            {
                Plu = plu,
                BarcodeText = barcodeText
            });
        }
    }
}

public partial class Barcode : ObservableObject
{
    [ObservableProperty] private string _plu = string.Empty;
    [ObservableProperty] private string _barcodeText = string.Empty;
    [ObservableProperty] private Bitmap? _barcodeImage;
}
