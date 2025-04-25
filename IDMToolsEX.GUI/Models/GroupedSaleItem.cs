using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace IDMToolsEX.Models;

public partial class GroupedSaleItem : ObservableObject
{
    [ObservableProperty] private string? _plu;
    [ObservableProperty] private string? _name;
    [ObservableProperty] private ObservableCollection<SaleItem>? _items;
    [ObservableProperty] private int? _itemsTotalQty = 0;
}
