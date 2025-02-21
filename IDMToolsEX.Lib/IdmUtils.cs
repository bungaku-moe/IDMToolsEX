using System.Diagnostics;

namespace IDMToolsEX.Lib;

public class IdmUtils
{
    public void RunAppAsAdmin(string appName)
    {
        try
        {
            ProcessStartInfo startInfo = new()
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = appName,
                Verb = "runas"
            };
            Process.Start(startInfo);
            Console.WriteLine($"{appName} Opened");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
