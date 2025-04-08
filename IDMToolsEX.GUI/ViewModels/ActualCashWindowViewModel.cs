using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;

namespace IDMToolsEX.ViewModels;

public partial class ActualCashWindowViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService;
    private readonly MainWindowViewModel _mainWindowViewModel;
    private readonly CultureInfo _idrCulture = new("id-ID");

    [ObservableProperty] private DateTimeOffset _date = DateTimeOffset.Now;
    [ObservableProperty] private int _shift = 1;
    [ObservableProperty] private int _station = 1;

    [ObservableProperty] private string? _salesDeposit = "0";
    [ObservableProperty] private decimal? _totalConsumentCash = 0;
    [ObservableProperty] private decimal? _totalChangeCash = 0;
    [ObservableProperty] private decimal? _totalDebitCashout = 0;
    [ObservableProperty] private decimal? _cashModal = 100000;

    [ObservableProperty] private string? _cashierActualCash = "0";
    [ObservableProperty] private decimal? _expectedActualCash = 0;
    [ObservableProperty] private decimal? _variance = 0;

    public ObservableCollection<CashSummaryRow> CashSummaryRows { get; } = new()
    {
        new CashSummaryRow { Name = "🛈 Total Uang Konsumen" },
        new CashSummaryRow { Name = "🛈 Total Uang Kembalian" },
        new CashSummaryRow { Name = "🛈 Total Tarik Tunai Debit" },
        new CashSummaryRow { Name = "🛈 Modal" },
        new CashSummaryRow { Name = "Aktual Kas" },
        new CashSummaryRow { Name = "Variance" }
    };

    public ActualCashWindowViewModel(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _databaseService = databaseService;
        _mainWindowViewModel = mainWindowViewModel;
        UpdateCashSummaryRows();
    }

    [RelayCommand]
    private async Task GetSalesInfo()
    {
        _mainWindowViewModel.AppendLog("Mendapatkan informasi sales...");
        if (!_databaseService.IsConnected)
        {
            _mainWindowViewModel.AppendLog("Gagal mendapatkan infomasi sales. Koneksi ke database terputus!");
            return;
        }

        var actualCashResult = await _databaseService.GetActualCashAsync(Date, Shift, Station);
        TotalConsumentCash = actualCashResult.totalConsumentCash;
        TotalChangeCash = actualCashResult.totalChangeCash;
        ExpectedActualCash = actualCashResult.totalExpectedCash;
        TotalDebitCashout = await _databaseService.GetTotalDebitCashoutAsync(Date, Shift, Station);
    }

    [RelayCommand]
    private void CalculateActualCash()
    {
        var salesDepositDecimal = ParseDecimal(SalesDeposit);
        var actualCashDecimal = ParseDecimal(CashierActualCash);

        ExpectedActualCash = TotalConsumentCash - (CashModal + TotalChangeCash + salesDepositDecimal + TotalDebitCashout);
        Variance = actualCashDecimal - ExpectedActualCash;
    }

    private void UpdateCashSummaryRows()
    {
        CashSummaryRows[0].Value = FormatCurrency(TotalConsumentCash);
        CashSummaryRows[1].Value = FormatCurrency(TotalChangeCash);
        CashSummaryRows[2].Value = FormatCurrency(TotalDebitCashout);
        CashSummaryRows[3].Value = FormatCurrency(CashModal);
        CashSummaryRows[4].Value = FormatCurrency(ExpectedActualCash);
        CashSummaryRows[5].Value = FormatCurrency(Variance);
    }

    private decimal ParseDecimal(string? input) =>
        decimal.TryParse(input, NumberStyles.Any, _idrCulture, out var result) ? result : 0;

    private string FormatCurrency(decimal? value) =>
        (value ?? 0).ToString("C0", _idrCulture); // IDR format, no decimal places
}

public partial class CashSummaryRow : ObservableObject
{
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string? _value;
}
