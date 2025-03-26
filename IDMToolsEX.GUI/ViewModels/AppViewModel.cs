using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Views;

namespace IDMToolsEX.ViewModels;

public partial class AppViewModel : ViewModelBase
{
    private readonly MainWindow _mainWindow;

    public AppViewModel()
    {
        _mainWindow = new MainWindow
        {
            DataContext = new MainWindowViewModel()
        };
    }

    [RelayCommand]
    private void ShowWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (desktop.MainWindow == null)
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel()
                };

            desktop.MainWindow.WindowState = WindowState.Normal;
            desktop.MainWindow.Show();
            desktop.MainWindow.BringIntoView();
            desktop.MainWindow.Focus();
        }
    }

    [RelayCommand]
    private static void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
            application.Shutdown();
    }
}
