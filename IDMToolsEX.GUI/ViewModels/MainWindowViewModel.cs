using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;
using IDMToolsEX.Views;

namespace IDMToolsEX.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string _database = "pos";
    [ObservableProperty] private string _databaseConnectText = "Connect";
    private DatabaseService? _databaseService;
    [ObservableProperty] private string _host = "10.52.111.2";
    [ObservableProperty] private bool _isConnected;
    [ObservableProperty] private string _password = "Nl/shZKyKgEJDNvT2DNdfJRswrXwm+yeU=WMxuByCted";
    [ObservableProperty] private string _port = "3306";
    [ObservableProperty] private string _username = "kasir";

    public MainWindowViewModel()
    {
        _databaseService = new DatabaseService(GetConnectionString());
    }

    [RelayCommand]
    private void OpenApp(string? appName)
    {
        if (string.IsNullOrWhiteSpace(appName))
            return;

        try
        {
            AppendLog($"Starting application: {appName}...");
            Process.Start(new ProcessStartInfo(appName) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            AppendLog($"Error: Failed to start {appName}. {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task ToggleDatabaseConnectionAsync()
    {
        try
        {
            AppendLog(IsConnected ? "Disconnecting from the database..." : "Connecting to the database...");

            if (_databaseService == null)
                _databaseService = new DatabaseService(GetConnectionString());

            IsConnected = await _databaseService.ToggleConnectionAsync();

            AppendLog(IsConnected ? "Connected to the database." : "Disconnected from the database.");
        }
        catch (Exception e)
        {
            AppendLog($"Database connection error: {e.Message}");
        }
        finally
        {
            DatabaseConnectText = IsConnected ? "Disconnect" : "Connect";
        }
    }

    private string GetConnectionString()
    {
        return
            $"allow user variables=true;Persist Security Info=True;server={Host};port={Port};pooling=true;user id={Username};password={Password};connection timeout=75;database={Database};";
    }

    [RelayCommand]
    private void OpenActualCashWindow()
    {
        if (_databaseService == null)
        {
            AppendLog("Error: Database service is not initialized.");
            return;
        }

        var window = new ActualCashWindow(this, _databaseService);
        window.Show();
    }
}
