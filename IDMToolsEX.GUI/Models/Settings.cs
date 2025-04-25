using System.Collections.Generic;

namespace IDMToolsEX.Models;

public class Settings
{
    public DatabaseCredentials DatabaseCredentials { get; set; } = new();
    public List<string> HematPluList { get; set; } = [];
    public List<string> MurahPluList { get; set; } = [];
    public List<string> HebohPluList { get; set; } = [];
    public List<string> BadBarcodeList { get; set; } = [];
}
