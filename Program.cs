using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using MySql.Data.MySqlClient;

namespace IDMToolsEX;

class Program
{
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("user32.dll")]
    private static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    private const uint MOD_CTRL = 0x0002;
    private const uint VK_HOME = 0x24; // Home key
    private const int HOTKEY_ID = 1;
    private static readonly string filePath = "C:\\Temp\\log.txt"; // Change path as needed

    static void Main()
    {
        Console.WriteLine("Press Ctrl + Home to save text and open it in Notepad. Press Ctrl + X to exit.");

        // Register hotkeys
        RegisterHotKey(IntPtr.Zero, HOTKEY_ID, MOD_CTRL, VK_HOME);
        RegisterHotKey(IntPtr.Zero, HOTKEY_ID + 1, MOD_CTRL, 0x58); // 'X' key to exit

        MSG msg;
        while (GetMessage(out msg, IntPtr.Zero, 0, 0) > 0)
        {
            if (msg.message == 0x0312) // WM_HOTKEY message
            {
                int key = msg.wParam.ToInt32();
                if (key == HOTKEY_ID)
                {
                    Console.WriteLine("Ctrl + Home pressed! Saving text...");

                    // Get timestamp
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string text = $"[{timestamp}] Hello, this is auto-saved text!\n";

                    // Save to file
                    SaveToFile(text);

                    // Open in Notepad
                    OpenInNotepad();
                    
                    ConnectDatabase();
                }
                else if (key == HOTKEY_ID + 1)
                {
                    Console.WriteLine("Ctrl + X pressed! Exiting...");
                    break;
                }
            }
        }

        // Unregister hotkeys before exiting
        UnregisterHotKey(IntPtr.Zero, HOTKEY_ID);
        UnregisterHotKey(IntPtr.Zero, HOTKEY_ID + 1);
    }

    static void SaveToFile(string text)
    {
        try
        {
            // Ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Append text to file
            File.AppendAllText(filePath, text);
            Console.WriteLine($"Text saved to {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
        }
    }

    static void OpenInNotepad()
    {
        try
        {
            Process.Start("notepad.exe", filePath);
            Console.WriteLine("Opening file in Notepad...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening Notepad: {ex.Message}");
        }
    }

    private static void ConnectDatabase()
    {
        MySqlCommand Mcom = new();
        MySqlDataAdapter Mda = new();
        MySqlConnection Mcon = new();
        
        // flag2 = MainMdl.IsSector;
        // if (flag2)
        // {
            // Mcon = MainMdl.IdmSector.GetVersionV2(MainMdl.MyKey, Application.StartupPath + "\\POS2.exe", "kasir", null, null, null, null);
            // Mcom.Connection = Mcon;
        // }
        // else
        // {
        Mcon = new MySqlConnection($"allow user variables=true;{InfoToko.Get_KoneksiSQL()}");
        Mcom.Connection = Mcon;
        // }
        // Mcon.Open();
        
        // string tglIndo = string.Format(DateTime.Now.ToString(), "yyyy-MM-dd");
        // string sTanggal = $"{DateTime.Now:yy}{tglIndo[^2..]}-{tglIndo.Substring(3, 2)}-{tglIndo.Substring(0, 2)}";
        // DateTime tgl_aktual = DateTime.Parse(sTanggal);
        
        // Mcom.CommandText = $"SELECT COUNT(*) FROM Initial WHERE Recid='' AND Tanggal='{sTanggal}' AND Station='{sStation}' AND Shift='{sShift}'";
        Console.WriteLine("Enter Shift:");
        int shift = Convert.ToInt32(Console.ReadLine());
        
        Console.WriteLine("Enter Station:");
        int station = Convert.ToInt32(Console.ReadLine());
        GetAndPrintKasAktualAndTotalSales(Mcon, shift, station);
    }
    
    static void GetAndPrintKasAktualAndTotalSales(MySqlConnection sqlConnection, int shift, int station)
    {
        // string connectionString = "your_connection_string_here"; // Replace with your database connection string
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        using (MySqlConnection conn = sqlConnection)
        {
            try
            {
                conn.Open();

                // Query to get Kas_Aktual from Initial table
                string queryKasAktual = $@"
                    SELECT Kas_Aktual FROM Initial 
                    WHERE Tanggal = '{currentDate}' 
                    AND Shift = '{shift}' 
                    AND Station = '{station}'";

                using (MySqlCommand cmd = new MySqlCommand(queryKasAktual, conn))
                {
                    object result = cmd.ExecuteScalar();
                    int kasAktual = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    Console.WriteLine($"Kas Aktual: {kasAktual}");
                }

                // Query to get total gross sales from Mtran table
                string queryTotalGross = $@"
                    SELECT SUM(Gross) FROM Mtran 
                    WHERE Tanggal = '{currentDate}' 
                    AND Shift = '{shift}' 
                    AND Station = '{station}'";

                using (MySqlCommand cmd = new MySqlCommand(queryTotalGross, conn))
                {
                    object result = cmd.ExecuteScalar();
                    int totalGross = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    Console.WriteLine($"Total Gross Sales: {totalGross}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

// Struct for Windows messages
[StructLayout(LayoutKind.Sequential)]
struct MSG
{
    public IntPtr hWnd;
    public uint message;
    public IntPtr wParam;
    public IntPtr lParam;
    public uint time;
    public POINT pt;
}

[StructLayout(LayoutKind.Sequential)]
struct POINT
{
    public int x;
    public int y;
}