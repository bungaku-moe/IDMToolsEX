using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace IDMToolsEX.Models;

public partial class GroupedSaleItem : ObservableObject
{
    [ObservableProperty] private ObservableCollection<SaleItem>? _items;
    [ObservableProperty] private int? _itemsTotalQty = 0;
    [ObservableProperty] private string? _name;
    [ObservableProperty] private string? _plu;
}
