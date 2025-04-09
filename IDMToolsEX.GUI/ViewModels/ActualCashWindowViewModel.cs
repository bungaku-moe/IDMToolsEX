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
    [ObservableProperty] private string? _salesDeposit = "0";
    [ObservableProperty] private string? _actualCash = "0";
    [ObservableProperty] private int _shift = 1;
    [ObservableProperty] private int _station = 1;
    [ObservableProperty] private decimal? _totalConsumentCash = 0;
    [ObservableProperty] private decimal? _totalChangeCash = 0;
    [ObservableProperty] private decimal? _expectedActualCash = 0;
    [ObservableProperty] private decimal? _variance = 0;
    [ObservableProperty] private decimal? _totalCashout;
    [ObservableProperty] private decimal? _cashModal = 100000;

    public ObservableCollection<CashSummaryRow> CashSummaryRows { get; set; } = [];

    public ActualCashWindowViewModel(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _databaseService = databaseService;
        _mainWindowViewModel = mainWindowViewModel;

        CashSummaryRows.Add(new CashSummaryRow { Name = "(i) Total Uang Konsumen", Value = FormatCurrency(TotalConsumentCash) });
        CashSummaryRows.Add(new CashSummaryRow { Name = "(i) Total Uang Kembalian", Value = FormatCurrency(TotalChangeCash) });
        CashSummaryRows.Add(new CashSummaryRow { Name = "(i) Total Tarik Tunai", Value = FormatCurrency(TotalCashout) });
        CashSummaryRows.Add(new CashSummaryRow { Name = "(i) Modal", Value = FormatCurrency(CashModal) });
        CashSummaryRows.Add(new CashSummaryRow { Name = "Aktual Kas", Value = FormatCurrency(ExpectedActualCash) });
        CashSummaryRows.Add(new CashSummaryRow { Name = "Variance", Value = FormatCurrency(Variance) });
    }

    [RelayCommand]
    private async Task GetSalesInfo()
    {
        _mainWindowViewModel.AppendLog("Mendapatkan informasi sales...");
        if (!_databaseService.IsConnected)
        {
            _mainWindowViewModel.AppendLog("Gagal mendapatkan informasi sales! Database tidak terhubung.");
            return;
        }

        var result = await _databaseService.GetExpectedActualCashAsync(Date, Shift, Station);
        TotalConsumentCash = result.totalConsumentCash;
        TotalChangeCash = result.totalChangeCash;
        ExpectedActualCash = result.totalActualCash;
        TotalCashout = await _databaseService.GetTotalCashoutAsync(Date, Shift, Station);
    }

    [RelayCommand]
    private void CalculateActualCash()
    {
        var salesDepositDecimal = ParseDecimal(SalesDeposit);
        var actualCashDecimal = ParseDecimal(ActualCash);

        ExpectedActualCash = TotalConsumentCash - (TotalChangeCash + salesDepositDecimal + TotalCashout + CashModal);
        Variance = actualCashDecimal - ExpectedActualCash;
    }

    partial void OnTotalConsumentCashChanged(decimal? value)
    {
        CashSummaryRows[0].Value = FormatCurrency(value);
        CashSummaryRows[0].NotifyValueChanged();
    }

    partial void OnTotalChangeCashChanged(decimal? value)
    {
        CashSummaryRows[1].Value = FormatCurrency(value);
        CashSummaryRows[1].NotifyValueChanged();
    }

    partial void OnTotalCashoutChanged(decimal? value)
    {
        CashSummaryRows[2].Value = FormatCurrency(value);
        CashSummaryRows[2].NotifyValueChanged();
    }

    partial void OnCashModalChanged(decimal? value)
    {
        CashSummaryRows[3].Value = FormatCurrency(value);
        CashSummaryRows[3].NotifyValueChanged();
    }

    partial void OnExpectedActualCashChanged(decimal? value)
    {
        CashSummaryRows[4].Value = FormatCurrency(value);
        CashSummaryRows[4].NotifyValueChanged();
    }

    partial void OnVarianceChanged(decimal? value)
    {
        CashSummaryRows[5].Value = FormatCurrency(value);
        CashSummaryRows[5].NotifyValueChanged();
    }

    private decimal ParseDecimal(string? input)
    {
        if (decimal.TryParse(input, NumberStyles.Any, _idrCulture, out var result))
            return result;
        return 0;
    }

    private string FormatCurrency(decimal? value)
    {
        return (value ?? 0).ToString("C0", _idrCulture); // IDR format, no decimal places
    }
}

public partial class CashSummaryRow : ObservableObject
{
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string? _value;

    public void NotifyValueChanged()
    {
        OnPropertyChanged(nameof(Value));
    }
}
