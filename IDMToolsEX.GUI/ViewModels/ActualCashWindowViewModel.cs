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

    private readonly CultureInfo _idrCulture = new("id-ID");

    // private readonly MainWindowViewModel _mainWindowViewModel;
    [ObservableProperty] private ObservableCollection<CashSummaryRow> _cashSummary = [];

    [ObservableProperty] private DateTimeOffset? _date = DateTimeOffset.Now;
    [ObservableProperty] private decimal? _expectedActualCash = 0;
    [ObservableProperty] private decimal? _modalMoney = 100000;

    [ObservableProperty] private string? _physicalMoney = "0";

    // [ObservableProperty] private string? _salesDeposit = "0";
    [ObservableProperty] private int? _shift = 1;
    [ObservableProperty] private int? _station = 1;
    [ObservableProperty] private decimal? _totalCashout;
    [ObservableProperty] private decimal? _totalChangeCash = 0;
    [ObservableProperty] private decimal? _totalConsumentCash = 0;
    [ObservableProperty] private decimal? _variance = 0;

    public ActualCashWindowViewModel(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _databaseService = databaseService;
        // _mainWindowViewModel = mainWindowViewModel;
    }

    public async Task Initialize()
    {
        // CashSummary =
        // [
        //     new CashSummaryRow { Name = "(i) Total Uang Konsumen", Value = FormatCurrency(0) },
        //     new CashSummaryRow { Name = "(i) Total Uang Kembalian", Value = FormatCurrency(0) },
        //     new CashSummaryRow { Name = "(i) Total Tarik Tunai", Value = FormatCurrency(0) },
        //     new CashSummaryRow
        //         { Name = "(i) Total BA Virtual (Cashout, Struk)", Value = $"{FormatCurrency(0)}, {FormatCurrency(0)}" },
        //     new CashSummaryRow { Name = "(i) Total Nominal Cancel", Value = FormatCurrency(0) },
        //     new CashSummaryRow { Name = "(i) Total Setoran Sales", Value = FormatCurrency(0) },
        //     new CashSummaryRow { Name = "(i) Modal", Value = FormatCurrency(ModalMoney) },
        //     new CashSummaryRow
        //     {
        //         Name = "Aktual Kas, BA Virtual Cashout",
        //         Value = $"{FormatCurrency(0)}, {FormatCurrency(0)}"
        //     },
        //     new CashSummaryRow { Name = "Variance", Value = $"{FormatCurrency(0)}, {FormatCurrency(0)}" }
        // ];
        await CalculateActualCash();
    }

    // [RelayCommand]
    // private async Task GetSalesInfo()
    // {
    //     _mainWindowViewModel.AppendLog("Mendapatkan informasi sales...");
    //     if (!_databaseService.IsConnected)
    //     {
    //         _mainWindowViewModel.AppendLog("Gagal mendapatkan informasi sales! Database tidak terhubung.");
    //         return;
    //     }
    //
    //     var result = await _databaseService.GetExpectedActualCashAsync(Date, Shift ?? 1, Station ?? 1);
    //     TotalConsumentCash = result.totalConsumentCash;
    //     TotalChangeCash = result.totalChangeCash;
    //     ExpectedActualCash = result.totalActualCash;
    //     TotalCashout = await _databaseService.GetTotalCashoutAsync(Date, Shift ?? 1, Station ?? 1);
    // }

    [RelayCommand]
    public async Task CalculateActualCash()
    {
        var expectedActualCash =
            await _databaseService.GetExpectedActualCashAsync(Date ?? DateTimeOffset.Now, Shift ?? 1, Station ?? 1);
        var totalCashout =
            await _databaseService.GetTotalCashoutAsync(Date ?? DateTimeOffset.Now, Shift ?? 1, Station ?? 1);
        var beritaAcaraVirtual =
            await _databaseService.GetTotalBeritaAcaraVirtualAsync(Date ?? DateTimeOffset.Now, Shift ?? 1,
                Station ?? 1);
        var salesDeposit =
            await _databaseService.GetTotalSalesDepositAsync(Date ?? DateTimeOffset.Now, Shift ?? 1, Station ?? 1);
        var totalCancel =
            await _databaseService.GetTotalCancelAsync(Date ?? DateTimeOffset.Now, Shift ?? 1, Station ?? 1);
        var actualCash = expectedActualCash.totalActualCash - (totalCashout.TotalCashout + salesDeposit + beritaAcaraVirtual.Struk);
        var physicalMoney = ParseDecimal(PhysicalMoney);

        CashSummary.Clear();
        CashSummary =
        [
            new CashSummaryRow
                { Name = "ℹ️ Total Uang Konsumen", Value = FormatCurrency(expectedActualCash.totalConsumentCash) },
            new CashSummaryRow
                { Name = "ℹ️ Total Uang Kembalian", Value = FormatCurrency(expectedActualCash.totalChangeCash) },
            new CashSummaryRow { Name = "ℹ️ Total Tarik Tunai, Qty", Value = $"{FormatCurrency(totalCashout.TotalCashout)}, {totalCashout.Count}" },
            new CashSummaryRow
            {
                Name = "ℹ️ Total BA Virtual (Cashout, Struk)",
                Value = $"{FormatCurrency(beritaAcaraVirtual.Cashout)}, {FormatCurrency(beritaAcaraVirtual.Struk)}"
            },
            new CashSummaryRow { Name = "ℹ️ Total Nominal Cancel", Value = FormatCurrency(totalCancel) },
            new CashSummaryRow { Name = "ℹ️ Total Setoran Sales", Value = FormatCurrency(salesDeposit) },
            new CashSummaryRow { Name = "ℹ️ Modal", Value = FormatCurrency(ModalMoney) },
            new CashSummaryRow
            {
                Name = "Aktual Kas, Aktual Kas + BA Virtual (Cashout)",
                Value = $"{FormatCurrency(actualCash)}, {FormatCurrency(actualCash - beritaAcaraVirtual.Cashout)}"
            },
            new CashSummaryRow
            {
                Name = "Variance, Variance + BA Virtual (Cashout)",
                Value =
                    $"{FormatCurrency(physicalMoney - actualCash)}, {FormatCurrency(physicalMoney - (actualCash + beritaAcaraVirtual.Cashout))}"
            }
        ];
    }

    private decimal ParseDecimal(string? input)
    {
        if (decimal.TryParse(input, NumberStyles.Any, _idrCulture, out var result))
            return result;
        return 0;
    }

    private string FormatCurrency(decimal? value)
    {
        return (value ?? 0).ToString("C0", _idrCulture);
    }
}

public partial class CashSummaryRow : ObservableObject
{
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string? _value;
}
