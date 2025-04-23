using CommunityToolkit.Mvvm.ComponentModel;

namespace IDMToolsEX.Models;

public partial class SaleItem : ObservableObject
{
    [ObservableProperty] private string? _name = "NO NAME";
    [ObservableProperty] private string? _plu = "NO PLU";
    [ObservableProperty] private decimal? _price = 0;
    [ObservableProperty] private int? _qty = 0;
    [ObservableProperty] private string? _rtype = "NO TYPE";
    [ObservableProperty] private string? _time = "NO TIME";
}
