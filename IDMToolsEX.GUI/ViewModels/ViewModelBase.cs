using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace IDMToolsEX.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty] private string _logText = "";

    public void AppendLog(string message)
    {
        LogText += $"{DateTime.Now:HH:mm:ss} - {message}\n";
    }

    [RelayCommand]
    private void ClearLog()
    {
        LogText = string.Empty;
    }
}
