using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using IDMToolsEX.Models;
using IDMToolsEX.Utility;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX.Views;

public partial class MainWindow : Window
{
    private static readonly string[] _databaseSchemaSuggestion = ["pos"];
    private static readonly string[] _databaseHostsSuggestion = ["10.52.111.2", "localhost"];
    private static readonly string[] _databasePortsSuggestion = ["3306"];
    private static readonly string[] _databaseUsernameSuggestion = ["kasir", "root"];

    private static readonly string[] _databasePasswordsSuggestion =
        ["Nl/shZKyKgEJDNvT2DNdfJRswrXwm+yeU=WMxuByCted", "135321"];

    public MainWindow()
    {
        InitializeComponent();
        Initialization();
        Closed += (_, _) =>
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
            UpdateSettings();
            desktop.MainWindow = null;
        };
        Closing += MainWindow_Closing;
    }

    private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
    {
        UpdateSettings();
        // Cancel the default close behavior
        e.Cancel = true;
        // Hide all open windows
        foreach (var window in GetOpenWindows())
            window.Hide();
    }

    // Get all open windows in the application
    private IEnumerable<Window> GetOpenWindows()
    {
        return ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Windows;
    }

    private void Initialization()
    {
        DatabaseNameTextBox.ItemsSource = _databaseSchemaSuggestion.OrderBy(x => x);
        DatabaseHostTextBox.ItemsSource = _databaseHostsSuggestion.OrderBy(x => x);
        DatabasePortTextBox.ItemsSource = _databasePortsSuggestion.OrderBy(x => x);
        DatabaseUsernameTextBox.ItemsSource = _databaseUsernameSuggestion.OrderBy(x => x);
        DatabasePasswordTextBox.ItemsSource = _databasePasswordsSuggestion.OrderBy(x => x);
    }

    private void UpdateSettings()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime application) return;

        var mainWindow = application.MainWindow.DataContext as MainWindowViewModel;
        var settingsLoader = new SettingsLoader();
        var settings = new
        {
            DatabaseCredentials = new DatabaseCredentials
            {
                Database = DatabaseNameTextBox.Text,
                Server = DatabaseHostTextBox.Text,
                Port = Convert.ToInt16(DatabasePortTextBox.Text),
                Username = DatabaseUsernameTextBox.Text,
                Password = DatabasePasswordTextBox.Text
            }
        };
        mainWindow.Settings.DatabaseCredentials = settings.DatabaseCredentials;
        settingsLoader.SaveSettings(mainWindow.Settings);
    }
}
