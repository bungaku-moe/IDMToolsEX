using System.Diagnostics;
using GlobalHotKeys;
using GlobalHotKeys.Native.Types;

namespace Simple_Windows_Calculator;

internal static class Program
{
    private static HotKeyManager? _hotKeyManager;

    private static string IdmToolPath =>
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IDMToolsEX", "IDMToolsEX.GUI.exe");

    [STAThread]
    private static void Main()
    {
        _hotKeyManager = new HotKeyManager();
        _hotKeyManager.Register(VirtualKeyCode.VK_HOME, Modifiers.Control);
        _hotKeyManager.HotKeyPressed
            .Subscribe(hotKey => { Process.Start(new ProcessStartInfo(IdmToolPath) { UseShellExecute = true }); });

        // Dispose HotKeyManager when the app exits
        Application.ApplicationExit += (_, _) => _hotKeyManager?.Dispose();

        ApplicationConfiguration.Initialize();
        Application.Run(new SimpleCalculator());
    }
}
