using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;

namespace IDMToolsEX.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // private readonly IdmUtils _utils = new();

    [ObservableProperty] private bool isPaneOpen;

    [RelayCommand]
    private void TogglePane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    [RelayCommand]
    private void OpenActualCashWindow()
    {
        // var window = new ActualCashWindow();
        // window.Show();
    }

    [RelayCommand]
    private void OpenProgram(string name)
    {
        IdmUtils utils = new();
        utils.RunAppAsAdmin(name);
    }
}
