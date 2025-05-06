using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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

    [ObservableProperty] private ObservableCollection<Barcode> _barcodeList = [];
    private int _index;
    [ObservableProperty] private bool _isCommaDelimiter;
    [ObservableProperty] private bool _isNewlineDelimiter = true;
    [ObservableProperty] private bool _isSpaceDelimiter;

    [ObservableProperty] private bool _isOneBarcode = true;
    private string _lastPlu = string.Empty;
    [ObservableProperty] private int _lineCount;
    [ObservableProperty] private ObservableCollection<(string, string)> _modisOptions = [];
    [ObservableProperty] private ObservableCollection<string> _modisShelfNames = [];

    // private readonly MainWindowViewModel _mainWindowViewModel;
    [ObservableProperty] private string _pluList = string.Empty;

    [ObservableProperty] private string _selectedModis;
    [ObservableProperty] private string _selectedModisDescription;
    [ObservableProperty] private string _selectedShelfFrom;
    [ObservableProperty] private string _selectedShelfTo;
    [ObservableProperty] private ObservableCollection<string> _shelfOptions = [];

    [ObservableProperty] private bool _isLoading;
    private CancellationTokenSource _cancellationTokenSource = new();

    private MainWindowViewModel _mainWindowViewModel;

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

    public void Cleanup()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    public void UpdatePluListCount()
    {
        LineCount = PluList?.Split(['\n'], StringSplitOptions.RemoveEmptyEntries).Length ?? 0;
    }

    [RelayCommand]
    private async Task GenerateBarcode()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        IsLoading = true;
        _index = 0;
        _lastPlu = string.Empty;
        BarcodeList.Clear();

        var delimiters = IsNewlineDelimiter ? ['\r', '\n'] :
            IsCommaDelimiter ? [','] :
            IsSpaceDelimiter ? new[] { ' ' } : Array.Empty<char>();

        var pluList = PluList
            .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
            .Select(plu => plu.Trim());

        try
        {
            foreach (var plu in pluList)
            {
                token.ThrowIfCancellationRequested();
                await AddBarcodesToListAsync(plu, token);
            }
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation if needed
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task GenerateBarcodeFromModis()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        IsLoading = true;
        _index = 0;
        BarcodeList.Clear();
        _lastPlu = string.Empty;

        try
        {
            var pluList =
                await _databaseService.GetShelfPluAsync(SelectedModis, SelectedShelfFrom, SelectedShelfTo);
            foreach (var plu in pluList)
            {
                token.ThrowIfCancellationRequested();
                await AddBarcodesToListAsync(plu, token);
            }
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation if needed
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task AddBarcodesToListAsync(string plu, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var details = await _databaseService.GetProductDescriptionAsync(plu);
        var barcodes = (await _databaseService.GetBarcodesAsync(plu)).ToArray();

        if (_lastPlu != plu)
        {
            _index++;
            _lastPlu = plu;
        }

        // If barcodes is empty, add a placeholder barcode
        if (barcodes.Length == 0)
        {
            BarcodeList.Add(new Barcode(_mainWindowViewModel)
            {
                Index = _index,
                Plu = plu,
                Abbreviation = details.Abbreviation,
                Description = details.Description,
                BarcodeText = string.Empty,
                BarcodeImage = null
            });
        }
        else
        {
            foreach (var barcode in barcodes)
            {
                token.ThrowIfCancellationRequested();
                var isInvalid = _mainWindowViewModel.Settings.BadBarcodeList.Any(badBarcode => badBarcode == barcode);

                if (IsOneBarcode && isInvalid)
                    continue;

                var barcodeImage = await BarcodeGenerator.GenerateBarcodeImageAsync(barcode, token);
                BarcodeList.Add(new Barcode(_mainWindowViewModel)
                {
                    Index = _index,
                    Plu = plu,
                    Abbreviation = details.Abbreviation,
                    Description = details.Description,
                    BarcodeText = barcode,
                    BarcodeImage = barcodeImage,
                    IsInvalid = isInvalid
                });

                if (IsOneBarcode)
                    break; // Add only one good barcode
            }
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
    [ObservableProperty] private bool _isInvalid;
    [ObservableProperty] private string _plu = string.Empty;
    [ObservableProperty] private string _rowColor = "Transparent";

    private MainWindowViewModel _mainWindowViewModel;

    public Barcode(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }

    partial void OnIsInvalidChanged(bool value)
    {
        if (value)
        {
            RowColor = "Red";
            _mainWindowViewModel.Settings.BadBarcodeList.Add(BarcodeText);
        }
        else
        {
            RowColor = "Transparent";
            _mainWindowViewModel.Settings.BadBarcodeList.Remove(BarcodeText);
        }

        _mainWindowViewModel.SettingsLoader.SaveSettings(_mainWindowViewModel.Settings);
    }
}
