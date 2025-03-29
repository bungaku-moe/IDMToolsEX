using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;
using IDMToolsEX.Utility;
using IDMToolsEX.Views;

namespace IDMToolsEX.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly SystemSecurity _systemSecurity;
    [ObservableProperty] private string _database = "pos";
    [ObservableProperty] private string _databaseConnectText = "Connect";
    private DatabaseService? _databaseService;
    [ObservableProperty] private string _host = "10.52.111.2";
    [ObservableProperty] private bool _isConnected;
    [ObservableProperty] private string _password = "Nl/shZKyKgEJDNvT2DNdfJRswrXwm+yeU=WMxuByCted";
    [ObservableProperty] private string _port = "3306";
    [ObservableProperty] private string _toggleRestrictionsText = "Disable Restrictions";
    [ObservableProperty] private string _username = "kasir";

    public MainWindowViewModel()
    {
        _systemSecurity = new SystemSecurity();
        // _databaseService = new DatabaseService(Database, Host, Port, Username, Password);
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
    private async Task ToggleDatabaseConnection()
    {
        try
        {
            AppendLog(IsConnected ? "Disconnecting from the database..." : "Connecting to the database...");

            _databaseService = new DatabaseService(Database, Host, Port, Username, Password);
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

    [RelayCommand]
    private async void TestDb()
    {
        try
        {
            AppendLog(await _databaseService.TestAsync());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    [RelayCommand]
    private void OpenActualCashWindow()
    {
        if (_databaseService == null)
        {
            AppendLog("Error: Database service is not initialized. Please connect to the database first.");
            return;
        }

        var window = new ActualCashWindow(this, _databaseService);
        window.Show();
    }

    [RelayCommand]
    private void ToggleRestrictions()
    {
        switch (_systemSecurity.ArePoliciesEnabled())
        {
            case true:
                ToggleRestrictionsText = "Disable Restrictions";
                _systemSecurity.DisablePolicies();
                break;
            case false:
                ToggleRestrictionsText = "Enable Restrictions";
                _systemSecurity.EnablePolicies();
                break;
        }
    }
}
