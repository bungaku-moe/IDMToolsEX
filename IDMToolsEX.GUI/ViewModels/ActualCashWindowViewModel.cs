using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;

namespace IDMToolsEX.ViewModels;

public partial class ActualCashWindowViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService;
    private readonly MainWindowViewModel _mainWindowViewModel;
    [ObservableProperty] private decimal? _actualCash = 0;
    [ObservableProperty] private DateTimeOffset _date = DateTimeOffset.Now;
    [ObservableProperty] private int _shift = 1;
    [ObservableProperty] private int _station = 1;

    public ActualCashWindowViewModel(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _databaseService = databaseService;
        _mainWindowViewModel = mainWindowViewModel;
    }

    [RelayCommand]
    private async Task GetActualCash()
    {
        _mainWindowViewModel.AppendLog("Getting Actual Cash...");

        if (!_databaseService.IsConnected)
        {
            _mainWindowViewModel.AppendLog("Failed to get Actual Cash. Database not connected!");
            return;
        }

        ActualCash =
            await _databaseService.GetKasAktualAsync(Date, Shift, Station);
        decimal? kasComp = await _databaseService.GetKasComAsync(Date, Shift, Station);
        decimal? salesCash = await _databaseService.GetKSalesCashAsync(Date, Shift, Station);
        decimal? stationKasAktual = await _databaseService.GetStationKasAktualAsync(Date, Shift, Station);
        decimal? mtranToday = await _databaseService.GetMtranTodayAsync(Date, Shift, Station);

        _mainWindowViewModel.AppendLog($"KAS_AKTUAL: {ActualCash} | KAS_COMP: {kasComp} | SALES_CASH: {salesCash} | STATION_KAS_AKTUAL: {stationKasAktual} | Mtran: {mtranToday}");
    }
}
