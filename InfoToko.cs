using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace IDMToolsEX;

public static class InfoToko
{
    private const string PRODUCT_NAME = "POS.NET";
    private const string DATABASE_REG_KEY = $@"Software\Indomaret\{PRODUCT_NAME}\Database";
    
    public static string Get_KoneksiSQL()
    {
        string format =
            Decrypt(
                "bE378x1FcAhsJS7uPpr7/Kh8ZvW0CoYqimUf3u1zT7JXjJZ9IOcW0KhKl7vre+JD/90zSJXoEEjYnX7fZ6niK9rpnepLO9S60O9X/qgGYK7+hBpNKDS7eQ==");
        string text = gp().Trim();
        string text2 = string.Format(format, new object[]
        {
            GetServerName(),
            GetSQLPort(),
            Decrypt("HI97RIhVShE="),
            text
        });
        Console.WriteLine(text2);
        
        string sqlConnection = text2.Trim() + ";database=" + Get_NamaDB();
        Console.WriteLine(sqlConnection);
        return sqlConnection;
    }

    public static string Decrypt(string sWord)
    {
        try
        {
            byte[] key = Convert.FromBase64String("qL32tocPVnY2qztesn1bj54QD/+oD1Gk");
            byte[] iv = Convert.FromBase64String("6344Dd6xbDE=");
            using TripleDESCryptoServiceProvider tdes = new();
            using ICryptoTransform decryptor = tdes.CreateDecryptor(key, iv);
            using MemoryStream ms = new MemoryStream(Convert.FromBase64String(sWord));
            using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            byte[] decryptedBytes = new byte[sWord.Length];
            int bytesRead = cs.Read(decryptedBytes, 0, decryptedBytes.Length);
            string result = Encoding.ASCII.GetString(decryptedBytes, 0, bytesRead).Trim('\0');
            Console.WriteLine(result);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during decryption! {ex.Message}");
            return string.Empty;
        }
    }

    private static string gp()
    {
        return Directory.Exists(GetIDMPath())
            ? Decrypt("1HH3YA0QFheFRetN7hQHIIDAcQ1uoBbQtWE2miomsOgNTzY68G0LEs8FGpf93uhK")
            : string.Empty;
    }

    public static string GetIDMPath()
    {
        return Utils.ReadRegistryValue(DATABASE_REG_KEY, "IDM") ?? "D:\\IDM";
    }
    
    public static string? GetServerName()
    {
        return Utils.ReadRegistryValue(DATABASE_REG_KEY, "Server");
    }
    
    public static string? GetSQLPort()
    {
        return Utils.ReadRegistryValue(DATABASE_REG_KEY, "Port") ?? "3306";
    }
    
    public static string? Get_NamaDB()
    {
        return Utils.ReadRegistryValue(DATABASE_REG_KEY, "NamaDB");
    }
}