using Avalonia.Controls;
using IDMToolsEX.Lib;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX.Views;

public partial class ActualCashWindow : Window
{
    public ActualCashWindow(MainWindowViewModel mainWindowViewModel, DatabaseService databaseService)
    {
        DataContext = new ActualCashWindowViewModel(mainWindowViewModel, databaseService);
        Loaded += (_, _) =>
        {
            if (DataContext is not ActualCashWindowViewModel viewModel) return;
            viewModel.Initialize();
            InitializeComponent();
        };
    }
}
