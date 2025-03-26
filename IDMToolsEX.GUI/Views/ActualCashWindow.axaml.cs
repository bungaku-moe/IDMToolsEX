using Avalonia.Controls;
using IDMToolsEX.Lib;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX.Views;

public class ActualCashWindow : Window
{
    public ActualCashWindow(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        DataContext = new ActualCashWindowViewModel(mainWindowViewModel, databaseService);
        InitializeComponent();
    }
}
