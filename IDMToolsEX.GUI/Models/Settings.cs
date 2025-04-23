using System.Collections.Generic;

namespace IDMToolsEX.Models;

public class Settings
{
    public DatabaseCredentials DatabaseCredentials { get; set; } = new();
    public List<string> SalesReportPluList { get; set; } = [];
    public List<string> BadBarcodeList { get; set; } = [];
}
