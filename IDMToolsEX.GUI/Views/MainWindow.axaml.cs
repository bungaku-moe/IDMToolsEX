using System.Linq;
using Avalonia.Controls;

namespace IDMToolsEX.Views;

public partial class MainWindow : Window
{
    private static readonly string[] _databaseHostsSuggestion = ["10.52.76.66", "10.52.147.162"];
    private static readonly string[] _databasePortsSuggestion = ["3306", "3307"];
    private static readonly string[] _databasePasswordsSuggestion = ["Nl/shZKyKgEJDNvT2DNdfJRswrXwm+yeU=WMxuByCted"];

    public MainWindow()
    {
        InitializeComponent();
        Initialization();
    }

    private void Initialization()
    {
        DatabaseHostTextBox.ItemsSource = _databaseHostsSuggestion.OrderBy(x => x);
        DatabasePortTextBox.ItemsSource = _databasePortsSuggestion.OrderBy(x => x);
        DatabasePasswordTextBox.ItemsSource = _databasePasswordsSuggestion.OrderBy(x => x);
    }
}