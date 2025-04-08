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
    [ObservableProperty] private string _toggleRestrictionsText = "Matikan Pembatasan Sistem";
    [ObservableProperty] private string _username = "kasir";

    public MainWindowViewModel()
    {
        _systemSecurity = new SystemSecurity();
    }

    [RelayCommand]
    private void OpenApp(string? appName)
    {
        if (string.IsNullOrWhiteSpace(appName))
            return;

        try
        {
            AppendLog($"Memulai aplikasi: {appName}...");
            Process.Start(new ProcessStartInfo(appName) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            AppendLog($"Error: Gagal memulai aplikasi {appName}. {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task ToggleDatabaseConnection()
    {
        try
        {
            AppendLog(IsConnected ? "Memutuskan koneksi ke database..." : "Menghubungkan koneksi ke database...");

            _databaseService ??= new DatabaseService(Database, Host, Port, Username, Password);
            IsConnected = await _databaseService.ToggleConnectionAsync();

            AppendLog(IsConnected ? "Tersambung ke database." : "Terputus ke database.");

            if (!IsConnected)
            {
                _databaseService.Dispose();
                _databaseService = null;
            }
        }
        catch (Exception e)
        {
            AppendLog($"Error koneksi ke database: {e.Message}");
        }
        finally
        {
            DatabaseConnectText = IsConnected ? "Putuskan" : "Sambungkan";
        }
    }

    [RelayCommand]
    private async void TestDb()
    {
        try
        {
            // AppendLog(await _databaseService.TestAsync());
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
            AppendLog("Error: Database belum disambungkan. Sambungkan ke database terlebih dahulu.");
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
                ToggleRestrictionsText = "Matikan Pembatasan Sistem";
                _systemSecurity.DisablePolicies();
                break;
            case false:
                ToggleRestrictionsText = "Pulihkan Pembatasan Sistem";
                _systemSecurity.EnablePolicies();
                break;
        }
    }
}
