using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Views;

namespace IDMToolsEX.ViewModels;

public partial class AppViewModel : ViewModelBase
{
    [RelayCommand]
    public void ShowWindow()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        desktop.MainWindow ??= new MainWindow
        {
            DataContext = new MainWindowViewModel()
        };

        desktop.MainWindow.WindowState = WindowState.Normal;
        desktop.MainWindow.Show();
        desktop.MainWindow.BringIntoView();
        desktop.MainWindow.Focus();
    }

    [RelayCommand]
    private static void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
            application.Shutdown();
    }
}
