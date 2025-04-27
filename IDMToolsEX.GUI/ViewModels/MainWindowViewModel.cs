using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IDMToolsEX.Lib;
using IDMToolsEX.Models;
using IDMToolsEX.Utility;

namespace IDMToolsEX.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly SystemSecurity _systemSecurity;
    [ObservableProperty] private string _databaseConnectText = "💖 Hubungkan";
    private DatabaseService? _databaseService;
    [ObservableProperty] private bool _isConnected;
    [ObservableProperty] private Settings _settings;

    [ObservableProperty] private SettingsLoader _settingsLoader;
    // [ObservableProperty] private string _toggleRestrictionsText = "Matikan Batasan Sistem";

    public MainWindowViewModel()
    {
        _systemSecurity = new SystemSecurity();
        SettingsLoader = new SettingsLoader();
        Settings = SettingsLoader.LoadSettings();
        InitializeDatabaseConnectionAsync();
    }

    private async void InitializeDatabaseConnectionAsync()
    {
        await ToggleDatabaseConnection();
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
            AppendLog($"Error memulai aplikasi {appName}. {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task ToggleDatabaseConnection()
    {
        try
        {
            AppendLog(IsConnected ? "Memutuskan koneksi ke database..." : "Menghubungkan koneksi ke database...");

            _databaseService ??= new DatabaseService(Settings.DatabaseCredentials.Database,
                Settings.DatabaseCredentials.Server, Settings.DatabaseCredentials.Port.ToString(),
                Settings.DatabaseCredentials.Username, Settings.DatabaseCredentials.Password);

            if (IsConnected)
            {
                await _databaseService.DisconnectAsync();
                _databaseService.Dispose();
                _databaseService = null;
            }
            else
            {
                IsConnected = await _databaseService.ConnectAsync();
            }

            AppendLog(IsConnected ? "Terkoneksi ke database." : "Terputus dari database.");
        }
        catch (Exception e)
        {
            AppendLog($"Error koneksi ke database: {e.Message}");
        }
        finally
        {
            DatabaseConnectText = IsConnected ? "💔 Putuskan" : "💖 Hubungkan";
        }
    }

    [RelayCommand]
    private void ToggleRestrictions()
    {
        // switch (_systemSecurity.ArePoliciesEnabled())
        // {
        //     case true:
        // ToggleRestrictionsText = "Matikan Batasan Sistem";
        _systemSecurity.DisablePolicies();
        //         break;
        //     case false:
        //         ToggleRestrictionsText = "Pulihkan Batasan Sistem";
        //         _systemSecurity.EnablePolicies();
        //         break;
        // }
    }

    [RelayCommand]
    private void OpenWindow(string className)
    {
        if (_databaseService == null)
        {
            AppendLog("Error! Database belum dihubungkan. Silahkan hubungkan terlebih dahulu.");
            return;
        }

        var windowType = Type.GetType($"IDMToolsEX.Views.{className}");
        if (windowType == null)
        {
            AppendLog($"Error! Tidak dapat menemukan jendela dengan nama: {className}.");
            return;
        }

        if (Activator.CreateInstance(windowType, this, _databaseService) is not Window window)
        {
            AppendLog($"Error! Tidak dapat membuat instance jendela: {className}.");
            return;
        }

        window.Show();
    }
}
