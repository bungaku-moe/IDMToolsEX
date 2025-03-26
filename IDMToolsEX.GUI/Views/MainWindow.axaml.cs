using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace IDMToolsEX.Views;

public partial class MainWindow : Window
{
    private static readonly string[] _databaseHostsSuggestion = ["10.52.111.2"];
    private static readonly string[] _databasePortsSuggestion = ["3306"];
    private static readonly string[] _databasePasswordsSuggestion = ["Nl/shZKyKgEJDNvT2DNdfJRswrXwm+yeU=WMxuByCted"];

    public MainWindow()
    {
        InitializeComponent();
        Initialization();
        Closed += (_, _) =>
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = null;
        };
    }

    private void Initialization()
    {
        DatabaseHostTextBox.ItemsSource = _databaseHostsSuggestion.OrderBy(x => x);
        DatabasePortTextBox.ItemsSource = _databasePortsSuggestion.OrderBy(x => x);
        DatabasePasswordTextBox.ItemsSource = _databasePasswordsSuggestion.OrderBy(x => x);
    }
}
