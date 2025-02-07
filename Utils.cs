using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace IDMToolsEX;

public static class Utils
{
    public static string? ReadRegistryValue(string sbkey, string key)
    {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(sbkey);
        return Conversions.ToString(registryKey.GetValue(key));
    }
    
    public static object DBNull(object anyValue)
    {
        return anyValue is DBNull ? 0 : anyValue;
    }
}
