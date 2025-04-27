using System;
using Avalonia.Controls;
using IDMToolsEX.Lib;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX.Views;

public partial class ActualCashWindow : Window
{
    private readonly ActualCashWindowViewModel _viewModel;

    public ActualCashWindow(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        _viewModel = new ActualCashWindowViewModel(mainWindowViewModel, databaseService);
        DataContext = _viewModel;
        Loaded += async (_, _) =>
        {
            await _viewModel.Initialize();
            InitializeComponent();
        };
    }

    private void OnSelectedDateChanged(object? sender, DatePickerSelectedValueChangedEventArgs e)
    {
        _viewModel.Date = e.NewDate;
        _viewModel.CalculateActualCash();
    }

    private void OnShiftChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        _viewModel.Shift = Convert.ToInt32(e.NewValue);
        _viewModel.CalculateActualCash();
    }

    private void OnStationChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        _viewModel.Station = Convert.ToInt32(e.NewValue);
        _viewModel.CalculateActualCash();
    }
}
