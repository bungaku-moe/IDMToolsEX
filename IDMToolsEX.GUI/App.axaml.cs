using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using GlobalHotKeys;
using GlobalHotKeys.Native.Types;
using IDMToolsEX.ViewModels;

namespace IDMToolsEX;

public class App : Application
{
    private readonly AppViewModel _appViewModel;
    private HotKeyManager? _hotKeyManager;
    private IDisposable? _hotKeySubscription;

    public App()
    {
        _appViewModel = new AppViewModel();
        DataContext = _appViewModel;
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            CultureInfo.CurrentCulture = new CultureInfo("id-ID");
            CultureInfo.CurrentUICulture = new CultureInfo("id-ID");

            _hotKeyManager = new HotKeyManager();
            _hotKeySubscription = _hotKeyManager.Register(VirtualKeyCode.VK_HOME, Modifiers.Control);

            _hotKeyManager.HotKeyPressed
                .Subscribe(hotKey => { Dispatcher.UIThread.Post(() => { _appViewModel.ShowWindow(); }); });

            desktop.Exit += (sender, args) =>
            {
                _hotKeySubscription.Dispose();
                _hotKeyManager.Dispose();
            };



            // desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Show Main Window on startup
            // _appViewModel.ShowWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
