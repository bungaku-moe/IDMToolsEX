using Avalonia.Controls;
using IDMToolsEX.Lib;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX.Views;

public partial class BarcodeWindow : Window
{
    public BarcodeWindow(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        DataContext = new BarcodeWindowViewModel(mainWindowViewModel, databaseService);
        InitializeComponent();
    }
}
